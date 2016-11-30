using System;

namespace Rankingsystem.Classes
{
    public abstract class Role
    {
        public Role(bool fb, bool ft, double kda, long kp)
        {
            this.firstBlood = fb;
            this.firstTurret = ft;
            this._KDA = kda;
            this._KP = kp;
        }
        
        //Generelle features
        private bool firstBlood;
        private bool firstTurret;
        private double _KDA;
        private long _KP;

        public long KP
        {
            get { return _KP; }
            set { _KP = value; }
        }

        public double KDA
        {
            get { return _KDA; }
            set { _KDA = value; }
        }

        public bool FirstTurret
        {
            get { return firstTurret; }
            set { firstTurret = value; }
        }
        
        public bool FirstBlood
        {
            get { return firstBlood; }
            set { firstBlood = value; }
        }
        
        public abstract double[] getData();

        public double convertBool(bool b)
        {
            if (b == true) return 1.0;
            else return 0.0;
        }

        public virtual long IndividualPerformance()
        {
            long returnValue = 0;
            returnValue = (long)Math.Round(_KDA) + _KP;
            if (firstBlood) returnValue += 50;
            if (firstTurret) returnValue += 100;

            return returnValue;
        }

        public override string ToString()
        {
            return "KDA: " + _KDA + "\n" +
                "Kill participation: " + _KP + "\n" +
                "First Blood: " + firstBlood + "\n" +
                "First Turret: " + firstTurret + "\n";
        }
    }
}