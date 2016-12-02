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
            List<List<double>> idealtest = new List<List<double>>();
            for (int teamCounter = 0; teamCounter < teams.Count; teamCounter++)
            {
                List<double> participantsData = new List<double>();
                foreach (Participant p in teams[teamCounter].Participants)
                {
                    participantsData = participantsData.Concat(p.Role.GetData()).ToList();
                }
                if (participantsData.Count == 38)
                    idealtest.Add(convertBool(teams[teamCounter].Winner));
            }
            return idealtest.Select(listElement => listElement.ToArray()).ToArray();
        }

        private List<double> createParticipantData(int teamCounter)
        {
            List<double> participantsData = new List<double>();
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

        private double[][] createInput()
        {
            List<List<double>> input = new List<List<double>>();
            for (int teamCounter = 0; teamCounter < teams.Count; teamCounter++)
            {
                List<double> participantsData = new List<double>();
                //if ((Top)teams[teamCounter].Participants.Find(p => p.Role is Top).Role != null
                //    && (Jungle)teams[teamCounter].Participants.Find(p => p.Role is Jungle).Role != null
                //    && (Mid)teams[teamCounter].Participants.Find(p => p.Role is Mid).Role != null
                //    && (Bot)teams[teamCounter].Participants.Find(p => p.Role is Bot).Role != null
                //    && (Support)teams[teamCounter].Participants.Find(p => p.Role is Support).Role != null
                //    )
                //{
                //    participantsData = createParticipantData(teamCounter);
                //}
                try
                {
                    participantsData = createParticipantData(teamCounter);
                }
                catch (NullReferenceException)
                {
                    
                }

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

        public void execute()
        {
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(new ActivationLinear(), true, 38));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 1));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 1));
            network.Structure.FinalizeStructure();
            network.Reset();

            var input = createInput();
            var ideal = createIdeal();

            IMLDataSet trainingSet = new BasicMLDataSet(input, ideal);

            IMLTrain train = new Backpropagation(network, trainingSet);
            int epoch = 1;
            //Muligvis problemematik pga af dmgToChamps
            do
            {
                train.Iteration();
                Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
                epoch++;
            } while (train.Error > 0.17);

            Console.WriteLine(@"Neural Network Results:");
            foreach (IMLDataPair pair in trainingSet)
            {
                IMLData output = network.Compute(pair.Input);
                Console.WriteLine(@" actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
            }
            Console.ReadKey();
        }

    }
}
