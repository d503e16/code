using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rankingsystem.Classes;
using Newtonsoft.Json;
using System.Net.Http;
using Rankingsystem.Classes.Roles;
using System.Data.SQLite;

namespace Rankingsystem
{

    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();
            db.InitDatabase();

            string json = "";
            string sql = "SELECT * FROM matchTable WHERE matchId = 2776285553";
            db.dbConnection.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, db.dbConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                json = (string)reader["match"];
            }
            
            db.dbConnection.Close();
            MatchAPI m = JsonConvert.DeserializeObject<MatchAPI>(json);
            Console.WriteLine("");
            Console.ReadKey();
        }
    }
}
