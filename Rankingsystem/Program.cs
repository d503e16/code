using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rankingsystem.Classes;
using Newtonsoft.Json;
using System.Net.Http;

namespace Rankingsystem
{

    class Program
    {
        static void Main(string[] args)
        {
            Database database = new Database();
            database.InitDatabase();
            var summoner = database.GetSummoner(1);
            Console.WriteLine("test");
            Console.ReadKey();
        }
    }
}
