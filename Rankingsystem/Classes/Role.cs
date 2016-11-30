using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    public abstract class Role
    {
        public Role(bool fb, bool ft, double kda, double kp)
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
        private double _KP;

        public double KP
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
    }
}