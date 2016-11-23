﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rankingsystem.Classes.Roles;

namespace Rankingsystem.Classes
{
    public class MatchAPI
    {
        public long MatchId { get; set; }
        public List<ParticipantAPI> Participants { get; set; }
        public List<ParticipantIdentityAPI> ParticipantIdentities { get; set; }
        public List<TeamAPI> Teams { get; set; }
        public string QueueType { get; set; }

        public class TeamAPI
        {
            public int TeamId { get; set; }
            public bool Winner { get; set; }
        }

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
            public bool FirstBlood { get { return Stats.FirstBloodAssist || Stats.FirstBloodKill; } }
            public bool FirstTurret { get { return Stats.FirstTowerAssist || Stats.FirstTowerKill; } }
            public double KDA {
                get
                {
                    return Stats.Deaths != 0 ?
                      ((double)Stats.Kills + Stats.Assists) / Stats.Deaths : 
                      (double)Stats.Kills + Stats.Assists;
                }
            }
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

        public Match CreateMatch()
        {
            return new Match(createTeam(100), createTeam(200));
        }

        private Team createTeam(int teamId)
        {
            List<ParticipantAPI> list = Participants.FindAll(p => p.TeamId == teamId);
            List<Participant> teamParticipants = new List<Participant>();

            foreach (ParticipantAPI p in list)
            {
                teamParticipants.Add(createParticipant(p));
            }
            return new Team(teamParticipants, Teams.Find(team => team.TeamId == teamId).Winner);
        }

        private Participant createParticipant(ParticipantAPI p)
        {
            var summonerId = ParticipantIdentities.Find(
                pId => pId.ParticipantId == p.ParticipantId).Player.SummonerId;
            switch (p.Timeline.Role)
            {
                case "DUO_CARRY":
                    return new Participant(summonerId, createBotData(p));
                case "DUO_SUPPORT":
                    return new Participant(summonerId, createSupportData(p));
                case "NONE":
                    return new Participant(summonerId, createJungleData(p));
                case "SOLO":
                    if (p.Timeline.Lane == "MIDDLE" || p.Timeline.Lane == "MID")
                        return new Participant(summonerId, createMidData(p));
                    else if (p.Timeline.Lane == "TOP")
                        return new Participant(summonerId, createTopData(p));
                    else return null;
                default:
                    return null;
            }
        }

        private double getKillParticipation(ParticipantAPI p)
        { 
            var teamKills = Participants.FindAll(player => player.TeamId == p.TeamId).
                Sum(player => player.Stats.Kills);
            var killParticipation = teamKills != 0 ?
                ((double)p.Stats.Kills + p.Stats.Assists) / teamKills :
                0;
            return killParticipation;
        } 

        // Lav props frem for udregninger!
        private Bot createBotData(ParticipantAPI p)
        {  
            var enemyCs = Participants.Find(enemy => enemy.TeamId != p.TeamId &&
                enemy.Timeline.Role == "DUO_CARRY").Stats.MinionsKilled;

            return new Bot(p.FirstBlood,
                p.FirstTurret,
                p.KDA,
                getKillParticipation(p),
                (p.Timeline.CreepsPerMinDeltas.ZeroToTen + p.Timeline.CreepsPerMinDeltas.TenToTwenty) / 2,
                p.Stats.MinionsKilled - enemyCs, 
                p.Stats.TotalDamageDealtToChampions);
        }
       
        private Support createSupportData(ParticipantAPI p)
        {
            return new Support(
                p.FirstBlood,
                p.FirstTurret,
                p.KDA,
                getKillParticipation(p),
                p.Stats.Assists);
        }

        private Top createTopData(ParticipantAPI p)
        {
            long enemyCs = Participants.Find(enemy => enemy.Timeline.Lane == p.Timeline.Lane
                && p.TeamId != enemy.TeamId).Stats.MinionsKilled;

            return new Top(p.FirstBlood,
                p.FirstTurret,
                p.KDA,
                getKillParticipation(p),
                p.Stats.WardsKilled + p.Stats.WardsPlaced,
                (p.Timeline.CreepsPerMinDeltas.ZeroToTen + p.Timeline.CreepsPerMinDeltas.TenToTwenty) / 2,
                p.Stats.MinionsKilled - enemyCs,
                p.Stats.TotalDamageDealtToChampions,
                p.Stats.Assists,
                p.Stats.Deaths);
        }

        private Mid createMidData(ParticipantAPI p)
        {
            long enemyCs = Participants.Find(enemy => enemy.Timeline.Lane == p.Timeline.Lane &&
                enemy.TeamId != p.TeamId).Stats.MinionsKilled;

            return new Mid(p.FirstBlood,
                p.FirstTurret,
                p.KDA,
                getKillParticipation(p),
                p.Stats.WardsKilled + p.Stats.WardsPlaced,
                (p.Timeline.CreepsPerMinDeltas.ZeroToTen + p.Timeline.CreepsPerMinDeltas.TenToTwenty) / 2,
                p.Stats.MinionsKilled - enemyCs,
                p.Stats.TotalDamageDealtToChampions,
                p.Stats.NeutralMinionsKilledEnemyJungle);
        }

        private Jungle createJungleData(ParticipantAPI p)
        {
            return new Jungle(p.FirstBlood,
                p.FirstTurret,
                p.KDA,
                getKillParticipation(p),
                p.Stats.NeutralMinionsKilledTeamJungle,
                p.Stats.NeutralMinionsKilledEnemyJungle,
                p.Stats.WardsKilled + p.Stats.WardsPlaced);
        }
    }  
}
