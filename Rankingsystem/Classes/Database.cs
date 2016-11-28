using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;

namespace Rankingsystem.Classes
{
    public class Database
    {
        public SQLiteConnection DbConnection { get; set; }

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

            DbConnection = new SQLiteConnection("Data Source=" + file + ";Version=3;");
            DbConnection.Open();

            createTables();

            DbConnection.Close();
        }

        // A method for creating tables in the databasefile
        private void createTables()
        {
            var rankTableSql =
                "CREATE TABLE IF NOT EXISTS rankTable (id INTEGER PRIMARY KEY, username VARCHAR(20), points INTEGER)";
            var matchTableSql =
                "CREATE TABLE IF NOT EXISTS matchTable (matchId INTEGER PRIMARY KEY, match VARCHAR(1000000))";

            var cmdRankTable = new SQLiteCommand(rankTableSql, DbConnection);
            var cmdMatchTable = new SQLiteCommand(matchTableSql, DbConnection);

            cmdRankTable.ExecuteNonQuery();
            cmdMatchTable.ExecuteNonQuery();
        }

        public long GetSummonerRank(long id)
        {
            string sql = "SELECT * FROM rankTable WHERE id = " + id;

            DbConnection.Open();
            var cmd = new SQLiteCommand(sql, DbConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // reader[2] = rankingpoints
                var points = (long)reader[2];
                DbConnection.Close();
                return points;      
            }
            DbConnection.Close();
            throw new RowNotInTableException();
        }

        public void UpdateSummoner(Summoner s)
        {
            string sql = "INSERT OR REPLACE INTO rankTable (id, username, points) VALUES" +
                "(" + s.PlayerId + ",\"" + s.UserName + "\"," + s.RankingPoints + ")";
            DbConnection.Open();
            var cmd = new SQLiteCommand(sql, DbConnection);
            cmd.ExecuteNonQuery();
            DbConnection.Close();
        }

        public Match GetMatch(long matchId)
        {
            string sql = "SELECT * FROM matchTable WHERE matchId = " + matchId;

            DbConnection.Open();
            var cmd = new SQLiteCommand(sql, DbConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // reader[1] = match data
                var m = JsonConvert.DeserializeObject<MatchAPI>((string)reader[1]);
                DbConnection.Close();
                return m.CreateMatch();
            }
            DbConnection.Close();
            return null;
        }
    }
}