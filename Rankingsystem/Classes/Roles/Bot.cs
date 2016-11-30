using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    public class Bot : Role
    {
        public Bot(bool firstBlood, bool firstTurret, double KDA, double KP, 
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

        public override double[] getData()
        {
            List<double> list = new List<double>();
            list.Add((double)minionDiff);
            list.Add((double)dmgToChamps);
            list.Add(laneMinion);
            list.Add(convertBool(FirstBlood));
            list.Add(convertBool(FirstTurret));
            list.Add(KP);
            list.Add(KDA);
            return list.ToArray();
            }
    }
}