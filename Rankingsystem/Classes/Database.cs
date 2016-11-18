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
                            Directory.GetParent(
                            Environment.CurrentDirectory).ToString()
                        ).ToString()
                    ).ToString()) + "\\LolRank.sqlite";
            if (!File.Exists(file))
                Console.WriteLine("asd");
                SQLiteConnection.CreateFile("LolRank" + ".sqlite");

            m_dbConnection = new SQLiteConnection("Data Source=" + file + ";Version=3;");
            m_dbConnection.Open();

            CreateTables();
        }

        // A method for creating tables in the databasefile
        private void CreateTables()
        {
            Execute("CREATE TABLE IF NOT EXISTS rankTable (id INTEGER PRIMARY KEY, username VARCHAR(20), points INTEGER)");
            Execute("CREATE TABLE IF NOT EXISTS matchTable (matchId INTEGER PRIMARY KEY, match VARCHAR(1000000))");
        }

        // A method for executing SQL string in the database
        public void Execute(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        // A method for retrieving information from the database file and seperating it acording to a string array
        public void Read(string sql, ref List<string> resultData, params string[] elements)
        {
            resultData.Clear();
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string s = "";
                foreach (string el in elements)
                {
                    s += reader[el] + ",";
                }
                resultData.Add(s);
            }
        }
    }
}