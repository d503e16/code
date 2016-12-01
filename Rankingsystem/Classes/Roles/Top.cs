using System;
using System.Collections.Generic;

namespace Rankingsystem.Classes.Roles
{
    public class Top : Role
    {
        public Top(bool fb, bool ft, double kda, long kp, long wards, double laneMinions,
            long minionDiff, long dmgToChamps, long assists, long deaths) :
            base(fb, ft, kda, kp)
        {
            this.wards = wards;
            this.laneMinions = laneMinions;
            this.minionDiff = minionDiff;
            this.dmgToChamps = dmgToChamps;
            this.assists = assists;
            this.deaths = deaths;
        }
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
                if (laneMinions >= 0 && laneMinions <= 2) return 1;
                else if (laneMinions > 2 && laneMinions <= 4) return 2;
                else if (laneMinions > 4 && laneMinions <= 6) return 3;
                else if (laneMinions > 6 && laneMinions <= 8) return 4;
                else return 5;
            }
        }

        public int WardsScore
        {
            get
            {
                if (wards >= 0 && wards <= 5) return 1;
                else if (wards > 5 && wards <= 10) return 2;
                else if (wards > 10 && wards <= 15) return 3;
                else if (wards > 15 && wards <= 20) return 4;
                else return 5;
            }
        }

        public int DeathScore
        {
            get
            {
                if (deaths >= 15) return 1;
                else if (deaths < 15 && deaths >= 10) return 2;
                else if (deaths < 10 && deaths >= 6) return 3;
                else if (deaths < 6 && deaths >= 3) return 4;
                else return 5;
            }
        }

        public int AssistScore
        {
            get
            {
                if (assists == 0) return 1;
                else if (assists > 0 && assists <= 5) return 2;
                else if (assists > 5 && assists <= 8) return 3;
                else if (assists > 8 && assists <= 10) return 4;
                else return 5;
            }
        }
        
        public override List<double> GetData()
        {
            List<double> list = base.GetData();
            list.Add(laneMinions);
            list.Add((double)minionDiff);
            list.Add((double)wards);
            list.Add((double)dmgToChamps);
            list.Add((double)assists);
            list.Add((double)deaths);
            return list;
        }

        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() +
                Convert.ToInt64(
                    ((double)DmgToChampsScore + LaneMinionsScore + MinionDiffScore + 
                WardsScore + DeathScore + AssistScore + 0.00001) / 6);
        }

        public override string ToString()
        {
            return base.ToString() +
                "Lane Minions: " + LaneMinionsScore + "\n" +
                "Minion Difference: " + MinionDiffScore + "\n" +
                "Damage to Champions: " + DmgToChampsScore + "\n" +
                "Wards: " + WardsScore + "\n" +
                "Deaths: " + DeathScore + "\n" +
                "Assist: " + AssistScore;
        }
    }
}