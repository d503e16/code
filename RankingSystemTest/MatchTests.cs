using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rankingsystem.Classes;
using Rankingsystem.Classes.Roles;
using System.IO;
using System.Collections.Generic;

namespace RankingSystemTest
{
    [TestClass]
    public class MatchTests
    {
        Database db = new Database("testdb");

        [TestMethod]
        public void UpdateRanks_WinningParticipantWith10Points_NewRankEquals10Plus40PlusPerformance()
        {
            Match match = createTestMatch();

            var testParticipant = match.Team1.Participants[0];
            testParticipant.RankingPoints = 10;
            var performance = testParticipant.Role.IndividualPerformance();

            match.UpdateRanks();

            var rankAfterUpdate = testParticipant.RankingPoints;

            Assert.AreEqual(
                10 + 40 + performance,
                rankAfterUpdate);
        }

        [TestMethod]
        public void UpdateRanks_WinningParticipantWith0Points_NewRankEquals40PlusPerformance()
        {
            Match match = createTestMatch();

            var testParticipant = match.Team1.Participants[0];
            testParticipant.RankingPoints = 0;
            var performance = testParticipant.Role.IndividualPerformance();

            match.UpdateRanks();

            var rankAfterUpdate = testParticipant.RankingPoints;

            Assert.AreEqual(
                40 + performance,
                rankAfterUpdate);
        }

        [TestMethod]
        public void UpdateRanks_LosingParticipantWith100Points_NewRankEquals100Minus50PlusPerformance()
        {
            Match match = createTestMatch();

            var testParticipant = match.Team2.Participants[0];
            testParticipant.RankingPoints = 100;
            var performance = testParticipant.Role.IndividualPerformance();

            match.UpdateRanks();

            var rankAfterUpdate = testParticipant.RankingPoints;

            //-50 for losing a match + the individualPerformance
            Assert.AreEqual(
                100 - 50 + performance,
                rankAfterUpdate);
        }

        [TestMethod]
        public void UpdateRanks_LosingParticipantWith20Points_NewRankEquals0()
        {
            Match match = createTestMatch();

            var testParticipant = match.Team2.Participants[0];
            testParticipant.RankingPoints = 20;
            //-50 for losing a match + the individualPerformance
            var performance = -50 + testParticipant.Role.IndividualPerformance();

            match.UpdateRanks();

            var rankAfterUpdate = testParticipant.RankingPoints;

            // RankingPoints are set to 0 because 20-50+IndividualPerformance < 0 
            Assert.AreEqual(
                0,
                rankAfterUpdate);
        }

        private Match createTestMatch()
        {
            List<Participant> p1 = new List<Participant>();
            List<Participant> p2 = new List<Participant>();

            p1.Add(new Participant(1, "userOne", 
                new Mid(true, true, 3, 80, 20, 25000, 20)));
            p1.Add(new Participant(2, "userTwo",
                new Support(true, false, 8, 40, 20)));

            p2.Add(new Participant(3, "userThree",
                new Mid(false, false, 0.8, 35, -20, 12000, 10)));
            p2.Add(new Participant(4, "userFour",
                new Support(false, false, 2, 45, 10)));

            Team t1 = new Team(p1, true);
            Team t2 = new Team(p2, false);

            return new Match(1, t1, t2, "testdb");
        }
    }
}
