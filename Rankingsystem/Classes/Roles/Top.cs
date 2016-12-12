using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rankingsystem.Classes.Roles
{
    public class Top : Role
    {
        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
        public Top(bool fb, bool ft, double kda, long kp, double laneMinions,
            long dmgToChamps, long assists, long deaths) :
            base(fb, ft, kda, kp)
        {
            this.laneMinions = laneMinions;
            this.dmgToChamps = dmgToChamps;
            this.assists = assists;
            this.deaths = deaths;
        }
        //Top features
        private double laneMinions;
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

        public double LaneMinions
        {
            get { return laneMinions; }
            set { laneMinions = value; }
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
        
        public override List<string> GetData()
        {
            List<string> list = base.GetData();
            list.Add(laneMinions.ToString(CultureInfo.InvariantCulture));
            list.Add(dmgToChamps.ToString(CultureInfo.InvariantCulture));
            list.Add(assists.ToString(CultureInfo.InvariantCulture));
            list.Add(deaths.ToString(CultureInfo.InvariantCulture));
            return list;
        }

        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() +
                Convert.ToInt64(
                    ((double)DmgToChampsScore + LaneMinionsScore +
                    DeathScore + AssistScore + 0.00001) / 4);
        }

        public override string ToString()
        {
            return base.ToString() +
                "Lane Minions: " + LaneMinionsScore + "\n" +
                "Damage to Champions: " + DmgToChampsScore + "\n" +
                "Deaths: " + DeathScore + "\n" +
                "Assist: " + AssistScore;
        }
    }
}