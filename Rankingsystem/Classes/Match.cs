using System;
using Rankingsystem.Classes.Roles;

namespace Rankingsystem.Classes
{
    public class Match
    {
        public Match(Team t1, Team t2)
        {
            this.team1 = t1;
            this.team2 = t2;
        }
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

        public void UpdateRanks(Database db)
        {
            foreach (Participant p in team1.Participants)
            {
                if (team1.Winner) p.RankingPoints += p.Role.IndividualPerformance() + 50; // Winning team + individual perf
                else p.RankingPoints += p.Role.IndividualPerformance() - 50; // Losing team + individual perf
                db.UpdateSummoner(p as Summoner);
            }
            foreach (Participant p in team2.Participants)
            {
                if (team2.Winner) p.RankingPoints += p.Role.IndividualPerformance() + 50; // Winning team + individual perf
                else p.RankingPoints += p.Role.IndividualPerformance() - 50; // Losing team + individual perf
                db.UpdateSummoner(p as Summoner);
            }
        }
    }
}
