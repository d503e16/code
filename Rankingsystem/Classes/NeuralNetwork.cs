using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.App.Analyst;
using Encog.App.Analyst.CSV.Normalize;
using Encog.App.Analyst.Script.Normalize;
using Encog.App.Analyst.Wizard;
using Encog.Util.CSV;
using Encog.Util.Simple;
using Rankingsystem.Classes.Roles;


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
        //private string outputFile = Directory.GetParent(
        //        Directory.GetParent(
        //            Directory.GetParent(
        //            Environment.CurrentDirectory).ToString()
        //        ).ToString()
        //    ) + "\\target.csv";

        public NeuralNetwork(List<Match> listMatch)
        {
            foreach (Match m in listMatch)
            {
                teams.Add(m.Team1);
                teams.Add(m.Team2);
            }
        }

        private double[][] createIdeal()
        {
            List<List<double>> idealtest = new List<List<double>>();
            for (int teamCounter = 0; teamCounter < teams.Count; teamCounter++)
            {
                List<double> participantsData = new List<double>();
                List<string> tempData = new List<string>();
                foreach (Participant p in teams[teamCounter].Participants)
                {
                    tempData = tempData.Concat(p.Role.GetData()).ToList();
                }
                participantsData = tempData.ConvertAll(data => double.Parse(data));

                if (participantsData.Count == 38)
                    idealtest.Add(convertBool(teams[teamCounter].Winner));
            }
            return idealtest.Select(listElement => listElement.ToArray()).ToArray();
        }

        private List<string> createParticipantData(int teamCounter)
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

        private string[][] createInput()
        {
            List<List<string>> input = new List<List<string>>();
            for (int teamCounter = 0; teamCounter < teams.Count; teamCounter++)
            {
                List<string> participantsData = new List<string>();
                 participantsData = createParticipantData(teamCounter);

                if (participantsData.Count == 38)
                    input.Add(participantsData);
            }
            return input.Select(listElement => listElement.ToArray()).ToArray();
        }
        
        private List<double> convertBool(bool b)
        {
            List<double> list = new List<double>();
            if (b == true)
            {
                list.Add(1.0);
                return list;
            }
            else
            { 
                list.Add(0.0); return list;
            }
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

        //private void createFiles()
        //{
        //    SaveArrayAsCSV(createInput(), inputFile);
        //    var sourceFile = new FileInfo(inputFile);
        //    var targetFile = new FileInfo(outputFile);
        //    var analyst = new EncogAnalyst();
        //    var wizard = new AnalystWizard(analyst);

        //    wizard.Wizard(sourceFile, false, AnalystFileFormat.DecpntComma);

        //    var norm = new AnalystNormalizeCSV();
        //    norm.Analyze(sourceFile, false, CSVFormat.English, analyst);
        //    norm.ProduceOutputHeaders = false;
        //    norm.Normalize(targetFile);
        //}

        //private string[][] getNormalized()
        //{
        //    string[][] array = File.ReadLines(outputFile).Select(line => line.Split(',')).ToArray();

        //    //List<List<double>> norm = new List<List<double>>();
        //    //for (int i = 0; i < teams.Count; i++)
        //    //{

        //    //}
        //    return array;
        //}

        //private void serializeToExcel()
        //{
        //    XmlSerializer s = new XmlSerializer(typeof(string[][]));
        //    using (StreamWriter sw = new StreamWriter(inputFile))
        //    {
        //        s.Serialize(sw, createInput());
        //    }
        //}
        public void execute()
        {
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(new ActivationLinear(), true, 38));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 1));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 1));
            network.Structure.FinalizeStructure();
            network.Reset();

            var ideal = createIdeal();
            var input = createInput();

            SaveArrayAsCSV(input, inputFile);


            //createFiles();

            //var trainingset = EncogUtility.LoadCSV2Memory(outputFile, network.InputCount, network.OutputCount, true, CSVFormat.English, false);

            //IMLDataSet trainingSet = new BasicMLDataSet(, ideal);

            //IMLTrain train = new Backpropagation(network, trainingSet);
            //int epoch = 1;

            //do
            //{
            //    train.Iteration();
            //    Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
            //    epoch++;
            //} while (train.Error > 0.17);

            //Console.WriteLine(@"Neural Network Results:");
            //foreach (IMLDataPair pair in trainingSet)
            //{
            //    IMLData output = network.Compute(pair.Input);
            //    Console.WriteLine(@" actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
            //}
            Console.ReadKey();
        }
    }
}
