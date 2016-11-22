using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    class Jungle : Role
    {
        //Jungle features
        private long ownMonsters;
        private long enemyMonsters;
        private int wards;

        public int Wards
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