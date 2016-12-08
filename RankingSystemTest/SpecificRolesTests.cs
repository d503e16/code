using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rankingsystem.Classes.Roles;

namespace RankingSystemTest
{
    [TestClass]
    public class SpecificRolesTests
    {
        [TestMethod]
        public void GetData_Bot_ResultHas7Entries()
        {
            var b = createTestBot();

            var data = b.GetData();

            Assert.AreEqual(7, data.Count);
        }

        [TestMethod]
        public void IndividualPerformance_BotWithScore8_Result8()
        {
            var b = createTestBot();

            var performance = b.IndividualPerformance();

            Assert.AreEqual(8, performance);
        }

        private Bot createTestBot()
        {   
            // 5, 5, 5, 5, 4, 3, 3
            return new Bot(true, true, 10, 90, 8, 20, 30000);
        } 
    }
}
