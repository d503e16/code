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
        //Lists of object contained in the Database instance used to retrieve data from the database file
        public string[] playerTableColumns = new string[3] { "id", "username", "points" };
        public string[] matchTableColumns = new string[2] { "matchid", "match" };
        public SQLiteConnection m_dbConnection;
        public List<string> readInfo = new List<string>();

        public Database()
        {
            readInfo = new List<string>();
        }

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
                Console.WriteLine("asd");
                SQLiteConnection.CreateFile("LolRank" + ".sqlite");

            m_dbConnection = new SQLiteConnection("Data Source=" + file + ";Version=3;");
            m_dbConnection.Open();

            CreateTables();

            m_dbConnection.Close();
        }

        // A method for creating tables in the databasefile
        private void CreateTables()
        {
            string rankTableSql = "CREATE TABLE IF NOT EXISTS rankTable (id INTEGER PRIMARY KEY, username VARCHAR(20), points INTEGER)";
            string matchTableSql = "CREATE TABLE IF NOT EXISTS matchTable (matchId INTEGER PRIMARY KEY, match VARCHAR(1000000))";

            SQLiteCommand cmdRankTable = new SQLiteCommand(rankTableSql, m_dbConnection);
            SQLiteCommand cmdMatchTable = new SQLiteCommand(matchTableSql, m_dbConnection);

            cmdRankTable.ExecuteNonQuery();
            cmdMatchTable.ExecuteNonQuery();
        }

        public Summoner GetSummoner(long id)
        {
            Summoner s = new Summoner();
            string sql = "SELECT * FROM rankTable WHERE id = " + id;

            m_dbConnection.Open();
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                s.PlayerId = id;
                s.RankingPoints = (long)reader[2];
                s.UserName = (string)reader[1];
            }

            if (s.PlayerId != 0) return s;
            return null;
        }
    }
}