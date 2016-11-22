using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rankingsystem.Classes.Roles;

namespace Rankingsystem.Classes
{
    class MatchAPI
    {
        public long MatchId { get; set; }
        public List<ParticipantAPI> Participants { get; set; }
        public List<ParticipantIdentityAPI> ParticipantIdentities { get; set; }
        public string QueueType { get; set; }

        public class ParticipantIdentityAPI
        {
            public int ParticipantId { get; set; }
            public PlayerAPI Player { get; set; }

            public class PlayerAPI
            {
                public long SummonerId { get; set; }
                public string SummonerName { get; set; }
            }
        }

        public class ParticipantAPI
        {
            public StatAPI Stats { get; set; }
            public int TeamId { get; set; }
            public ParticipantTimelineAPI Timeline { get; set; }
            public int ParticipantId { get; set; }

            public class StatAPI
            {
                public long Assists { get; set; }
                public long Deaths { get; set; }
                public long Kills { get; set; }
                public bool FirstBloodAssist { get; set; }
                public bool FirstBloodKill { get; set; }
                public bool FirstTowerAssist { get; set; }
                public bool FirstTowerKill { get; set; }
                public long MinionsKilled { get; set; }
                public long NeutralMinionsKilled { get; set; }
                public long NeutralMinionsKilledEnemyJungle { get; set; }
                public long NeutralMinionsKilledTeamJungle { get; set; }
                public long TotalDamageDealtToChampions { get; set; }
                public long WardsKilled { get; set; }
                public long WardsPlaced { get; set; }
                public bool Winner { get; set; }
            }

            public class ParticipantTimelineAPI
            {
                public string Role { get; set; }
                public string Lane { get; set; }
                public ParticipantTimelineDataAPI CreepsPerMinDeltas { get; set; }
                public ParticipantTimelineDataAPI CsDiffPerMinDeltas { get; set; }

                public class ParticipantTimelineDataAPI
                {
                    public double ZeroToTen { get; set; }
                    public double TenToTwenty { get; set; }
                }
            }
        }

        public Match Creatematch()
        {
            Team A = new Team();
            Team B = new Team();

            for (int i = 0; i < 10; i++)
            {
                this.getparticipant(i);
                if (this.getparticipant(i).TeamId == 100)
                {
                    A.Participants.Add(this.getparticipant(i));
                }
                else
                    B.Participants.Add(this.getparticipant(i));
            }
            match.Team1 = A;
            match.Team2 = B;

            return match;
        }

        private List<Participant> getParticipants()
        {
            List<Participant> result = new List<Participant>();
            foreach (ParticipantAPI p in Participants)
            {
                switch (p.Timeline.Role)
                {
                    case "DUO_CARRY":
                        var summoner = ParticipantIdentities.Find(pId => pId.ParticipantId == p.ParticipantId);
                        result.Add(new Participant(summoner.Player.SummonerId, fillBotData(p)));
                        break;

                    default:
                        break;
                }
            }
        }

        private Bot fillBotData(ParticipantAPI p)
        {
            
            var enemyCs = Participants.Find(enemy => enemy.TeamId != p.TeamId &&
                enemy.Timeline.Role == "DUO_CARRY").Stats.MinionsKilled;

            var teamKills = Participants.FindAll(player => player.TeamId == p.TeamId).
                Sum(player => player.Stats.Kills);
            var kda = p.Stats.Deaths != 0 ? ((double)p.Stats.Kills + p.Stats.Assists) / p.Stats.Deaths : 
                p.Stats.Kills + p.Stats.Assists;
            var killParticipation = teamKills != 0 ? ((double)p.Stats.Kills + p.Stats.Assists) / teamKills :
                0;

            return new Bot(p.Stats.FirstBloodAssist || p.Stats.FirstBloodKill,
                p.Stats.FirstTowerAssist || p.Stats.FirstTowerKill,
                kda,
                killParticipation,
                (p.Timeline.CreepsPerMinDeltas.ZeroToTen + p.Timeline.CreepsPerMinDeltas.TenToTwenty) / 2,
                p.Stats.MinionsKilled - enemyCs, 
                p.Stats.TotalDamageDealtToChampions);
        }
       
        private Support fillsupportdata(Participant p, int i)
        {
            Support s = new Support();
            p.Role = s;

            var stats = this.Participants[i].Stats;

            fillGeneralData(p, i);
            s.Assists = stats.Assists;

            return s;
        }
        private Top filltopdata(Participant p, int i)
        {
            Top t = new Top();
            p.Role = t;

            var stats = this.Participants[i].Stats;

            fillGeneralData(p, i);
            t.LaneMinions = this.Participants[i].Timeline.CreepsPerMinDeltas.ZeroToTen + this.Participants[i].Timeline.CreepsPerMinDeltas.TenToTwenty;
            t.DmgToChamps = stats.TotalDamageDealtToChampions;
            t.Assists = stats.Assists;
            t.Deaths = stats.Deaths;

            return t;
        }
        private Mid fillmiddata(Participant p, int i)
        {
            Mid m = new Mid();
            p.Role = m;

            var stats = this.Participants[i].Stats;
            var enemy = this.Participants.Find(x => x.TeamId != p.TeamId && x.Timeline.Role == "SOLO" && x.Timeline.Lane == "MIDDLE" || x.Timeline.Lane == "MID");

            fillGeneralData(p, i);
            m.MinionDiff = stats.MinionsKilled - enemy.Stats.MinionsKilled;
            m.DmgToChamps = stats.TotalDamageDealtToChampions;
            m.EnemyMonsters = stats.NeutralMinionsKilledEnemyJungle;

            return m;
        }
        private Jungle filljungledata(Participant p, int i)
        {
            Jungle j = new Jungle();
            p.Role = j;

            var stats = this.Participants[i].Stats;

            fillGeneralData(p, i);
            j.EnemyMonsters = stats.NeutralMinionsKilledTeamJungle;
            j.EnemyMonsters = stats.NeutralMinionsKilledEnemyJungle;

            return j;
        }
    }  
}
