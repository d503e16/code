using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rankingsystem.Classes;
using Newtonsoft.Json;
using System.Net.Http;
using Rankingsystem.Classes.Roles;
using Rankingsystem.Classes.NeuralNetwork;

namespace Rankingsystem
{

    class Program
    {
        static void Main(string[] args)
        {
            //Database database = new Database();
            //database.InitDatabase();
            //HttpClient client = new HttpClient();
            //var response = client.GetStringAsync("https://euw.api.pvp.net/api/lol/euw/v2.2/match/2776285553?api_key=RGAPI-3b849016-e4b4-4e6e-bf2d-c5e74e2368a5");
            //MatchAPI match = JsonConvert.DeserializeObject<MatchAPI>(response.Result);
            //List<Participant> a = new List<Participant>();
            //for (int i = 0; i < 10; i++)
            //{
            //    a.Add(match.getparticipant(i));
            //}

            //var summoner = database.GetSummoner(1);
            //Console.WriteLine("test");
            Rank r = new Rank();
            r.execute();
            Console.ReadKey();
        }
    }
}
