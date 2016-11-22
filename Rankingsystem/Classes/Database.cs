using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;

namespace Rankingsystem.Classes
{
    public class Database
    {
        public SQLiteConnection dbConnection;

        // A method for opening the conection to the database file
        public void InitDatabase()
        {
            string file =
                Directory.GetParent(
                        Directory.GetParent(
                            Directory.GetParent(
                            Environment.CurrentDirectory).ToString()
                        ).ToString()
                    ) + "\\LolRank.sqlite";
            if (!File.Exists(file))
                SQLiteConnection.CreateFile("LolRank" + ".sqlite");

            dbConnection = new SQLiteConnection("Data Source=" + file + ";Version=3;");
            dbConnection.Open();

            CreateTables();

            dbConnection.Close();
        }

        // A method for creating tables in the databasefile
        private void CreateTables()
        {
            string rankTableSql = "CREATE TABLE IF NOT EXISTS rankTable (id INTEGER PRIMARY KEY, username VARCHAR(20), points INTEGER)";
            string matchTableSql = "CREATE TABLE IF NOT EXISTS matchTable (matchId INTEGER PRIMARY KEY, match VARCHAR(1000000))";

            SQLiteCommand cmdRankTable = new SQLiteCommand(rankTableSql, dbConnection);
            SQLiteCommand cmdMatchTable = new SQLiteCommand(matchTableSql, dbConnection);

            cmdRankTable.ExecuteNonQuery();
            cmdMatchTable.ExecuteNonQuery();
        }

        public Summoner GetSummoner(long id)
        {
            string sql = "SELECT * FROM rankTable WHERE id = " + id;

            dbConnection.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // reader[0] = id of player, reader[1] = name of player, reader[2] = rankingpoints
                var s = new Summoner(id, (string)reader[1], (long)reader[2]);
                dbConnection.Close();
                return s;      
            }
            dbConnection.Close();
            return null;
        }

        public void UpdateSummoner(Summoner s)
        {
            string sql = "INSERT OR REPLACE INTO rankTable (id, username, points) VALUES" +
                "(" + s.PlayerId + ",\"" + s.UserName + "\"," + s.RankingPoints + ")";
            dbConnection.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.ExecuteNonQuery();
            dbConnection.Close();
        }
    }
}