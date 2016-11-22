using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    public class Summoner
    {
        public long PlayerId { get; set; }
        public string UserName { get; set; }
        public long RankingPoints { get; set; }

        public Summoner(long id, string userName, long points)
        {
            PlayerId = id;
            UserName = userName;
            RankingPoints = points;
        }
    }
}