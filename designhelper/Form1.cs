using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace designhelper
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient();
        Database db = new Database();

        string apiKey = "RGAPI-380f1d21-de69-4b2c-aebc-ab7774e2aa3b";

        const HttpStatusCode HttpStatusCodeTooManyRequests = (HttpStatusCode)429;
        int requestCounter = 0;
        int tenSecondsInMs = 10000;

        private List<long> playerIds;
        private List<long> matchIds;
        private Dictionary<long, string> matchData;

        public Form1()
        {
            InitializeComponent();
        }

        private async void getMatches_Click(object sender, EventArgs e)
        {
            string input = summonerIdInput.Text;
            if (input != "" && input.All(char.IsDigit))
            {
                try
                {
                    summonerIdInput.Enabled = false;
                    getMatchesButton.Enabled = false;
                    await Add10MatchIdsAnd90playerIds(input);
                    playerIds.Remove(long.Parse(input));

                    while (matchIds.Count < 1000)
                    {
                        await Add10MatchIdsAnd90playerIds(playerIds.First().ToString());
                        playerIds.Remove(playerIds.First());
                    }
                }
                finally
                {
                    MessageBox.Show("Der er: " + matchIds.Count + " matchIds!");
                }
            }
            else
                MessageBox.Show("Input må kun være tal!");

            foreach (var matchId in matchIds)
            {
                await GetMatchById(matchId);
            }

            MessageBox.Show("Færdig med at hente matchdata!");
            fillDbWithMatchInfo.Visible = true;
        }

        private async Task Add10MatchIdsAnd90playerIds(string summonerId)
        {
            if (!playerIds.Contains(long.Parse(summonerId)))
                playerIds.Add(long.Parse(summonerId));

            await WaitTenSecondsIfTooManyRequests();

            requestCounter += 1;
            var response = await client.GetAsync(
                "https://euw.api.pvp.net/api/lol/euw/v1.3/game/by-summoner/" + summonerId + "/recent?api_key=" + apiKey);

            // hvis responset er rigtigt bliver der addet 10 gameIds og 90 playerIds 
            // ellers vises en messagebox med fejlmeddelse
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                var recentGameDto = JsonConvert.DeserializeObject<RecentGameDto>(json);

                foreach (var game in recentGameDto.Games)
                {
                    var gameMode = game.SubType;

                    if (gameMode == "RANKED_SOLO_5x5" || gameMode == "RANKED_PREMADE_5x5" || gameMode == "RANKED_TEAM_5x5")
                    {
                        if (!matchIds.Contains(game.GameId))
                            matchIds.Add(game.GameId);
                    }

                    // adder kun playerIds hvis der er mindre end 110 playerIds (9 ad gangen)
                    if (playerIds.Count < 110)
                        Add9playerIds(game);
                }
            }
            else if (response.StatusCode == HttpStatusCodeTooManyRequests)
            {
                await HandleTooManyRequests(response);

                requestCounter += 1;
                await Add10MatchIdsAnd90playerIds(summonerId);
            }
            else
            {
                MessageBox.Show("Noget gik galt! Fejl " + response.StatusCode + "\n Prøver igen!");

                requestCounter += 1;
                await Add10MatchIdsAnd90playerIds(summonerId);
            }
        }

        private void Add9playerIds(GameDto game)
        {
            foreach (var player in game.FellowPlayers)
            {
                if(!playerIds.Contains(player.SummonerId))
                    playerIds.Add(player.SummonerId);
            }
        }

        private async Task GetMatchById(long id)
        {
            await WaitTenSecondsIfTooManyRequests();

            requestCounter += 1;
            var response = await client.GetAsync(
                "https://euw.api.pvp.net/api/lol/euw/v2.2/match/" + id + "?api_key=" + apiKey);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var matchDetails = await response.Content.ReadAsStringAsync();
                matchData[id] = matchDetails;
            }
            else if (response.StatusCode == HttpStatusCodeTooManyRequests)
            {
                await HandleTooManyRequests(response);

                requestCounter += 1;
                await GetMatchById(id);
            }
            else
            {
                MessageBox.Show("Noget gik galt! Fejl " + response.StatusCode + "\n Prøver igen!");

                requestCounter += 1;
                await GetMatchById(id);
            }
        }

        private void Form1_Load(Object sender, EventArgs e)
        {
            playerIds = new List<long>();
            matchIds = new List<long>();
            matchData = new Dictionary<long, string>();

            db.InitDatabase();
        }

        private async Task WaitTenSecondsIfTooManyRequests()
        {
            if (requestCounter >= 8)
            {
                await Task.Delay(tenSecondsInMs);
                requestCounter = 0;
            }
        }

        private void fillDbWithMatchInfo_Click(object sender, EventArgs e)
        {
            foreach (var id in matchIds)
            {
                string sql = @"INSERT OR REPLACE INTO matchTable (matchId, match) 
                               VALUES (" + id + ", '" + matchData[id] + "')";
                db.Execute(sql);
            }
            if (MessageBox.Show("Data er nu lagt i database") == DialogResult.OK)
            {
                Application.Exit();
            }
            
        }

        private async Task HandleTooManyRequests(HttpResponseMessage response)
        {
            if (response.Headers.Contains("Retry-After"))
            {
                int retryAfter = int.Parse(response.Headers.GetValues("Retry-After").First());
                MessageBox.Show("For mange requests! Venter " + retryAfter + " sekunder. \nHÆNG LIGE PÅ");
                await Task.Delay(retryAfter * 1000);
            }
            else
            {
                await Task.Delay(1000);
            }
        }
    }
}