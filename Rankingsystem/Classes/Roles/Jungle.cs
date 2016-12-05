using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rankingsystem.Classes.Roles
{
    public class Jungle : Role
    {
        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
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
        
        public int WardsScore
        {
            get
            {
                if (wards >= 0 && wards <= 10) return 1;
                else if (wards > 10 && wards <= 20) return 2;
                else if (wards > 20 && wards <= 30) return 3;
                else if (wards > 30 && wards <= 40) return 4;
                else return 5;
            }
        }

        public int EnemyMonstersScore
        {
            get
            {
                if (enemyMonsters >= 0 && enemyMonsters <= 10) return 1;
                else if (enemyMonsters > 10 && enemyMonsters <= 20) return 2;
                else if (enemyMonsters > 20 && enemyMonsters <= 30) return 3;
                else if (enemyMonsters > 30 && enemyMonsters <= 40) return 4;
                else return 5;
            }
        }

        public int OwnMonstersScore
        {
            get
            {
                if (ownMonsters >= 0 && ownMonsters <= 20) return 1;
                else if (ownMonsters > 20 && ownMonsters <= 40) return 2;
                else if (ownMonsters > 40 && ownMonsters <= 60) return 3;
                else if (ownMonsters > 60 && ownMonsters <= 80) return 4;
                else return 5;
            }
        }

        public override List<string> GetData()
        {
            List<string> list = base.GetData();
            list.Add(ownMonsters.ToString(CultureInfo.InvariantCulture));
            list.Add(enemyMonsters.ToString(CultureInfo.InvariantCulture));
            list.Add(wards.ToString(CultureInfo.InvariantCulture));
            return list;
        }

        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() + 
                Convert.ToInt64(((double)WardsScore + EnemyMonstersScore + OwnMonstersScore) / 3);
        }

        public override string ToString()
        {
            return base.ToString() +
                "Enemy Monsters: " + EnemyMonstersScore + "\n" +
                "Own Monsters: " + OwnMonstersScore + "\n" +
                "Wards: " + WardsScore;
        }
    }
}