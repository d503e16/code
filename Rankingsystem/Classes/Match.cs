using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    class Match
    {
        private Team team1;
        private Team team2;

        public Team Team2
        {
            get { return team2; }
            set { team2 = value; }
        }


        public Team Team1
        {
            get { return team1; }
            set { team1 = value; }
        }

    }
}
