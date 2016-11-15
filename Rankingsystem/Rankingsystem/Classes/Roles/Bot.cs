using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    class Bot
    {
        //Bot features
        private float laneMinion;
        private int minionDiff;
        private int dmgToChamps;

        public int DmgToChamps
        {
            get { return dmgToChamps; }
            set { dmgToChamps = value; }
        }

        public int MinionDiff
        {
            get { return minionDiff; }
            set { minionDiff = value; }
        }

        public float LaneMinions
        {
            get { return laneMinion; }
            set { laneMinion = value; }
        }

    }
}
