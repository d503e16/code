namespace Rankingsystem.Classes.Roles
{
    public class Mid : Role
    {
        public Mid(bool fb, bool ft, double kda, long kp, long wards, double laneMinions,
            long minionDiff, long dmgToChamps, long enemyMonsters) : 
            base(fb, ft, kda, kp)
        {
            this.wards = wards;
            this.laneMinions = laneMinions;
            this.minionDiff = minionDiff;
            this.dmgToChamps = dmgToChamps;
            this.enemyMonsters = enemyMonsters;
        }
        //Mid features
        private long wards;
        private double laneMinions;
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

        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() + wards + (long)laneMinions +
                minionDiff + dmgToChamps + enemyMonsters;
        }

        public override string ToString()
        {
            return base.ToString() +
                "Lane Minions: " + laneMinions + "\n" +
                "Minion Difference: " + minionDiff + "\n" +
                "Damage to Champions: " + dmgToChamps + "\n" +
                "Wards: " + wards + "\n" +
                "Enemy Monsters: " + enemyMonsters;
        }
    }
}