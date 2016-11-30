using System.Collections.Generic;

namespace Rankingsystem.Classes.Roles
{
    public class Jungle : Role
    {
        public Jungle(bool fb, bool ft, double kda, long kp, long ownMonsters, 
            long enemyMonsters, long wards) :
            base(fb, ft, kda, kp)
        {
            this.ownMonsters = ownMonsters;
            this.enemyMonsters = enemyMonsters;
            this.wards = wards;
        }
        //Jungle features
        private long ownMonsters;
        private long enemyMonsters;
        private long wards;

        public long Wards
        {
            get { return wards; }
            set { wards = value; }
        }

        public long EnemyMonsters
        {
            get { return enemyMonsters; }
            set { enemyMonsters = value; }
        }

        public long OwnMonsters
        {
            get { return ownMonsters; }
            set { ownMonsters = value; }
        }

        public override double[] getData()
        {
            List<double> list = new List<double>();
            list.Add((double)ownMonsters);
            list.Add((double)enemyMonsters);
            list.Add((double)wards);
            list.Add(convertBool(FirstBlood));
            list.Add(convertBool(FirstTurret));
            list.Add(KP);
            list.Add(KDA);
            return list.ToArray();
        }

        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() + enemyMonsters + ownMonsters + wards;
        }

        public override string ToString()
        {
            return base.ToString() +
                "Enemy Monsters: " + enemyMonsters + "\n" +
                "Own Monsters: " + ownMonsters + "\n" +
                "Wards: " + wards;
        }
    }
}