using System;
using System.Collections.Generic;

namespace Rankingsystem.Classes.Roles
{
    public class Bot : Role
    {
        public Bot(bool firstBlood, bool firstTurret, double KDA, long KP, 
            double laneMinion, long minionDiff, long dmgToChamps) : 
            base(firstBlood, firstTurret, KDA, KP)
        {
            this.laneMinion = laneMinion;
            this.minionDiff = minionDiff;
            this.dmgToChamps = dmgToChamps;
        }

        //Bot features
        private double laneMinion;
        private long minionDiff;
        private long dmgToChamps;
        
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
            get { return laneMinion; }
            set { laneMinion = value; }
        }
        
        public override List<double> GetData()
        {
            List<double> list = base.GetData();
            list.Add((double)minionDiff);
            list.Add((double)dmgToChamps/10000);
            list.Add(laneMinion);
            return list;
        }

        public int DmgToChampsScore
        {
            get
            {
                if (dmgToChamps >= 0 && dmgToChamps <= 10000) return 1;
                else if (dmgToChamps > 10000 && dmgToChamps <= 20000) return 2;
                else if (dmgToChamps > 20000 && dmgToChamps <= 30000) return 3;
                else if (dmgToChamps > 30000 && dmgToChamps <= 40000) return 4;
                else return 5;
            }
        }

        public int MinionDiffScore
        {
            get
            {
                if (minionDiff < 0) return 1;
                else if (minionDiff >= 0 && minionDiff <= 15) return 2;
                else if (minionDiff > 15 && minionDiff <= 30) return 3;
                else if (minionDiff > 30 && minionDiff <= 45) return 4;
                else return 5;
            }
        }

        public int LaneMinionsScore
        {
            get
            {
                if (laneMinion >= 0 && laneMinion <= 2) return 1;
                else if (laneMinion > 2 && laneMinion <= 4) return 2;
                else if (laneMinion > 4 && laneMinion <= 6) return 3;
                else if (laneMinion > 6 && laneMinion <= 8) return 4;
                else return 5;
            }
        }

        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() +
                Convert.ToInt64(((double)DmgToChampsScore + MinionDiffScore + LaneMinionsScore) / 3);
        }

        public override string ToString()
        {
            return base.ToString() +
                "Minion Difference: " + MinionDiffScore + "\n" +
                "Lane Minions: " + LaneMinionsScore + "\n" +
                "Damage to Champions: " + DmgToChampsScore;
        }
    }
}