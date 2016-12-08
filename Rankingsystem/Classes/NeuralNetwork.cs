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

namespace Rankingsystem.Classes
{
    public class NeuralNetwork
    {
        private List<Team> teams = new List<Team>();
        private string inputFile = Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(
                    Environment.CurrentDirectory).ToString()
                ).ToString()
            ) + "\\input.csv";
        private string normFile = Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(
                    Environment.CurrentDirectory).ToString()
                ).ToString()
            ) + "\\NormalizedInput.csv";
        private string networkFile = Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(
                    Environment.CurrentDirectory).ToString()
                ).ToString()
            ) + "\\Network.ser";

        public NeuralNetwork(List<Match> listMatch)
        {
            foreach (Match m in listMatch)
            {
                teams.Add(m.Team1);
                teams.Add(m.Team2);
            }
        }

        private List<string> createParticipantData(int teamCounter)
        {
            List<string> participantsData = new List<string>();
            foreach (Participant p in teams[teamCounter].Participants)
            {
                participantsData = participantsData.Concat(p.Role.GetData()).ToList();
            }
            return participantsData;
        }

        private string[][] createInputAndIdeal()
        {
            List<List<string>> input = new List<List<string>>();
            for (int teamCounter = 0; teamCounter < teams.Count; teamCounter++)
            {
                List<string> participantsData = new List<string>();
                 participantsData = createParticipantData(teamCounter);
                if (participantsData.Count == 38)
                    participantsData.Add(convertBool(teams[teamCounter].Winner).ToString());
                    input.Add(participantsData);
            }
            return input.Select(listElement => listElement.ToArray()).ToArray();
        }
        
        private double convertBool(bool b)
        {
            if (b == true) return 1.0;
            else return 0.0;
        }

        public static void SaveArrayAsCSV<T>(T[][] jaggedArrayToSave, string fileName)
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
            network.AddLayer(new BasicLayer(null, true, 38));
            network.AddLayer(new BasicLayer(new ActivationTANH(), true, 1));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));
            network.Structure.FinalizeStructure();
            network.Reset();
            return network;
        }

        public void Train()
        {
            var network = createNetwork();
            //SaveArrayAsCSV(createInputAndIdeal(), inputFile);
            var trainingSet = EncogUtility.LoadCSV2Memory(normFile, network.InputCount, 1, false, CSVFormat.English, false);
            
            IMLTrain train = new Backpropagation(network, trainingSet);
            int epoch = 1;
            int truecases = 0;
            do
            {
                train.Iteration();
                Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
                epoch++;
            } while (train.Error > 0.05);

            Console.WriteLine(@"Neural Network Results:");
            foreach (IMLDataPair pair in trainingSet)
            {
                IMLData output = network.Compute(pair.Input);
                Console.WriteLine(@" actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
                if (pair.Ideal[0] == 1 && output[0] > 0.5)
                {
                    truecases++;
                }
                else if (pair.Ideal[0] == 0 && output[0] < 0.5)
                    truecases++;
            }
            Console.WriteLine(truecases);
            SerializeObject.Save(networkFile, network);
            Console.ReadKey();
        }

        public void Test(double[] input)
        {
            double[] output = new double[1];
            BasicNetwork network = (BasicNetwork)SerializeObject.Load(networkFile);
            network.Compute(input, output);
            Console.WriteLine(output[0]);
            Console.ReadKey();
        }
    }
}
