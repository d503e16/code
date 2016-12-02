using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Rankingsystem.Classes
{
    public class Database
    {
        private string connString { get; set; }

        // A method for opening the conection to the database file
        public Database()
        {
            var file =
                Directory.GetParent(
                        Directory.GetParent(
                            Directory.GetParent(
                            Environment.CurrentDirectory).ToString()
                        ).ToString()
                    ) + "\\LolRank.sqlite";
            if (!File.Exists(file))
                SQLiteConnection.CreateFile("LolRank" + ".sqlite");

            connString = "Data Source=" + file + ";Version=3;";

            createTables();
        }

        private void write(string sql)
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

            write(rankTableSql);
            write(matchTableSql);
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
                            return m.CreateMatch();
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

            write(sql);
        }
        
        public List<Match> GetAllMatches()
        {
            string sql = "SELECT * FROM matchTable";
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
                                matches.Add(m.CreateMatch());
                                Console.WriteLine("Match added " + i);
                                //Match test = matches[matches.Count - 1];
                                //List<Team> teams = new List<Team>();
                                //teams.Add(test.Team1);
                                //teams.Add(test.Team2);
                                //int features = 0;
                                //foreach (Team t in teams)
                                //{
                                //    for (int j = 0; j < t.Participants.Count ; j++)
                                //    {
                                //        features += t.Participants[j].Role.GetData().Length;
                                //        if (j == 4 && features != 38)
                                //        {
                                //            string deletesql = "DELETE FROM matchTable WHERE matchId =" + test.MatchId;
                                //            using (SQLiteCommand delete = new SQLiteCommand(deletesql, c))
                                //            {
                                //                delete.ExecuteNonQuery();
                                //            }
                                //            features = 0;
                                //        }
                                //        else
                                //        {
                                //            features = 0;
                                //        }
                                //    }
                                   
                                //}
                                
                                i++;
                            }
                            catch (NullReferenceException)
                            {
                                string deletesql = "DELETE FROM matchTable WHERE matchId =" + m.MatchId;
                                using (SQLiteCommand delete = new SQLiteCommand(deletesql, c))
                                {
                                    delete.ExecuteNonQuery();
                                }
                                continue;
                            }
                        }
                    }
                }
            }
            return matches;
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
    }
}