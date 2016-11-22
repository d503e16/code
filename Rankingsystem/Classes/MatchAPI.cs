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
            }
        }

        public class ParticipantAPI
        {
            public StatAPI Stats { get; set; }
            public int TeamId { get; set; }
            public ParticipantTimelineAPI Timeline { get; set; }

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

        //Participant p = new Participant();
        //p.TeamId = match.Participants[0].TeamId;
        //    Jungle j = match.Participants.Find(p => p.Timeline.Role == "JUNGLE" && p.TeamId == 100);
        //    if (match.Participants[0].Timeline.Role == "Jungle")
        //    {
        //        Jungle Ja = new Jungle();
        //p.Roleprop = Ja;
        //    }

    public Participant getparticipant(int i)
        {
            Participant p = new Participant();
            p.TeamId = this.Participants[i].TeamId;

            switch (this.Participants[i].Timeline.Role)
            {
                case "DUO_CARRY":
                    p.Role = fillbotdata(p, i);
                    break;
                case "NONE":
                    p.Role = new Jungle();
                    break;
                case "DUO_SUPPORT":
                    p.Role = new Support();
                    break;
                case "SOLO":
                    if (this.Participants[i].Timeline.Lane == "MIDDLE")
                    {
                        p.Role = new Mid();
                    }
                    else
                        p.Role = new Top();
                    break;
            }
            return p;
        }
        private Bot fillbotdata(Participant p, int i)
        {
            Bot b = new Bot();
            p.Role = b;

            var stats = this.Participants[i].Stats;
            var enemy = this.Participants.Find(x => x.TeamId != p.TeamId && x.Timeline.Role == "DUO_CARRY");

            fillgeneraldata(p, i);
            b.DmgToChamps = stats.TotalDamageDealtToChampions;
            b.LaneMinions = this.Participants[i].Timeline.CreepsPerMinDeltas.ZeroToTen + this.Participants[i].Timeline.CreepsPerMinDeltas.TenToTwenty;
            b.MinionDiff = stats.MinionsKilled - enemy.Stats.MinionsKilled;

            return b;
        }
        private void fillgeneraldata(Participant p, int i)
        {
            var stats = this.Participants[i].Stats;
            var teamkills = this.Participants.FindAll(x => x.TeamId == p.TeamId).Sum(x => x.Stats.Kills + x.Stats.Assists);

            p.Role.FirstBlood = stats.FirstBloodKill || stats.FirstBloodAssist;
            p.Role.FirstTurret = stats.FirstTowerKill || stats.FirstTowerAssist;
            if (stats.Deaths == 0)
            {
                p.Role.KDA = stats.Kills + stats.Assists;
            }
            else
            {
                p.Role.KDA = (stats.Kills + stats.Assists) / stats.Deaths;
            }
            p.Role.KP = teamkills / (stats.Assists + stats.Kills);
        }
    }  
}
