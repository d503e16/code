using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rankingsystem.Classes.Roles
{
    public class Mid : Role
    {
        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
        public Mid(bool fb, bool ft, double kda, long kp,
            long minionDiff, long dmgToChamps, long enemyMonsters) : 
            base(fb, ft, kda, kp)
        {
            this.minionDiff = minionDiff;
            this.dmgToChamps = dmgToChamps;
            this.enemyMonsters = enemyMonsters;
        }
        //Mid features
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

        public int EnemyMonstersScore
        {
            get
            {
                if (enemyMonsters >= 0 && enemyMonsters <= 5) return 1;
                else if (enemyMonsters > 5 && enemyMonsters <= 10) return 2;
                else if (enemyMonsters > 10 && enemyMonsters <= 15) return 3;
                else if (enemyMonsters > 15 && enemyMonsters <= 20) return 4;
                else return 5;
            }
        }

        public override List<string> GetData()
        {
            List<string> list = base.GetData();
            list.Add(minionDiff.ToString(CultureInfo.InvariantCulture));
            list.Add(dmgToChamps.ToString(CultureInfo.InvariantCulture));
            list.Add(enemyMonsters.ToString(CultureInfo.InvariantCulture));
            return list;
        }

        public override List<string> GetNormalizedData(int fbMax, int ftMax, int KPMax , double KDAMax)
        {
            const double minionDiffMax = 233, dmgToChampsMax = 89532, enemyMonstersMax = 25;
            List<string> list = base.GetNormalizedData(fbMax, ftMax, KPMax, KDAMax);
            list.Add((minionDiff / minionDiffMax).ToString(CultureInfo.InvariantCulture));
            list.Add((dmgToChamps / dmgToChampsMax).ToString(CultureInfo.InvariantCulture));
            list.Add((enemyMonsters / enemyMonstersMax).ToString(CultureInfo.InvariantCulture));
            return list;
        }

        public override long IndividualPerformance()
        {
            return base.IndividualPerformance() + 
                Convert.ToInt64(((double)MinionDiffScore + DmgToChampsScore + EnemyMonstersScore) / 3);
        }

        public override string ToString()
        {
            return base.ToString() +
                "Minion Difference: " + MinionDiffScore + "\n" +
                "Damage to Champions: " + DmgToChampsScore + "\n" +
                "Enemy Monsters: " + EnemyMonstersScore;
        }
    }
}