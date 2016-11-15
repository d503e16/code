using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    class Summoner
    {
        private int playerId;
        private int rankingPoints;
        private string userName;

        public int PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public int RankingPoints
        {
            get { return rankingPoints; }
            set { rankingPoints = value; }
        }

    }
}
