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
        
        private double[][] createIdeal()
        {
            double[][] ideal = new double[teams.Count][];
            for (int i = 0; i < teams.Count; i++)
            {
                ideal[i] = new double[] { convertBool(teams[i].Winner) };
            }
            return ideal;
        }

        private double[][] createInput()
        {
            List<List<double>> input = new List<List<double>>();
            for (int teamCounter = 0; teamCounter < teams.Count; teamCounter++)
            {
                List<double> participantsData = new List<double>();
                Support supp = (Support)teams[teamCounter].Participants.Find(p => p.Role is Support).Role;
                foreach (Participant p in teams[teamCounter].Participants)
                {
                    participantsData = participantsData.Concat(p.Role.GetData()).ToList();
                }
                if (participantsData.Count == 38)
                    input.Add(participantsData);
            }

            return input.Select(listElement => listElement.ToArray()).ToArray();
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

            double[][] testInput = createInput();
            double[][] testIdeal = createIdeal();

            IMLDataSet trainingSet = new BasicMLDataSet(testInput, testIdeal);

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
