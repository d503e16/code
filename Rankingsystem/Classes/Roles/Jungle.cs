using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rankingsystem.Classes.Roles
{
    public class Jungle : Role
    {
        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
        public Jungle(bool fb, bool ft, double kda, long kp, long ownMonsters, 
            long enemyMonsters) :
            base(fb, ft, kda, kp)
        {
            this.ownMonsters = ownMonsters;
            this.enemyMonsters = enemyMonsters;
        }
        //Jungle features
        private long ownMonsters;
        private long enemyMonsters;

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
            return list;
        }

        public override List<string> GetNormalizedData(int fbMax, int ftMax, int KPMax, double KDAMax)
        {
            const double ownMonstersMax = 135, enemyMonstersMax = 66;
        
            List<string> list = base.GetNormalizedData(fbMax, ftMax, KPMax, KDAMax);
            list.Add((ownMonsters / ownMonstersMax).ToString(CultureInfo.InvariantCulture));
            list.Add((enemyMonsters / enemyMonstersMax).ToString(CultureInfo.InvariantCulture));
            return list;
        }

        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() + 
                Convert.ToInt64(((double)EnemyMonstersScore + OwnMonstersScore + 0.00001) / 2);
        }

        public override string ToString()
        {
            return base.ToString() +
                "Enemy Monsters: " + EnemyMonstersScore + "\n" +
                "Own Monsters: " + OwnMonstersScore;
        }
    }
}