using System;
using Rankingsystem.Classes.Roles;
using System.Collections.Generic;

namespace Rankingsystem.Classes
{
    public class Match
    {
        public long MatchId { get; set; }

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
            const int winningPoints = 40;
            const int losingPoints = 50;

            foreach (Participant p in team1.Participants)
            {
                if (team1.Winner) p.RankingPoints += p.Role.IndividualPerformance() + winningPoints; // Winning team + individual perf
                else p.RankingPoints += p.Role.IndividualPerformance() - losingPoints; // Losing team + individual perf
                db.UpdateSummoner(p as Summoner);
            }
            foreach (Participant p in team2.Participants)
            {
                if (team2.Winner) p.RankingPoints += p.Role.IndividualPerformance() + winningPoints; // Winning team + individual perf
                else p.RankingPoints += p.Role.IndividualPerformance() - losingPoints; // Losing team + individual perf
                db.UpdateSummoner(p as Summoner);
            }
        }
    }
}
