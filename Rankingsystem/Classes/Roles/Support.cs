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

        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() + assists;
        }

        public override string ToString()
        {
            return base.ToString() +
                "Assists: " + assists;
        }

    }
}