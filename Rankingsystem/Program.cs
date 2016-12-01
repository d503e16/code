using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rankingsystem.Classes;
using Newtonsoft.Json;
using System.Net.Http;
using Rankingsystem.Classes.Roles;
using System.Data.SQLite;

namespace Rankingsystem
{
    class Program
    {
        static void Main(string[] args)
        {
            //Database d = new Database();
            //NeuralNetwork n = new NeuralNetwork(d.GetAllMatches());
            //n.execute();
            RankingSystem s = new RankingSystem();
            s.Start();
        }
    }
}
