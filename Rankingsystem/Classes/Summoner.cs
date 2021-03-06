using System.Collections.Generic;

namespace Rankingsystem.Classes
{
    public class Summoner
    {
        private long rankingPoints;

        public long PlayerId { get; set; }
        public string UserName { get; set; }
        public long RankingPoints {
            get
            {
                return rankingPoints;
            }
            set
            {
                if (value < 0) rankingPoints = 0;
                else rankingPoints = value;
            }
        }
        public List<long> MatchIds { get; set; }

        public Summoner(long id, string userName)
        {
            PlayerId = id;
            UserName = userName;
            MatchIds = new List<long>();
        }

    }
}