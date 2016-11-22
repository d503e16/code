using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    public class Jungle : Role
    {
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

    }
}