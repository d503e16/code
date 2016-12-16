using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Util.CSV;
using Encog.Util.Simple;
using Encog.Util;
using Rankingsystem.Classes.Roles;

namespace Rankingsystem.Classes
{
    public class NeuralNetwork
    {
        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
        private List<Team> trainTeams = new List<Team>();
        private List<Team> testTeams = new List<Team>();
        private string inputFile = getFile("trainInput.csv");
        private string normFile = getFile("normalizedInput.csv");
        private string networkFile = getFile("Network.ser");
        private string normTestFile = getFile("normalizedTestInput.csv");

        private static string getFile(string file)
        {
            return Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(
                    Environment.CurrentDirectory).ToString()
                ).ToString()
            ) + "\\" + file;
        }

        public NeuralNetwork(List<Match> trainMatches, List<Match> testMatches)
        {
            foreach (Match m in trainMatches)
            {
                trainTeams.Add(m.Team1);
                trainTeams.Add(m.Team2);
            }

            foreach (Match m in testMatches)
            {
                testTeams.Add(m.Team1);
                testTeams.Add(m.Team2);
            }
        }

        private List<string> createParticipantData(int teamCounter, List<Team> teams)
        {
            List<string> participantsData = new List<string>();
            Top top = (Top)teams[teamCounter].Participants.Find(p => p.Role is Top).Role;
            Jungle jungle = (Jungle)teams[teamCounter].Participants.Find(p => p.Role is Jungle).Role;
            Mid mid = (Mid)teams[teamCounter].Participants.Find(p => p.Role is Mid).Role;
            Bot bot = (Bot)teams[teamCounter].Participants.Find(p => p.Role is Bot).Role;
            Support support = (Support)teams[teamCounter].Participants.Find(p => p.Role is Support).Role;
            
            participantsData = participantsData.Concat(top.GetData()).ToList();
            participantsData = participantsData.Concat(jungle.GetData()).ToList();
            participantsData = participantsData.Concat(mid.GetData()).ToList();
            participantsData = participantsData.Concat(bot.GetData()).ToList();
            participantsData = participantsData.Concat(support.GetData()).ToList();
            
            return participantsData;
        }


        private void createInputAndIdeal()
        {
            List<List<string>> input = new List<List<string>>();
            for (int teamCounter = 0; teamCounter < trainTeams.Count; teamCounter++)
            {
                List<string> participantsData = new List<string>();
                 participantsData = createParticipantData(teamCounter, trainTeams);
                if (participantsData.Count != 34)
                    participantsData.Add(convertBool(trainTeams[teamCounter].Winner).ToString());
                    input.Add(participantsData);
            }
            SaveArrayAsCSV(input.Select(listElement => listElement.ToArray()).ToArray(), inputFile);
        }

        private void createTestInput()
        {
            string[] strInput;
            List<string[]> result = new List<string[]>();  
            for (int teamCounter = 0; teamCounter < testTeams.Count; teamCounter++)
            {
                List<string> participantsData = new List<string>();
                participantsData = createParticipantData(teamCounter, testTeams);
                if (participantsData.Count == 33)
                {
                    strInput = participantsData.Select(listElement => listElement).ToArray();
                    result.Add(strInput);
                }
            }
            SaveArrayAsCSV(result.ToArray(), normTestFile);
        }

        private List<double[]> readTestInput()
        {
            string[] strInput = new string[testTeams.Count];
            double[] filler = new double[33];
            List<double[]> result = new List<double[]>();
            var data = File.ReadLines(normTestFile).Select(x => x.Split(',')).ToList();
            for (int k = 0; k < testTeams.Count; k++)
            {
                result.Add(new double[1]);
                strInput = data[k].Select(listElement => listElement).ToArray();
                filler = Array.ConvertAll(strInput, element => double.Parse(element, cultureInfo));
                result[k] = filler;
            }
            return result;
        }
        
        private double convertBool(bool b)
        {
            if (b == true) return 1.0;
            else return 0.0;
        }

        private void SaveArrayAsCSV<T>(T[][] jaggedArrayToSave, string fileName)
        {
            using (StreamWriter file = new StreamWriter(fileName))
            {
                foreach (T[] array in jaggedArrayToSave)
                {
                    foreach (T item in array)
                    {
                        file.Write(item + ",");
                    }
                    file.Write(Environment.NewLine);
                }
            }
        }

        private BasicNetwork createNetwork()
        {
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, 33));
            network.AddLayer(new BasicLayer(new ActivationTANH(), true, 1));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));
            network.Structure.FinalizeStructure();
            network.Reset();
            return network;
        }

        public void Train()
        {
            var network = createNetwork();
            IMLDataSet trainingSet = EncogUtility.LoadCSV2Memory(normFile, network.InputCount, 1, false, CSVFormat.English, false);
            
            IMLTrain train = new Backpropagation(network, trainingSet);
            int epoch = 1;
            int truecases = 0;
            do
            {
                train.Iteration();
                Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
                epoch++;
            } while (train.Error > 0.05);
            train.FinishTraining();

            Console.WriteLine(@"Neural Network Results:");
            foreach (IMLDataPair pair in trainingSet)
            {
                IMLData output = network.Compute(pair.Input);
                Console.WriteLine(@" actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
                if (pair.Ideal[0] == 1 && output[0] > 0.5)
                    truecases++;
                else if (pair.Ideal[0] == 0 && output[0] < 0.5)
                    truecases++;
            }
            Console.WriteLine(truecases);
            SerializeObject.Save(networkFile, network);
        }

        public double TestSingle(double[] input)
        {
            double[] outPut = new double[1];
            BasicNetwork network = (BasicNetwork)SerializeObject.Load(networkFile);
            network.Compute(input, outPut);
            //Console.Write("The chance of winning is: " + Math.Round(output[0]*100,2) + "% | ");
            return outPut[0];
        }

        public void TestAll()
        {
            List<double[]> testData = readTestInput();
            int trueCases = 0;
            for (int j = 0; j < testTeams.Count; j++)
            {
                double outPut = TestSingle(testData[j]);
                if (j % 2 == 0)
                {
                    int enemyIndex = j + 1;
                    if (outPut > TestSingle(testData[enemyIndex]) && testTeams[j].Winner == true)
                    {
                        trueCases++;
                        Console.Write("Team1: " + Math.Round(outPut*100,2) + "%" + " Team2: " + Math.Round(TestSingle(testData[enemyIndex]) * 100, 2) + "%" + " | ");
                        Console.WriteLine("Correct prediction " + j);
                    }
                    else if (outPut < TestSingle(testData[enemyIndex]) && testTeams[j].Winner == false)
                    {
                        trueCases++;
                        Console.Write("Team1: " + Math.Round(outPut * 100, 2) + "%" + " Team2: " + Math.Round(TestSingle(testData[enemyIndex]) * 100, 2) + "%" + " | ");
                        Console.WriteLine("Correct prediction " + j);
                    }
                    else
                    {
                        Console.Write("Team1: " + Math.Round(outPut * 100, 2) + "%" + " Team2: " + Math.Round(TestSingle(testData[enemyIndex]) * 100, 2) + "%" + " | ");
                        Console.WriteLine("Wrong prediction " + j);
                    }
                        //Console.WriteLine("Team1: " + Math.Round(outPut * 100, 2) + "% Team2: " + Math.Round(TestSingle(testData[enemyIndex]) * 100, 2) + "%   " + j);
                }
            }
            double trueProcent = ((double)trueCases) / (testTeams.Count/2) * 100;
            Console.WriteLine("The system guessed " + Math.Round(trueProcent,2) + "% correct of the matches");
        }
    }
}

