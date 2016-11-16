using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    class Top
    {
        //Top features
        private int wards;
        private float laneMinions;
        private int minionDiff;
        private int dmgToChamps;
        private int assists;
        private int deaths;

        public int Deaths
        {
            get { return deaths; }
            set { deaths = value; }
        }

        public int Assists
        {
            get { return assists; }
            set { assists = value; }
        }

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
            get { return laneMinions; }
            set { laneMinions = value; }
        }

        public int Wards
        {
            get { return wards; }
            set { wards = value; }
        }

    }
}