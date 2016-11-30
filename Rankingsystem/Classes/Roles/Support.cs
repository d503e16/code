using System.Collections.Generic;

namespace Rankingsystem.Classes.Roles
{
    public class Support : Role
    {
        public Support(bool fb, bool ft, double kda, long kp, long assists) : 
            base(fb, ft, kda, kp)
        {
            this.assists = assists;
        }

        private long assists;

        public long Assists
        {
            get { return assists; }
            set { assists = value; }
        }
        
        public override double[] GetData()
        {
            List<double> list = new List<double>();
            list.Add((double)assists);
            list.Add(convertBool(FirstBlood));
            list.Add(convertBool(FirstTurret));
            list.Add(KP);
            list.Add(KDA);
            return list.ToArray();
        }

        public int AssistScore
        {
            get
            {
                if (assists == 0) return 1;
                else if (assists > 0 && assists <= 10) return 2;
                else if (assists > 10 && assists <= 14) return 3;
                else if (assists > 14 && assists <= 20) return 4;
                else return 5;
            }
        }

        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() + AssistScore;
        }

        public override string ToString()
        {
            return base.ToString() +
                "Assists: " + AssistScore;
        }
    }
}