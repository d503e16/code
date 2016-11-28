namespace Rankingsystem.Classes.Roles
{
    public class Support : Role
    {
        public Support(bool fb, bool ft, double kda, double kp, long assists) : 
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

    }
}