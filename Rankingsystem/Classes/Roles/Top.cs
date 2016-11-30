using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    public class Top : Role
    {
        public Top(bool fb, bool ft, double kda, double kp, long wards, double laneMinions,
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

        public override double[] getData()
        {
            List<double> list = new List<double>();
            list.Add(laneMinions);
            list.Add((double)minionDiff);
            list.Add((double)wards);
            list.Add((double)dmgToChamps);
            list.Add((double)assists);
            list.Add((double)deaths);
            list.Add(convertBool(FirstBlood));
            list.Add(convertBool(FirstTurret));
            list.Add(KP);
            list.Add(KDA);
            return list.ToArray();
        }
    }
}