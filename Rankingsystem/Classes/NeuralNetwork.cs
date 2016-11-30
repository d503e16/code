using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Rankingsystem.Classes.Roles;


namespace Rankingsystem.Classes
{
    public class NeuralNetwork
    {
        private List<Team> teams = new List<Team>();
        //private double[][] input = new double[][];

        public NeuralNetwork(List<Match> listMatch)
        {
            foreach (Match m in listMatch)
            {
                teams.Add(m.Team1);
                teams.Add(m.Team2);
            }
        }
        
        public double[][] CreateIdeal()
        {
            double[][] ideal = new double[teams.Count][];
            for (int i = 0; i < teams.Count ; i++)
            {
                ideal[i] = new double[] { convertBool(teams[i].Winner) };
            }
            return ideal;
        }

        public double[][] CreateInput()
        {
            double[][] input = new double[teams.Count][];
            for (int i = 0; i < teams.Count; i++)
            {
                for (int j = 0; j < teams[i].Participants.Count; j++)
                {
                    if (input[i] == null)
                    {
                        input[i] = teams[i].Participants[j].Role.GetData();
                    }
                    else
                    {
                        input[i] = input[i].Concat(teams[i].Participants[j].Role.GetData()).ToArray();
                    }
                }
            }
            return input;
        }
        
        private double convertBool(bool b)
        {
            if (b == true) return 1.0;
            else return 0.0;
        }

        public void execute()
        {
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, false, 38));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 1));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 1));
            network.Structure.FinalizeStructure();
            network.Reset();

            IMLDataSet trainingSet = new BasicMLDataSet(CreateInput(), CreateIdeal());

            IMLTrain train = new Backpropagation(network, trainingSet);

            int epoch = 1;

            do
            {
                train.Iteration();
                Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
                epoch++;
            } while (train.Error > 0.01);

            Console.WriteLine(@"Neural Network Results:");
            foreach (IMLDataPair pair in trainingSet)
            {
                IMLData output = network.Compute(pair.Input);
                Console.WriteLine(network.GetLayerOutput(1, 0)
                                  + @", actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
            }
            Console.ReadKey();
        }
    }
}
