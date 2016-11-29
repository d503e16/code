using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;

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

        // A method for creating tables in the databasefile
        private void createTables()
        {
            var rankTableSql =
                "CREATE TABLE IF NOT EXISTS rankTable (id INTEGER PRIMARY KEY, username VARCHAR(20), points INTEGER)";
            var matchTableSql =
                "CREATE TABLE IF NOT EXISTS matchTable (matchId INTEGER PRIMARY KEY, match VARCHAR(1000000))";

            using (var c = new SQLiteConnection(connString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(rankTableSql, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            using (var c = new SQLiteConnection(connString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(matchTableSql, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
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

        public void UpdateSummoner(Summoner s)
        {
            string sql = "INSERT OR REPLACE INTO rankTable (id, username, points) VALUES" +
                "(" + s.PlayerId + ",\"" + s.UserName + "\"," + s.RankingPoints + ")";

            using (var c = new SQLiteConnection(connString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
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
    }
}