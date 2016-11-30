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

<<<<<<< HEAD
        public override double[] getData()
        {
            List<double> list = new List<double>();
            list.Add((double)assists);
            list.Add(convertBool(FirstBlood));
            list.Add(convertBool(FirstTurret));
            list.Add(KP);
            list.Add(KDA);
            return list.ToArray();
        }
=======
        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() + assists;
        }

        public override string ToString()
        {
            return base.ToString() +
                "Assists: " + assists;
        }

>>>>>>> 344e3ba751e383ca27cb07a0ea195b2a89e84f04
    }
}