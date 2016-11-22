using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    public class Top : Role
    {
        //Top features
        private long wards;
        private double laneMinions;
        private long minionDiff;
        private long dmgToChamps;
        private long assists;
        private long deaths;

        public long Deaths
        {
            get { return deaths; }
            set { deaths = value; }
        }

        public long Assists
        {
            get { return assists; }
            set { assists = value; }
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

        public double LaneMinions
        {
            get { return laneMinions; }
            set { laneMinions = value; }
        }

        public long Wards
        {
            get { return wards; }
            set { wards = value; }
        }
    }
}