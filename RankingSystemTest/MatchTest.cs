using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rankingsystem.Classes;
using System.IO;

namespace RankingSystemTest
{
    [TestClass]
    public class MatchTest
    {
        Database db = new Database("testdb");

        [TestMethod]
        public void UpdateRanks()
        {
            var match = db.GetMatch(1);
            
            match.UpdateRanks(db);
            var rankOfPlayerAfterUpdate = db.GetSummonerRank(31803573); 

            Assert.IsTrue(90 == rankOfPlayerAfterUpdate);
        }

        [TestInitialize]
        public void Initialize()
        {
            db.ResetAndLoadTestTables();
        }
    }
}
