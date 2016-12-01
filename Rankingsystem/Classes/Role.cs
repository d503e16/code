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
        
        public abstract double[] GetData();

        public double convertBool(bool b)
        {
            if (b == true) return 1.0;
            else return 0.0;
        }

        public int KDAScore {
            get
            {
                if (_KDA >= 0.0 && 1.0 >= _KDA) return 1;
                else if (_KDA > 1.0 && 2.0 >= _KDA) return 2;
                else if (_KDA > 3.0 && 4.0 >= _KDA) return 3;
                else if (_KDA > 4.0 && 5.0 >= _KDA) return 4;
                else return 5;
            }
        }

        public int KPScore
        {
            get
            {
                if (_KP >= 0 && 20 >= _KP) return 1;
                else if (_KP > 20 && 40 >= _KP) return 2;
                else if (_KP > 40 && 60 >= _KP) return 3;
                else if (_KP > 60 && 80 >= _KP) return 4;
                else return 5;
            }
        }

        public int FirstBloodScore {
            get
            {
                return firstBlood == true ? 5 : 1;
            }
        }

        public int FirstTurretScore
        {
            get
            {
                return firstTurret == true ? 5 : 1;
            }
        }

        public virtual long IndividualPerformance()
        {
            return Convert.ToInt64((KDAScore + KPScore + FirstBloodScore + FirstTurretScore + 0.00001) / 4);
        }
        
        public override string ToString()
        {
            return  
                "KDA: " + KDAScore + "\n" +
                "Kill participation: " + KPScore + "\n" +
                "First Blood: " + FirstBloodScore + "\n" +
                "First Turret: " + FirstTurretScore + "\n";
        }
    }
}