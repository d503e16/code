using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Rankingsystem.Classes.Roles;

namespace Rankingsystem.Classes
{
    public class Database
    {
        private string connString { get; set; }
        private string fileName;

        public Database(string fileName)
        {
            this.fileName = fileName;
            var file =
                Directory.GetParent(
                        Directory.GetParent(
                            Directory.GetParent(
                            Environment.CurrentDirectory).ToString()
                        ).ToString()
                    ) + "\\" + fileName + ".sqlite";
            if (!File.Exists(file))
                SQLiteConnection.CreateFile(file);

            connString = "Data Source=" + file + ";Version=3;";

            createTables();
        }

        public void Write(string sql)
        {
            using (var c = new SQLiteConnection(connString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // A method for creating tables in the databasefile
        private void createTables()
        {
            var rankTableSql =
                "CREATE TABLE IF NOT EXISTS rankTable (id INTEGER PRIMARY KEY, username VARCHAR(20), points INTEGER, matchIds VARCHAR(1000000))";
            var matchTableSql =
                "CREATE TABLE IF NOT EXISTS matchTable (matchId INTEGER PRIMARY KEY, match VARCHAR(1000000))";
            var testMatchTableSql =
                "CREATE TABLE IF NOT EXISTS matchTable (matchId INTEGER PRIMARY KEY, match VARCHAR(1000000))";

            Write(rankTableSql);
            Write(matchTableSql);
            Write(testMatchTableSql);
        }

        public long GetSummonerRank(long id)
        {
            string sql = "SELECT * FROM rankTable WHERE id = " + id;

            using (SQLiteConnection c = new SQLiteConnection(connString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // reader[2] = ranking points
                            var points = (long)reader[2];
                            return points;
                        }
                    }
                }
            }
            
            throw new RowNotInTableException();
        }

        public Match GetMatch(long matchId)
        {
            string sql = "SELECT * FROM matchTable WHERE matchId = " + matchId;

            using (SQLiteConnection c = new SQLiteConnection(connString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // reader[1] = match data
                            var m = JsonConvert.DeserializeObject<MatchAPI>((string)reader[1]);
                            return m.CreateMatch(fileName);
                        }
                    }
                }
            }

            return null;
        }

        public List<long> GetMatchIds(long summonerId)
        {
            string sql = "SELECT * FROM rankTable WHERE id = " + summonerId;

            using (SQLiteConnection c = new SQLiteConnection(connString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // reader[3] = match ids
                            return JsonConvert.DeserializeObject<List<long>>((string)reader[3]);
                        }
                    }
                }
            }
            return new List<long>();
        }

        public void UpdateSummoner(Summoner s)
        {
            string sql = "INSERT OR REPLACE INTO rankTable (id, username, points, matchIds) VALUES" +
                "(" + s.PlayerId + ",\"" + s.UserName + "\"," + s.RankingPoints + ",\"" + JsonConvert.SerializeObject(s.MatchIds) + "\")";

            Write(sql);
        }
        
        public List<Match> GetAllMatches(string table)
        {
            string sql = "SELECT * FROM " + table;
            List<Match> matches = new List<Match>();
            using (SQLiteConnection c = new SQLiteConnection(connString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        int i = 1;
                        while (reader.Read())
                        {
                            var m = JsonConvert.DeserializeObject<MatchAPI>((string)reader[1]);
                            try
                            {
                                matches.Add(m.CreateMatch(fileName));
                                Console.WriteLine("Match added " + i);                                
                                i++;
                            }
                            catch (NullReferenceException)
                            {
                                string deletesql = "DELETE FROM " + table + " WHERE matchId =" + m.MatchId;
                                using (SQLiteCommand delete = new SQLiteCommand(deletesql, c))
                                {
                                    delete.ExecuteNonQuery();
                                }
                                continue;
                            }
                        }
                    }
                    deleteInvalidMatches(matches, c, table);
                }
            }
            Console.WriteLine(matches.Count);
            return matches;
        }
        
        private List<Match> deleteInvalidMatches(List<Match> matches, SQLiteConnection c, string table)
        {
            List<Match> list = new List<Match>();
            foreach (Match m in matches)
            {
                try
                {
                    Top top = (Top)m.Team1.Participants.Find(p => p.Role is Top).Role;
                    Jungle jungle = (Jungle)m.Team1.Participants.Find(p => p.Role is Jungle).Role;
                    Mid mid = (Mid)m.Team1.Participants.Find(p => p.Role is Mid).Role;
                    Bot bot = (Bot)m.Team1.Participants.Find(p => p.Role is Bot).Role;
                    Support support = (Support)m.Team1.Participants.Find(p => p.Role is Support).Role;

                    top = (Top)m.Team2.Participants.Find(p => p.Role is Top).Role;
                    jungle = (Jungle)m.Team2.Participants.Find(p => p.Role is Jungle).Role;
                    mid = (Mid)m.Team2.Participants.Find(p => p.Role is Mid).Role;
                    bot = (Bot)m.Team2.Participants.Find(p => p.Role is Bot).Role;
                    support = (Support)m.Team2.Participants.Find(p => p.Role is Support).Role;
                    list.Add(m);
                }
                    catch (NullReferenceException)
                    {
                        
                        string deletesql = "DELETE FROM " + table + " WHERE matchId =" + m.MatchId;
                        using (SQLiteCommand delete = new SQLiteCommand(deletesql, c))
                        {
                            delete.ExecuteNonQuery();
                        }
                        continue;
                    }
                }
            return list;
        }
        
        public bool SummonerExists(long id)
        {
            string sql = "SELECT * FROM rankTable WHERE id = " + id;

            using (SQLiteConnection c = new SQLiteConnection(connString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void ResetAndLoadTestTables()
        {
            Write("DELETE FROM rankTable");
            Write("DELETE FROM matchTable");

            var rankTableCsv =
                Directory.GetParent(
                        Directory.GetParent(
                            Directory.GetParent(
                            Environment.CurrentDirectory).ToString()
                        ).ToString()
                    ) + "\\rankTable.csv";


            var matchTableCsv =
                Directory.GetParent(
                        Directory.GetParent(
                            Directory.GetParent(
                            Environment.CurrentDirectory).ToString()
                        ).ToString()
                    ) + "\\matchTable.csv";

            var readerMatchTable = new StreamReader(matchTableCsv);
            var readerRankTable = new StreamReader(rankTableCsv);

            while (!readerMatchTable.EndOfStream)
            {
                var line = readerMatchTable.ReadLine();
                var values = line.Split('?');

                long mId = Convert.ToInt64(values[0]);
                string matchInfo = values[1];

                Write("INSERT INTO matchTable VALUES" +
                    "(" + mId + ",\'" + matchInfo + "\')");
            }

            while (!readerRankTable.EndOfStream)
            {
                var line = readerRankTable.ReadLine();
                var values = line.Split('?');

                long summonerId = Convert.ToInt64(values[0]);
                string userName = values[1];
                long points = Convert.ToInt64(values[2]);
                string matchIds = values[3];

                Write("INSERT INTO rankTable VALUES" +
                    "(" + summonerId + ",\'" + userName + "\'," + points + ",\'" + matchIds + "\')");
            }
        }
    }
}