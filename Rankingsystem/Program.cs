using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using Rankingsystem.Classes;

namespace Rankingsystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Database d = new Database("LolRank");
            List<Match> matches = d.GetAllMatches("matchTable");
            List<Match> testMatches = d.GetAllMatches("testMatchTable");
            NeuralNetwork n = new NeuralNetwork(matches, testMatches);
            n.TestAll();
            
            //RankingSystem s = new RankingSystem(d);
            //s.Start();
            Console.ReadKey();
        }
    }
}
