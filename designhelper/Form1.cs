using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        string apiKey = "RGAPI-380f1d21-de69-4b2c-aebc-ab7774e2aa3b";
        HttpClient client = new HttpClient();
        int requestCounter = 0;
        int id = 22913206;

        public Form1()
        {
            InitializeComponent();
        }

        private async void getSummonerButton_Click(object sender, EventArgs e)
        {
            getSummonerText.Text = id.ToString();
            string input = getSummonerText.Text;
            if (input != "")
            {
                if (requestCounter == 10)
                {
                    System.Threading.Thread.Sleep(10000);
                    requestCounter = 0;
                }

                var response = await client.GetAsync(
                    "https://euw.api.pvp.net/api/lol/euw/v1.3/game/by-summoner/" + input + "/recent?api_key=" + apiKey);
                requestCounter += 1;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = (JObject)JsonConvert.DeserializeObject(json);
                    var games = data["games"];

                    List<long> gameIds = new List<long>();
                    List<long> playerIds = new List<long>();

                    for (int i = 0; i < games.Count(); i++)
                    {
                        var game = games[i];

                        var gameId = games[i]["gameId"].Value<long>();
                        gameIds.Add(gameId);

                        var fellowPlayers = game["fellowPlayers"];
                        for (int j = 0; j < fellowPlayers.Count(); j++)
                        {
                            var playerId = fellowPlayers[j]["summonerId"].Value<long>();
                            playerIds.Add(playerId);
                        }
                    }
                }
            }
        }
    }
}
