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

        private List<string> info = new List<string>();
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
                    // tager kun ranked games
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

            // fill the matchData with existing data
            string sql = "SELECT * FROM matchTable";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, db.m_dbConnection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        matchData[(long)reader["matchId"]] = (string)reader["match"];
                    }
                }
            }

            if (matchData.Count > 1000)
                getMatchesButton.Enabled = false;
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

        private void kdaButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;

            foreach (var val in matchData.Values)
            {
                double kdaTeamA = 0;
                double kdaTeamB = 0;

                var match = JsonConvert.DeserializeObject<Match>(val);
                Team teamA = match.Teams.Find(t => t.TeamId == 100);
                Team teamB = match.Teams.Find(t => t.TeamId == 200);

                foreach (var p in match.Participants)
                {
                    double kda = 0;
                    if (p.Stats.Deaths != 0)
                        kda += ((double)p.Stats.Kills + p.Stats.Assists) / p.Stats.Deaths;
                    else
                        kda += (double)p.Stats.Kills + p.Stats.Assists;

                    if (p.TeamId == 100) kdaTeamA += kda;
                    else kdaTeamB += kda;
                }

                if (kdaTeamA > kdaTeamB && teamA.Winner == true) trueCases += 1;
                else if (kdaTeamB > kdaTeamA && teamB.Winner == true) trueCases += 1;
            }

            MessageBox.Show("Holdet med højest KDA vinder i gennemsnit " + (double)trueCases / matchData.Count() * 100 + "% af spillene");            
        }

        private void firstTowerButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);
                Team teamA = match.Teams.Find(t => t.TeamId == 100);
                Team teamB = match.Teams.Find(t => t.TeamId == 200);

                if (teamA.Winner == true && teamA.FirstTower == true) trueCases += 1;
                if (teamB.Winner == true && teamB.FirstTower == true) trueCases += 1;
            }

            MessageBox.Show("Holdet der får first tower vinder i gennemsnit " + (double)trueCases / matchData.Count() * 100 + "% af spillene");
        }

        private void firstBloodButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);
                Team teamA = match.Teams.Find(t => t.TeamId == 100);
                Team teamB = match.Teams.Find(t => t.TeamId == 200);

                if (teamA.Winner == true && teamA.FirstBlood == true) trueCases += 1;
                if (teamB.Winner == true && teamB.FirstBlood == true) trueCases += 1;
            }

            MessageBox.Show("Holdet der får first tower vinder i gennemsnit " + (double)trueCases / matchData.Count() * 100 + "% af spillene");
        }

        private void killParticipationButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;

            foreach (var val in matchData.Values)
            {
                long killsTeamA = 0;
                long killsTeamB = 0;
                double killParticipationTeamA = 0;
                double killParticipationTeamB = 0;

                var match = JsonConvert.DeserializeObject<Match>(val);
                Team teamA = match.Teams.Find(t => t.TeamId == 100);
                Team teamB = match.Teams.Find(t => t.TeamId == 200);

                // udregner total team kills
                foreach (var p in match.Participants)
                {
                    if (p.TeamId == 100) killsTeamA += p.Stats.Kills;
                    else killsTeamB += p.Stats.Kills;
                }

                // udregner samlet kill participation for hvert hold
                foreach (var p in match.Participants)
                {
                    var participation = ((double)p.Stats.Kills + p.Stats.Assists) / killsTeamA;
                    if (p.TeamId == 100) killParticipationTeamA += participation;
                    else killParticipationTeamB += participation;
                }

                if (killParticipationTeamA > killParticipationTeamB && teamA.Winner == true) trueCases += 1;
                else if (killParticipationTeamB > killParticipationTeamA && teamB.Winner == true) trueCases += 1;
            }

            MessageBox.Show("Holdet med bedst kill participation vinder i gennemsnit " + (double)trueCases / matchData.Count() * 100 + "% af spillene");
        }

        private void baronKillsButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int usedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);
                Team teamA = match.Teams.Find(t => t.TeamId == 100);
                Team teamB = match.Teams.Find(t => t.TeamId == 200);

                if (teamA.BaronKills + teamB.BaronKills > 0)
                {
                    if (teamA.BaronKills > teamB.BaronKills && teamA.Winner == true) trueCases += 1;
                    else if (teamB.BaronKills > teamA.BaronKills && teamB.Winner == true) trueCases += 1;

                    usedCases += 1;
                }
            }

            MessageBox.Show("Holdet med flest baron kills vinder i gennemsnit " + (double)trueCases / usedCases * 100 + "% af spillene (" + trueCases + " ud af " + usedCases + ")");
        }

        private void dragonKillsButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int usedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);
                Team teamA = match.Teams.Find(t => t.TeamId == 100);
                Team teamB = match.Teams.Find(t => t.TeamId == 200);

                if (teamA.DragonKills + teamB.DragonKills > 0)
                {
                    if (teamA.DragonKills > teamB.DragonKills && teamA.Winner == true) trueCases += 1;
                    else if (teamB.DragonKills > teamA.DragonKills && teamB.Winner == true) trueCases += 1;

                    usedCases += 1;
                }
            }

            MessageBox.Show("Holdet med flest dragon kills vinder i gennemsnit " + (double)trueCases / usedCases * 100 + "% af spillene (" + trueCases + " ud af " + usedCases + ")");
        }

        private void baronAndBaronKillsButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int usedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);
                Team teamA = match.Teams.Find(t => t.TeamId == 100);
                Team teamB = match.Teams.Find(t => t.TeamId == 200);

                if (teamA.DragonKills + teamA.BaronKills + teamB.DragonKills + teamB.BaronKills > 0)
                {
                    if (teamA.DragonKills + teamA.BaronKills > teamB.DragonKills + teamB.BaronKills && teamA.Winner == true) trueCases += 1;
                    else if (teamA.DragonKills + teamA.BaronKills < teamB.DragonKills + teamB.BaronKills && teamB.Winner == true) trueCases += 1;

                    usedCases += 1;
                }
            }

            MessageBox.Show("Holdet med flest dragon og baron kills vinder i gennemsnit " + (double)trueCases / usedCases * 100 + "% af spillene (" + trueCases + " ud af " + usedCases + ")");
        }

        private void creepsButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);
                Team teamA = match.Teams.Find(t => t.TeamId == 100);
                Team teamB = match.Teams.Find(t => t.TeamId == 200);

                long creepsTeamA = 0;
                long creepsTeamB = 0;

                foreach (var p in match.Participants)
                {
                    if (p.TeamId == 100) creepsTeamA += p.Stats.MinionsKilled + p.Stats.NeutralMinionsKilled;
                    else creepsTeamB += p.Stats.MinionsKilled + p.Stats.NeutralMinionsKilled;
                }

                if (creepsTeamA > creepsTeamB && teamA.Winner == true) trueCases += 1;
                else if (creepsTeamB > creepsTeamA && teamB.Winner == true) trueCases += 1;
            }

            MessageBox.Show("Holdet med flest creep kills vinder i gennemsnit " + (double)trueCases / matchData.Count * 100 + "% af spillene");
        }

        private void wardsSupportButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);
                Team teamA = match.Teams.Find(t => t.TeamId == 100);
                Team teamB = match.Teams.Find(t => t.TeamId == 200);

                long wardsScoreTeamA = 0;
                long wardsScoreTeamB = 0;

                foreach (var p in match.Participants)
                {
                    if (p.Timeline.Role == "DUO_SUPPORT")
                    {
                        long wardScore = p.Stats.WardsKilled + p.Stats.WardsPlaced;
                        if (p.TeamId == 100) wardsScoreTeamA += wardScore;
                        else wardsScoreTeamB += wardScore;
                    }
                }

                if (wardsScoreTeamA > wardsScoreTeamB && teamA.Winner == true) trueCases += 1;
                else if (wardsScoreTeamB > wardsScoreTeamA && teamB.Winner == true) trueCases += 1;
            }

            MessageBox.Show("Holdet med supporten der dræber og placerer flest wards vinder i gennemsnit " + (double)trueCases / matchData.Count * 100 + "% af spillene");
        }

        private void structureDmgButton_Click(object sender, EventArgs e)
        {
            // Den findes under recentgames i GameDto objekter
        }

        private void minionsPrMinAdcButton_Click(object sender, EventArgs e)
        {
            string result = "";

            for (double minionsPr2Min = 1; minionsPr2Min < 21; minionsPr2Min++)
            {
                int trueCases = 0;
                int usedCases = 0;
                foreach (var val in matchData.Values)
                {
                    var match = JsonConvert.DeserializeObject<Match>(val);
                    var winningTeam = match.Teams.Find(t => t.Winner == true);
                    var adcTeamA = match.Participants.Find(p => p.TeamId == 100 && p.Timeline.Role == "DUO_CARRY");
                    var adcTeamB = match.Participants.Find(p => p.TeamId == 200 && p.Timeline.Role == "DUO_CARRY");

                    if (adcTeamA != null && adcTeamB != null)
                    {
                        double csAdcTeamA = adcTeamA.Timeline.CreepsPerMinDeltas.ZeroToTen + adcTeamA.Timeline.CreepsPerMinDeltas.TenToTwenty;
                        double csAdcTeamB = adcTeamB.Timeline.CreepsPerMinDeltas.ZeroToTen + adcTeamB.Timeline.CreepsPerMinDeltas.TenToTwenty;
                        if (csAdcTeamA >= minionsPr2Min && csAdcTeamA > csAdcTeamB)
                        {
                            usedCases += 1;
                            if (adcTeamA.TeamId == winningTeam.TeamId) trueCases += 1;
                        }
                        else if (csAdcTeamB >= minionsPr2Min && csAdcTeamB > csAdcTeamA)
                        {
                            usedCases += 1;
                            if (adcTeamB.TeamId == winningTeam.TeamId) trueCases += 1;
                        }
                    }
                }
                result += "Når adc har mere end " + minionsPr2Min / 2 + " creeps pr min vinder holdet " + Math.Round(((double)trueCases / usedCases) * 100, 2) + "% af spillene (" + trueCases + "/" + usedCases + ")\n";
            }

            MessageBox.Show(result);
        }
    }
}