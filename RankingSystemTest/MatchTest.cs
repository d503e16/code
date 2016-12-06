using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rankingsystem.Classes;
using System.IO;

namespace RankingSystemTest
{
    [TestClass]
    public class MatchTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Database db = new Database("testdb");

            Assert.IsTrue(2 == 2);
        }
    }
}
