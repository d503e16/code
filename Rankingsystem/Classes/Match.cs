using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (team1.Winner) p.RankingPoints += CalculateRank(p.Role) + 50; // Winning team + individual perf
                else p.RankingPoints += CalculateRank(p.Role) - 50; // Losing team + individual perf
                db.UpdateSummoner(p as Summoner);
            }
            foreach (Participant p in team2.Participants)
            {
                if (team2.Winner) p.RankingPoints += CalculateRank(p.Role) + 50; // Winning team + individual perf
                else p.RankingPoints += CalculateRank(p.Role) - 50; // Losing team + individual perf
                db.UpdateSummoner(p as Summoner);
            }
        }
      
        private long CalculateRank(Role r)
        {
            long returnValue = (long)Math.Round(r.KDA) +
                        (long)(Math.Round(r.KP, 2) * 100);
            if (r.FirstBlood) returnValue += 50;
            if (r.FirstTurret) returnValue += 100;

            switch (r.GetType().Name)
            {
                case "Support":
                    var support = r as Support;
                    returnValue += support.Assists;
                    break;
                case "Bot":
                    var bot = r as Bot;
                    returnValue += (long)Math.Round(bot.LaneMinions, 2) +
                        bot.MinionDiff +
                        bot.DmgToChamps;
                    break;
                case "Mid":
                    var mid = r as Mid;
                    returnValue += mid.DmgToChamps +
                        mid.EnemyMonsters +
                        (long)Math.Round(mid.LaneMinions) +
                        mid.MinionDiff +
                        mid.Wards;
                    break;
                case "Top":
                    var top = r as Top;
                    returnValue += top.Assists +
                        top.Deaths +
                        top.DmgToChamps +
                        (long)Math.Round(top.LaneMinions) +
                        top.MinionDiff +
                        top.Wards;
                    break;
                case "Jungle":
                    var jungle = r as Jungle;
                    returnValue += jungle.EnemyMonsters +
                        jungle.OwnMonsters +
                        jungle.Wards;
                    break;
                default:
                    returnValue = 0;
                    break;
            }

            return returnValue;
        }
    }
}
