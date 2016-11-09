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

            MessageBox.Show("Holdet der får first blood vinder i gennemsnit " +
                Math.Round((double)trueCases / matchData.Count() * 100, 2) + 
                "% af spillene");
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
            // når adc har flere end x minions pr minut og vinder
            const int x = 15;
            int removedCases = 0;
            int trueCases = 0;

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

                    // hvis begge har over x minions pr min kigger vi ikke på det
                    if (csAdcTeamA >= x && csAdcTeamB >= x) removedCases += 1;
                    // hvis der kun er 1
                    else if (csAdcTeamA >= x || csAdcTeamB >= x)
                    {
                        if (csAdcTeamA >= x && adcTeamA.TeamId == winningTeam.TeamId) trueCases += 1;
                        else if (csAdcTeamB >= x && adcTeamB.TeamId == winningTeam.TeamId) trueCases += 1;
                    }
                    // hvis der er ingen
                    else removedCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når adc har mere end " + (double)x / 2 + " minions pr min\nvinder holdet " +
                (double)trueCases / (matchData.Count - removedCases) * 100 + "% af spillene ("
                + trueCases + "/" + (matchData.Count - removedCases) + ")");
        }

        private void moreMinionsButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                Match match = JsonConvert.DeserializeObject<Match>(val);

                Team winningTeam = match.Teams.Find(t => t.Winner == true);
                Participant adcTeamA = match.Participants.Find(p => p.TeamId == 100 && p.Timeline.Role == "DUO_CARRY");

                if (adcTeamA != null && adcTeamA.Timeline.CsDiffPerMinDeltas != null)
                {
                    var csDiffAdcTeamA = adcTeamA.Timeline.CsDiffPerMinDeltas.ZeroToTen + 
                        adcTeamA.Timeline.CsDiffPerMinDeltas.TenToTwenty;
                    if ((csDiffAdcTeamA > 0 && adcTeamA.TeamId == winningTeam.TeamId) ||
                        (csDiffAdcTeamA < 0 && adcTeamA.TeamId != winningTeam.TeamId))
                        trueCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når adc har flere cs end modstander adc\nvinder holdet " + (double)trueCases / (matchData.Count - removedCases) * 100 + "% af spillene");
        }

        private void dmgToChampsButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                Match match = JsonConvert.DeserializeObject<Match>(val);

                Participant adcTeamA = match.Participants.Find(p => p.TeamId == 100 && p.Timeline.Role == "DUO_CARRY");
                Participant adcTeamB = match.Participants.Find(p => p.TeamId == 200 && p.Timeline.Role == "DUO_CARRY");
                Team winningTeam = match.Teams.Find(t => t.Winner == true);

                if (adcTeamA != null && adcTeamB != null)
                {
                    if ((adcTeamA.Stats.TotalDamageDealtToChampions > adcTeamB.Stats.TotalDamageDealtToChampions &&
                        adcTeamA.TeamId == winningTeam.TeamId) ||
                        (adcTeamB.Stats.TotalDamageDealtToChampions > adcTeamA.Stats.TotalDamageDealtToChampions &&
                        adcTeamB.TeamId == winningTeam.TeamId)) trueCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når adc har givet mere skade til champs end modstander adc\nvinder holdet " + (double)trueCases / (matchData.Count - removedCases) * 100 + "% af spillene");
        }

        private void assistsSupportButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                var m = JsonConvert.DeserializeObject<Match>(val);

                var winningTeam = m.Teams.Find(t => t.Winner == true);
                var suppTeamA = m.Participants.Find(p => p.TeamId == 100 && p.Timeline.Role == "DUO_SUPPORT"); 
                var suppTeamB = m.Participants.Find(p => p.TeamId == 200 && p.Timeline.Role == "DUO_SUPPORT");

                if (suppTeamA != null && suppTeamB != null)
                {
                    if (suppTeamA.Stats.Assists > suppTeamA.Stats.Kills &&
                        suppTeamB.Stats.Assists > suppTeamB.Stats.Kills) removedCases += 1;
                    else if (suppTeamA.Stats.Assists > suppTeamA.Stats.Kills && suppTeamA.TeamId == winningTeam.TeamId)
                        trueCases += 1;
                    else if (suppTeamB.Stats.Assists > suppTeamB.Stats.Kills && suppTeamB.TeamId == winningTeam.TeamId)
                        trueCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når supp har flere assists per kills end modstander supp\nvinder holdet " + (double)trueCases / (matchData.Count - removedCases) * 100 + "% af spillene");
        }

        private void deathsSupportButton_Click(object sender, EventArgs e)
        {
            int trueCasesSupport = 0;
            int removedCasesSupport = 0;
            int trueCasesAdc = 0;
            int removedCasesAdc = 0;
            int trueCasesMid = 0;
            int removedCasesMid = 0;
            int trueCasesTop = 0;
            int removedCasesTop = 0;
            int trueCasesJgl = 0;
            int removedCasesJgl = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);

                var winningTeam = match.Teams.Find(t => t.Winner == true);
                var suppA = match.Participants.Find(p => p.Timeline.Role == "DUO_SUPPORT" && p.TeamId == 100);
                var suppB = match.Participants.Find(p => p.Timeline.Role == "DUO_SUPPORT" && p.TeamId == 200);
                var adcA = match.Participants.Find(p => p.Timeline.Role == "DUO_CARRY" && p.TeamId == 100);
                var adcB = match.Participants.Find(p => p.Timeline.Role == "DUO_CARRY" && p.TeamId == 200);
                var topA = match.Participants.Find(p => p.Timeline.Lane == "TOP" && p.TeamId == 100);
                var topB = match.Participants.Find(p => p.Timeline.Lane == "TOP" && p.TeamId == 200);
                var midA = match.Participants.Find(p => p.Timeline.Lane == "MIDDLE" && p.TeamId == 100);
                var midB = match.Participants.Find(p => p.Timeline.Lane == "MIDDLE" && p.TeamId == 200);
                var jglA = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 100);
                var jglB = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 200);

                if (suppA != null && suppB != null && adcA != null && adcB != null &&
                    topA != null && topB != null && midA != null && midB != null &&
                    jglA != null && jglB != null)
                {
                    List<Participant> teamA = match.Participants.FindAll(p => p.TeamId == 100);
                    List<Participant> teamB = match.Participants.FindAll(p => p.TeamId == 200);

                    if (teamA.Max(p => p.Stats.Deaths) == suppA.Stats.Deaths &&
                        teamB.Max(p => p.Stats.Deaths) == suppB.Stats.Deaths) removedCasesSupport += 1;
                    else if (teamA.Max(p => p.Stats.Deaths) == suppA.Stats.Deaths && winningTeam.TeamId == 100 ||
                        teamB.Max(p => p.Stats.Deaths) == suppB.Stats.Deaths && winningTeam.TeamId == 200) trueCasesSupport += 1;

                    if (teamA.Max(p => p.Stats.Deaths) == adcA.Stats.Deaths &&
                        teamB.Max(p => p.Stats.Deaths) == adcB.Stats.Deaths) removedCasesAdc += 1;
                    else if (teamA.Max(p => p.Stats.Deaths) == adcA.Stats.Deaths && winningTeam.TeamId == 100||
                        teamB.Max(p => p.Stats.Deaths) == adcB.Stats.Deaths && winningTeam.TeamId == 200) trueCasesAdc += 1;

                    if (teamA.Max(p => p.Stats.Deaths) == jglA.Stats.Deaths &&
                        teamB.Max(p => p.Stats.Deaths) == jglB.Stats.Deaths) removedCasesJgl += 1;
                    else if (teamA.Max(p => p.Stats.Deaths) == jglA.Stats.Deaths && winningTeam.TeamId == 100 ||
                        teamB.Max(p => p.Stats.Deaths) == jglB.Stats.Deaths && winningTeam.TeamId == 200) trueCasesJgl += 1;

                    if (teamA.Max(p => p.Stats.Deaths) == midA.Stats.Deaths &&
                        teamB.Max(p => p.Stats.Deaths) == midB.Stats.Deaths) removedCasesMid += 1;
                    else if (teamA.Max(p => p.Stats.Deaths) == midA.Stats.Deaths && winningTeam.TeamId == 100 ||
                        teamB.Max(p => p.Stats.Deaths) == midB.Stats.Deaths && winningTeam.TeamId == 200) trueCasesMid += 1;

                    if (teamA.Max(p => p.Stats.Deaths) == topA.Stats.Deaths &&
                        teamB.Max(p => p.Stats.Deaths) == topB.Stats.Deaths) removedCasesTop += 1;
                    else if (teamA.Max(p => p.Stats.Deaths) == topA.Stats.Deaths && winningTeam.TeamId == 100 ||
                        teamB.Max(p => p.Stats.Deaths) == topB.Stats.Deaths && winningTeam.TeamId == 200) trueCasesTop += 1;
                }
                else
                {
                    removedCasesAdc += 1;
                    removedCasesJgl += 1;
                    removedCasesMid += 1;
                    removedCasesSupport += 1;
                    removedCasesTop += 1;
                }
            }

            MessageBox.Show("Når rolle dør mest på hold vinder de:\n" +
                "Support: " + Math.Round((double)trueCasesSupport / (matchData.Count - removedCasesSupport) * 100, 2) + "% af spillene" +
                "(" + trueCasesSupport + "/" + (matchData.Count - removedCasesSupport) + ")\n" +
                "ADC: " + Math.Round((double)trueCasesAdc / (matchData.Count - removedCasesAdc) * 100, 2) + "% af spillene" +
                "(" + trueCasesAdc + "/" + (matchData.Count - removedCasesAdc) + ")\n" +
                "Mid: " + Math.Round((double)trueCasesMid / (matchData.Count - removedCasesMid) * 100, 2) + "% af spillene" +
                "(" + trueCasesMid + "/" + (matchData.Count - removedCasesMid) + ")\n" +
                "Top: " + Math.Round((double)trueCasesTop / (matchData.Count - removedCasesTop) * 100, 2) + "% af spillene" +
                "(" + trueCasesTop + "/" + (matchData.Count - removedCasesTop) + ")\n" +
                "Jgl: " + Math.Round((double)trueCasesJgl / (matchData.Count - removedCasesJgl) * 100, 2) + "% af spillene" +
                "(" + trueCasesJgl + "/" + (matchData.Count - removedCasesJgl) + ")\n");
        }

        private void minionsPrMinTopButton_Click(object sender, EventArgs e)
        {
            const int x = 15;
            int removedCases = 0;
            int trueCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);
                var winningTeam = match.Teams.Find(t => t.Winner == true);
                var topA = match.Participants.Find(p => p.TeamId == 100 && p.Timeline.Lane == "TOP");
                var topB = match.Participants.Find(p => p.TeamId == 200 && p.Timeline.Lane == "TOP");

                if (topA != null && topB != null)
                {
                    double csTopTeamA = topA.Timeline.CreepsPerMinDeltas.ZeroToTen + topA.Timeline.CreepsPerMinDeltas.TenToTwenty;
                    double csTopTeamB = topB.Timeline.CreepsPerMinDeltas.ZeroToTen + topB.Timeline.CreepsPerMinDeltas.TenToTwenty;

                    // hvis begge har over x minions pr min kigger vi ikke på det
                    if (csTopTeamA >= x && csTopTeamB >= x) removedCases += 1;
                    // hvis der kun er 1
                    else if (csTopTeamA >= x || csTopTeamB >= x) {
                        if (csTopTeamA >= x && winningTeam.TeamId == 100 ||
                            csTopTeamB >= x && winningTeam.TeamId == 200) trueCases += 1;
                    }
                    // hvis der er ingen
                    else removedCases += 1;
                }
                else removedCases += 1;
            }
            MessageBox.Show("Når top har flere end " + (double)x / 2 + " cs vinder de " + (double)trueCases / (matchData.Count - removedCases) * 100 + "% af spillene");

        }

        private void moreMinionsTopButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);

                var topA = match.Participants.Find(p => p.Timeline.Lane == "TOP" && p.TeamId == 100);
                var winningTeam = match.Teams.Find(t => t.Winner);

                if (topA != null && topA.Timeline.CsDiffPerMinDeltas != null)
                {
                    var csDiffTopA = topA.Timeline.CsDiffPerMinDeltas.ZeroToTen + topA.Timeline.CsDiffPerMinDeltas.TenToTwenty;
                    if (csDiffTopA > 0 && winningTeam.TeamId == 100 ||
                        csDiffTopA < 0 && winningTeam.TeamId == 200) trueCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når top har flere cs end modstander top\n" +
                "vinder holdet " + Math.Round((double)trueCases / (matchData.Count - removedCases) * 100, 2)
                + "% af spillene");
        }

        private void wardsTopButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);

                var winningTeam = match.Teams.Find(t => t.Winner);
                var topA = match.Participants.Find(p => p.Timeline.Lane == "TOP" && p.TeamId == 100);
                var topB = match.Participants.Find(p => p.Timeline.Lane == "TOP" && p.TeamId == 200);

                if (topA != null && topB != null)
                {
                    long wardsScoreTeamA = topA.Stats.WardsKilled + topA.Stats.WardsPlaced;
                    long wardsScoreTeamB = topB.Stats.WardsKilled + topB.Stats.WardsPlaced;

                    if (wardsScoreTeamA > wardsScoreTeamB && winningTeam.TeamId == 100 ||
                        wardsScoreTeamB > wardsScoreTeamA && winningTeam.TeamId == 200) trueCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Holdet med top der dræber og placerer flest wards\n" +
                "vinder i gennemsnit " + Math.Round((double)trueCases / matchData.Count * 100, 2)
                + "% af spillene");
        }

        private void dmgDealtToChampionsTopButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                Match match = JsonConvert.DeserializeObject<Match>(val);

                Participant topA = match.Participants.Find(p => p.TeamId == 100 && p.Timeline.Lane == "TOP");
                Participant topB = match.Participants.Find(p => p.TeamId == 200 && p.Timeline.Lane == "TOP");
                Team winningTeam = match.Teams.Find(t => t.Winner == true);

                if (topA != null && topB != null)
                {
                    if ((topA.Stats.TotalDamageDealtToChampions > topB.Stats.TotalDamageDealtToChampions &&
                         winningTeam.TeamId == 100) ||
                        (topB.Stats.TotalDamageDealtToChampions > topA.Stats.TotalDamageDealtToChampions &&
                        winningTeam.TeamId == 200)) trueCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når top har givet mere skade til champs end modstander top\n" +
                "vinder holdet " + Math.Round((double)trueCases / (matchData.Count - removedCases) * 100, 2)
                + "% af spillene");
        }

        private void assistsTopButton_Click(object sender, EventArgs e)
        {
            int trueCasesSupport = 0;
            int removedCasesSupport = 0;
            int trueCasesAdc = 0;
            int removedCasesAdc = 0;
            int trueCasesMid = 0;
            int removedCasesMid = 0;
            int trueCasesTop = 0;
            int removedCasesTop = 0;
            int trueCasesJgl = 0;
            int removedCasesJgl = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);

                var winningTeam = match.Teams.Find(t => t.Winner == true);
                var suppA = match.Participants.Find(p => p.Timeline.Role == "DUO_SUPPORT" && p.TeamId == 100);
                var suppB = match.Participants.Find(p => p.Timeline.Role == "DUO_SUPPORT" && p.TeamId == 200);
                var adcA = match.Participants.Find(p => p.Timeline.Role == "DUO_CARRY" && p.TeamId == 100);
                var adcB = match.Participants.Find(p => p.Timeline.Role == "DUO_CARRY" && p.TeamId == 200);
                var topA = match.Participants.Find(p => p.Timeline.Lane == "TOP" && p.TeamId == 100);
                var topB = match.Participants.Find(p => p.Timeline.Lane == "TOP" && p.TeamId == 200);
                var midA = match.Participants.Find(p => p.Timeline.Lane == "MIDDLE" && p.TeamId == 100);
                var midB = match.Participants.Find(p => p.Timeline.Lane == "MIDDLE" && p.TeamId == 200);
                var jglA = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 100);
                var jglB = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 200);

                if (suppA != null && suppB != null && adcA != null && adcB != null &&
                    topA != null && topB != null && jglA != null && jglB != null &&
                    midA != null && midB != null)
                {
                    List<Participant> teamA = match.Participants.FindAll(p => p.TeamId == 100);
                    List<Participant> teamB = match.Participants.FindAll(p => p.TeamId == 200);

                    if (teamA.Min(p => p.Stats.Assists) == suppA.Stats.Assists &&
                        teamB.Min(p => p.Stats.Assists) == suppB.Stats.Assists) removedCasesSupport += 1;
                    else if (teamA.Min(p => p.Stats.Assists) == suppA.Stats.Assists && winningTeam.TeamId == 100 ||
                        teamB.Min(p => p.Stats.Assists) == suppB.Stats.Assists && winningTeam.TeamId == 200) trueCasesSupport += 1;

                    if (teamA.Min(p => p.Stats.Assists) == adcA.Stats.Assists &&
                        teamB.Min(p => p.Stats.Assists) == adcB.Stats.Assists) removedCasesAdc += 1;
                    else if (teamA.Min(p => p.Stats.Assists) == adcA.Stats.Assists && winningTeam.TeamId == 100 ||
                        teamB.Min(p => p.Stats.Assists) == adcB.Stats.Assists && winningTeam.TeamId == 200) trueCasesAdc += 1;

                    if (teamA.Min(p => p.Stats.Assists) == jglA.Stats.Assists &&
                        teamB.Min(p => p.Stats.Assists) == jglB.Stats.Assists) removedCasesJgl += 1;
                    else if (teamA.Min(p => p.Stats.Assists) == jglA.Stats.Assists && winningTeam.TeamId == 100 ||
                        teamB.Min(p => p.Stats.Assists) == jglB.Stats.Assists && winningTeam.TeamId == 200) trueCasesJgl += 1;

                    if (teamA.Min(p => p.Stats.Assists) == midA.Stats.Assists &&
                        teamB.Min(p => p.Stats.Assists) == midB.Stats.Assists) removedCasesMid += 1;
                    else if (teamA.Min(p => p.Stats.Assists) == midA.Stats.Assists && winningTeam.TeamId == 100 ||
                        teamB.Min(p => p.Stats.Assists) == midB.Stats.Assists && winningTeam.TeamId == 200) trueCasesMid += 1;

                    if (teamA.Min(p => p.Stats.Assists) == topA.Stats.Assists &&
                        teamB.Min(p => p.Stats.Assists) == topB.Stats.Assists) removedCasesTop += 1;
                    else if (teamA.Min(p => p.Stats.Assists) == topA.Stats.Assists && winningTeam.TeamId == 100 ||
                        teamB.Min(p => p.Stats.Assists) == topB.Stats.Assists && winningTeam.TeamId == 200) trueCasesTop += 1;

                }
                else
                {
                    removedCasesAdc += 1;
                    removedCasesJgl += 1;
                    removedCasesMid += 1;
                    removedCasesSupport += 1;
                    removedCasesTop += 1;
                }
            }
            
            MessageBox.Show("Når rolle har færrest assists på hold vinder de:\n" +
                "Support: " + Math.Round((double)trueCasesSupport / (matchData.Count - removedCasesSupport) * 100, 2) + "% af spillene" +
                "(" + trueCasesSupport + "/" + (matchData.Count - removedCasesSupport) + ")\n" +
                "ADC: " + Math.Round((double)trueCasesAdc / (matchData.Count - removedCasesAdc) * 100, 2) + "% af spillene" +
                "(" + trueCasesAdc + "/" + (matchData.Count - removedCasesAdc) + ")\n" +
                "Mid: " + Math.Round((double)trueCasesMid / (matchData.Count - removedCasesMid) * 100, 2) + "% af spillene" +
                "(" + trueCasesMid + "/" + (matchData.Count - removedCasesMid) + ")\n" +
                "Top: " + Math.Round((double)trueCasesTop / (matchData.Count - removedCasesTop) * 100, 2) + "% af spillene" +
                "(" + trueCasesTop + "/" + (matchData.Count - removedCasesTop) + ")\n" +
                "Jgl: " + Math.Round((double)trueCasesJgl / (matchData.Count - removedCasesJgl) * 100, 2) + "% af spillene" +
                "(" + trueCasesJgl + "/" + (matchData.Count - removedCasesJgl) + ")\n");
        }

        private void minionsPrMinMidButton_Click(object sender, EventArgs e)
        {
            const int x = 15;
            int removedCases = 0;
            int trueCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);
                var winningTeam = match.Teams.Find(t => t.Winner == true);
                var midA = match.Participants.Find(p => p.TeamId == 100 && p.Timeline.Lane == "MIDDLE");
                var midB = match.Participants.Find(p => p.TeamId == 200 && p.Timeline.Lane == "MIDDLE");

                if (midA != null && midB != null)
                {
                    double csMidTeamA = midA.Timeline.CreepsPerMinDeltas.ZeroToTen + midA.Timeline.CreepsPerMinDeltas.TenToTwenty;
                    double csMidTeamB = midB.Timeline.CreepsPerMinDeltas.ZeroToTen + midB.Timeline.CreepsPerMinDeltas.TenToTwenty;

                    // hvis begge har over x minions pr min kigger vi ikke på det
                    if (csMidTeamA >= x && csMidTeamB >= x) removedCases += 1;
                    // hvis der kun er 1
                    else if (csMidTeamA >= x || csMidTeamB >= x)
                    {
                        if (csMidTeamA >= x && winningTeam.TeamId == 100 ||
                            csMidTeamB >= x && winningTeam.TeamId == 200) trueCases += 1;
                    }
                    // hvis der er ingen
                    else removedCases += 1;
                }
                else removedCases += 1;
            }
            MessageBox.Show("Når mid har flere end " + (double)x / 2 + 
                " cs vinder de " + 
                (double)trueCases / (matchData.Count - removedCases) * 100 +
                "% af spillene");
        }

        private void moreMinionsMidButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);
                var winningTeam = match.Teams.Find(t => t.Winner == true);
                var midA = match.Participants.Find(p => p.TeamId == 100 && p.Timeline.Lane == "MIDDLE");

                if (midA != null && midA.Timeline.CsDiffPerMinDeltas != null)
                {
                    var csDiff = midA.Timeline.CsDiffPerMinDeltas.ZeroToTen +
                        midA.Timeline.CsDiffPerMinDeltas.TenToTwenty;

                    if (csDiff > 0 && winningTeam.TeamId == 100 ||
                        csDiff < 0 && winningTeam.TeamId == 200) trueCases += 1; 
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når mid har flere cs end modstander adc\nvinder holdet " 
                + (double)trueCases / (matchData.Count - removedCases) * 100 +
                "% af spillene");
        }

        private void dmgToChampsMidButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                Match match = JsonConvert.DeserializeObject<Match>(val);

                Participant midA = match.Participants.Find(p => p.TeamId == 100 && p.Timeline.Lane == "MIDDLE");
                Participant midB = match.Participants.Find(p => p.TeamId == 200 && p.Timeline.Lane == "MIDDLE");
                Team winningTeam = match.Teams.Find(t => t.Winner == true);

                if (midA != null && midB != null)
                {
                    if ((midA.Stats.TotalDamageDealtToChampions > midB.Stats.TotalDamageDealtToChampions &&
                        midA.TeamId == winningTeam.TeamId) ||
                        (midB.Stats.TotalDamageDealtToChampions > midA.Stats.TotalDamageDealtToChampions &&
                        midB.TeamId == winningTeam.TeamId)) trueCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når mid har givet mere skade til champs end modstander adc\nvinder holdet " +
                (double)trueCases / (matchData.Count - removedCases) * 100 + "% af spillene");
        }

        private void wardsMidButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);

                var winningTeam = match.Teams.Find(t => t.Winner);
                var midA = match.Participants.Find(p => p.Timeline.Lane == "MIDDLE" && p.TeamId == 100);
                var midB = match.Participants.Find(p => p.Timeline.Lane == "MIDDLE" && p.TeamId == 200);

                if (midA != null && midB != null)
                {
                    long wardsScoreTeamA = midA.Stats.WardsKilled + midA.Stats.WardsPlaced;
                    long wardsScoreTeamB = midB.Stats.WardsKilled + midB.Stats.WardsPlaced;

                    if (wardsScoreTeamA > wardsScoreTeamB && winningTeam.TeamId == 100 ||
                        wardsScoreTeamB > wardsScoreTeamA && winningTeam.TeamId == 200) trueCases += 1;
                    else if (wardsScoreTeamA == wardsScoreTeamB) removedCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Holdet med mid der dræber og placerer flest wards\n" +
                "vinder i gennemsnit " + Math.Round((double)trueCases / matchData.Count * 100, 2)
                + "% af spillene");
        }

        private void enemyJungleMidButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);

                var midA = match.Participants.Find(p => p.Timeline.Lane == "MIDDLE" && p.TeamId == 100);
                var midB = match.Participants.Find(p => p.Timeline.Lane == "MIDDLE" && p.TeamId == 200);
                var winningTeam = match.Teams.Find(t => t.Winner);

                if (midA != null && midB != null)
                {
                    if (midA.Stats.NeutralMinionsKilledEnemyJungle > midB.Stats.NeutralMinionsKilledEnemyJungle &&
                        winningTeam.TeamId == 100 ||
                        midB.Stats.NeutralMinionsKilledEnemyJungle > midA.Stats.NeutralMinionsKilledEnemyJungle &&
                        winningTeam.TeamId == 200) trueCases += 1;
                    else if (midA.Stats.NeutralMinionsKilledEnemyJungle == midB.Stats.NeutralMinionsKilledEnemyJungle) removedCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når mid har dræbt flere minions i enemy jungle vinder de\n" +
                (double)trueCases / (matchData.Count - removedCases) * 100 +
                "% af spillene");
        }

        private void monstersKilledJglButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);

                var jglA = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 100);
                var jglB = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 200);
                var winningTeam = match.Teams.Find(t => t.Winner);

                if (jglA != null && jglB != null)
                {
                    if (jglA.Stats.NeutralMinionsKilled > jglB.Stats.NeutralMinionsKilled &&
                        winningTeam.TeamId == 100 ||
                        jglB.Stats.NeutralMinionsKilled > jglA.Stats.NeutralMinionsKilled &&
                        winningTeam.TeamId == 200) trueCases += 1;
                    else if (jglA.Stats.NeutralMinionsKilled == jglB.Stats.NeutralMinionsKilled) removedCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når jgl har dræbt flere minions i jgl end modstander jgl vinder de\n" +
                (double)trueCases / (matchData.Count - removedCases) * 100 +
                "% af spillene");
        }

        private void enemyJungleJglButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);

                var jglA = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 100);
                var jglB = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 200);
                var winningTeam = match.Teams.Find(t => t.Winner);

                if (jglA != null && jglB != null)
                {
                    if (jglA.Stats.NeutralMinionsKilledEnemyJungle > jglB.Stats.NeutralMinionsKilledEnemyJungle &&
                        winningTeam.TeamId == 100 ||
                        jglB.Stats.NeutralMinionsKilledEnemyJungle > jglA.Stats.NeutralMinionsKilledEnemyJungle &&
                        winningTeam.TeamId == 200) trueCases += 1;
                    else if (jglA.Stats.NeutralMinionsKilledEnemyJungle == jglB.Stats.NeutralMinionsKilledEnemyJungle) removedCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Når jgl har dræbt flere minions i enemy jungle vinder de\n" +
                (double)trueCases / (matchData.Count - removedCases) * 100 +
                "% af spillene");
        }

        private void wardsJglButton_Click(object sender, EventArgs e)
        {
            int trueCases = 0;
            int removedCases = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);

                var winningTeam = match.Teams.Find(t => t.Winner);
                var jglA = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 100);
                var jglB = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 200);

                if (jglA != null && jglB != null)
                {
                    long wardsScoreTeamA = jglA.Stats.WardsKilled + jglA.Stats.WardsPlaced;
                    long wardsScoreTeamB = jglB.Stats.WardsKilled + jglB.Stats.WardsPlaced;

                    if (wardsScoreTeamA > wardsScoreTeamB && winningTeam.TeamId == 100 ||
                        wardsScoreTeamB > wardsScoreTeamA && winningTeam.TeamId == 200) trueCases += 1;
                    else if (wardsScoreTeamA == wardsScoreTeamB) removedCases += 1;
                }
                else removedCases += 1;
            }

            MessageBox.Show("Holdet med jgl der dræber og placerer flest wards\n" +
                "vinder i gennemsnit " + Math.Round((double)trueCases / matchData.Count * 100, 2)
                + "% af spillene");
        }

        private void kdaJglButton_Click(object sender, EventArgs e)
        {
            int trueCasesSupport = 0;
            int removedCasesSupport = 0;
            int trueCasesAdc = 0;
            int removedCasesAdc = 0;
            int trueCasesMid = 0;
            int removedCasesMid = 0;
            int trueCasesTop = 0;
            int removedCasesTop = 0;
            int trueCasesJgl = 0;
            int removedCasesJgl = 0;

            foreach (var val in matchData.Values)
            {
                var match = JsonConvert.DeserializeObject<Match>(val);

                var winningTeam = match.Teams.Find(t => t.Winner == true);
                var suppA = match.Participants.Find(p => p.Timeline.Role == "DUO_SUPPORT" && p.TeamId == 100);
                var suppB = match.Participants.Find(p => p.Timeline.Role == "DUO_SUPPORT" && p.TeamId == 200);
                var adcA = match.Participants.Find(p => p.Timeline.Role == "DUO_CARRY" && p.TeamId == 100);
                var adcB = match.Participants.Find(p => p.Timeline.Role == "DUO_CARRY" && p.TeamId == 200);
                var topA = match.Participants.Find(p => p.Timeline.Lane == "TOP" && p.TeamId == 100);
                var topB = match.Participants.Find(p => p.Timeline.Lane == "TOP" && p.TeamId == 200);
                var midA = match.Participants.Find(p => p.Timeline.Lane == "MIDDLE" && p.TeamId == 100);
                var midB = match.Participants.Find(p => p.Timeline.Lane == "MIDDLE" && p.TeamId == 200);
                var jglA = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 100);
                var jglB = match.Participants.Find(p => p.Timeline.Lane == "JUNGLE" && p.TeamId == 200);

                if (suppA != null && suppB != null && adcA != null && adcB != null &&
                    topA != null && topB != null && midA != null && midB != null &&
                    jglA != null && jglB != null)
                {
                    double kdaSuppA = CalculateKda(suppA);
                    double kdaSuppB = CalculateKda(suppB);
                    double kdaAdcA = CalculateKda(adcA);
                    double kdaAdcB = CalculateKda(adcB);
                    double kdaMidA = CalculateKda(midA);
                    double kdaMidB = CalculateKda(midB);
                    double kdaTopA = CalculateKda(topA);
                    double kdaTopB = CalculateKda(topB);
                    double kdaJglA = CalculateKda(jglA);
                    double kdaJglB = CalculateKda(jglB);

                    if (kdaSuppA > kdaSuppB && winningTeam.TeamId == 100 ||
                        kdaSuppB > kdaSuppA && winningTeam.TeamId == 200) trueCasesSupport += 1;
                    else if (kdaSuppA == kdaSuppB) removedCasesSupport += 1;

                    if (kdaAdcA > kdaAdcB && winningTeam.TeamId == 100 ||
                        kdaAdcB > kdaAdcA && winningTeam.TeamId == 200) trueCasesAdc += 1;
                    else if (kdaAdcA == kdaAdcB) removedCasesAdc += 1;

                    if (kdaMidA > kdaMidB && winningTeam.TeamId == 100 ||
                        kdaMidB > kdaMidA && winningTeam.TeamId == 200) trueCasesMid += 1;
                    else if (kdaMidA == kdaMidB) removedCasesMid += 1;

                    if (kdaTopA > kdaTopB && winningTeam.TeamId == 100 ||
                        kdaTopB > kdaTopA && winningTeam.TeamId == 200) trueCasesTop += 1;
                    else if (kdaTopA == kdaTopB) removedCasesTop += 1;

                    if (kdaJglA > kdaJglB && winningTeam.TeamId == 100 ||
                        kdaJglB > kdaJglA && winningTeam.TeamId == 200) trueCasesJgl += 1;
                    else if (kdaJglA == kdaJglB) removedCasesJgl += 1;
                }
                else
                {
                    removedCasesAdc += 1;
                    removedCasesJgl += 1;
                    removedCasesMid += 1;
                    removedCasesSupport += 1;
                    removedCasesTop += 1;
                }
            }

            MessageBox.Show("Når rolle har bedre kda end modstander rolle vinder de:\n" +
                "Support: " + Math.Round((double)trueCasesSupport / (matchData.Count - removedCasesSupport) * 100, 2) + "% af spillene" +
                "(" + trueCasesSupport + "/" + (matchData.Count - removedCasesSupport) + ")\n" +
                "ADC: " + Math.Round((double)trueCasesAdc / (matchData.Count - removedCasesAdc) * 100, 2) + "% af spillene" +
                "(" + trueCasesAdc + "/" + (matchData.Count - removedCasesAdc) + ")\n" +
                "Mid: " + Math.Round((double)trueCasesMid / (matchData.Count - removedCasesMid) * 100, 2) + "% af spillene" +
                "(" + trueCasesMid + "/" + (matchData.Count - removedCasesMid) + ")\n" +
                "Top: " + Math.Round((double)trueCasesTop / (matchData.Count - removedCasesTop) * 100, 2) + "% af spillene" +
                "(" + trueCasesTop + "/" + (matchData.Count - removedCasesTop) + ")\n" +
                "Jgl: " + Math.Round((double)trueCasesJgl / (matchData.Count - removedCasesJgl) * 100, 2) + "% af spillene" +
                "(" + trueCasesJgl + "/" + (matchData.Count - removedCasesJgl) + ")\n");
        }

        private double CalculateKda(Participant p)
        {
            return p.Stats.Deaths != 0 ? 
                (double)(p.Stats.Assists + p.Stats.Kills) / p.Stats.Deaths : 
                (double)p.Stats.Kills + p.Stats.Assists; 
        }
    }
}