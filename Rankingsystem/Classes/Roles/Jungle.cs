using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    class Jungle
    {
        //Jungle features
        private int ownMonsters;
        private int enemyMonsters;
        private int wards;

        public int Wards
        {
            get { return wards; }
            set { wards = value; }
        }

        public int EnemyMonsters
        {
            get { return enemyMonsters; }
            set { enemyMonsters = value; }
        }

        public int OwnMonsters
        {
            get { return ownMonsters; }
            set { ownMonsters = value; }
        }

    }
}