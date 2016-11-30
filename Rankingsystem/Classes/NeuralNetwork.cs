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
        private List<Participant> participantsTeam1;
        private List<Participant> participantsTeam2;
        private Team team1;
        public NeuralNetwork(MatchAPI m)
        {
            team1 = m.CreateMatch().Team1;
            participantsTeam1 = m.CreateMatch().Team1.Participants;
            participantsTeam2 = m.CreateMatch().Team2.Participants;
        }
        private double[][] input = new double[1][];
        private static double[][] ideal = new double[1][];

        public void CreateIdeal()
        {
            ideal[0] = new double[] { convertBool(team1.Winner) };
        }

        public void CreateInput()
        {
            //input[0] = participantsTeam1[0].Role.getData();
            int i;
            for (i = 0; i < participantsTeam1.Count; i++)
            {
                input[i] = input[0].Concat(participantsTeam1[i].Role.getData()).ToArray();
            }
            for (int j = 0;  j < participantsTeam2.Count; j++, i++)
            {
                input[i] = input[0].Concat(participantsTeam2[j].Role.getData()).ToArray();
            }
        }

        private double convertBool(bool b)
        {
            if (b == true) return 1.0;
            else return 0.0;
        }

        public void execute()
        {
            CreateIdeal();
            CreateInput();
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, false, 38));
            network.AddLayer(new BasicLayer(new ActivationBiPolar(), false, 10));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));
            network.Structure.FinalizeStructure();
            network.Reset();

            IMLDataSet trainingSet = new BasicMLDataSet(input, ideal);

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
                Console.WriteLine(network.GetLayerOutput(1, 2)
                                  + @", actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
            }
        }
    }
}
