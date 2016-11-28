using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    public class RankingSystem
    {
        private Database db = new Database();
        public RankingSystem()
        {
            db.InitDatabase();
        }
        public void Start()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("1. Display rank of player");
                Console.WriteLine("2. Update ranks");
                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.D1 ||
                    input.Key == ConsoleKey.NumPad1)
                {
                    Console.Clear();
                    Console.Write("Enter summoner id: ");
                    var id = Console.ReadLine();
                    Console.WriteLine(DisplayRank(id));
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
                else if (input.Key == ConsoleKey.D2 ||
                         input.Key == ConsoleKey.NumPad2)
                {
                    Console.Clear();
                    Console.Write("Enter match id: ");
                    var id = Console.ReadLine();
                    Console.WriteLine(UpdateRank(id));
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
                else if (input.Key == ConsoleKey.Escape) break;
            } while (true);
        }

        private string DisplayRank(string summonerId)
        {
            try
            {
                return "player with id: " + summonerId + " has rank: " +
                    db.GetSummonerRank(Convert.ToInt64(summonerId)).ToString();
            }
            catch (Exception e)
            {
                if (e is NullReferenceException)
                    return "Summoner with id: " + summonerId + " does not exist!";
                else if (e is FormatException)
                    return "Id must be a number!";
                return "";
            }
        }

        private string UpdateRank(string matchId)
        {
            try
            {
                var match = db.GetMatch(Convert.ToInt64(matchId));
                match.UpdateRanks(db);
                return "Ranks are now updated!";
            }
            catch (Exception e)
            {
                if (e is NullReferenceException)
                    return "Match with id " + matchId + " does not exist!";
                else if (e is FormatException)
                    return "Id must be a number!";
                return e.Message;
            }
        }
    }
}
