using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    class Mid : Role
    {
        //Mid features
        private int wards;
        private float laneMinions;
        private long minionDiff;
        private long dmgToChamps;
        private long enemyMonsters;

        public long EnemyMonsters
        {
            get { return enemyMonsters; }
            set { enemyMonsters = value; }
        }


        public long DmgToChamps
        {
            get { return dmgToChamps; }
            set { dmgToChamps = value; }
        }

        public long MinionDiff
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