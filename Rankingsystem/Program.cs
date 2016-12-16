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
            //Database d = new Database("LolRank");
            //List<Match> matches = d.GetAllMatches("testMatchTable");
            //NeuralNetwork n = new NeuralNetwork(matches);
            //List<Team> teams = new List<Team>();
            //double[] filler = new double[33];
            //List<double[]> doubleArray = new List<double[]>();
            //string[] strInput = new string[teams.Count];
            //List<string[]> strArray = new List<string[]>();
            //List<string> input = new List<string>();

            //int truecases = 0;

            //foreach (Match m in matches)
            //{
            //    teams.Add(m.Team1);
            //    teams.Add(m.Team2);
            //}

            //for (int teamCounter = 0; teamCounter < teams.Count; teamCounter++)
            //{
            //    List<string> participantsData = new List<string>();
            //    participantsData = n.createParticipantData(teamCounter);
            //    if (participantsData.Count == 33)
            //    input = participantsData;
            //    strInput = input.Select(listElement => listElement).ToArray();
            //    filler = strInput.Select<string, double>(s => Double.Parse(s)).ToArray<double>();
            //    doubleArray.Add(filler);
            //    strArray.Add(strInput);
            //}

            //string[][] test = strArray.ToArray();
            //n.SaveArrayAsCSV(test, Directory.GetParent(
            //    Directory.GetParent(
            //        Directory.GetParent(
            //        Environment.CurrentDirectory).ToString()
            //    ).ToString()
            //) + "\\testInput.csv");

            //for (int i = 0; i < teams.Count; i++)
            //{
            //    double outPut = n.Test(doubleArray[i]);
            //    if (outPut < 0.5 && teams[i].Winner == false)
            //    {
            //        truecases++;
            //    }
            //    else if (outPut < 0.5 && teams[i].Winner == true)
            //    {
            //        truecases++;
            //    }
            //}
            //Console.WriteLine(truecases);

            ////double[] testInput = Array.ConvertAll(strInput[0].Split(','), new Converter<string, double>(Double.Parse));

            //NeuralNetwork n = new NeuralNetwork(d.GetAllMatches());
            //n.execute();
            Database d = new Database("LolRank");
            RankingSystem s = new RankingSystem(d);
            s.Start();
            //Console.ReadKey();
        }
    }
}
