using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    abstract class Role : Participant
    {
        //Generelle features
        private Boolean firstBlood;
        private Boolean firstTurret;
        private float _KDA;
        private float _KP;

        public float KP
        {
            get { return _KP; }
            set { _KP = value; }
        }

        public float KDA
        {
            get { return _KDA; }
            set { _KDA = value; }
        }

        public Boolean FirstTurret
        {
            get { return firstTurret; }
            set { firstTurret = value; }
        }
        
        public Boolean FirstBlood
        {
            get { return firstBlood; }
            set { firstBlood = value; }
        }

    }
}