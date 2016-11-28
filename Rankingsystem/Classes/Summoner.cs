namespace Rankingsystem.Classes
{
    public class Summoner
    {
        public long PlayerId { get; set; }
        public string UserName { get; set; }
        public long RankingPoints { get; set; }

        public Summoner(long id, string userName)
        {
            PlayerId = id;
            UserName = userName;
        }
    }
}