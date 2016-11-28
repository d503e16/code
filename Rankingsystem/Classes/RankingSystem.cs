using System;
using System.Data;
using System.Threading;

namespace Rankingsystem.Classes
{
    public class RankingSystem
    {
        private Database db = new Database();

        public void Start()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Title = "Ranking System";
            int selectedIndex = 0;

            do
            {
                Console.Clear();
                printOptions(selectedIndex);

                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.DownArrow &&
                    selectedIndex == 0) selectedIndex = 1;
                else if (input.Key == ConsoleKey.UpArrow &&
                    selectedIndex == 1) selectedIndex = 0;
                else if (input.Key == ConsoleKey.Enter)
                {
                    if (selectedIndex == 0)
                        printDisplayOption();
                    else
                        printUpdateOption();
                }
                else if (input.Key == ConsoleKey.Escape) break;
            } while (true);
        }

        private void printUpdateOption()
        {
            Console.Clear();
            Console.Write("Enter match id: ");
            var id = Console.ReadLine();
            Console.WriteLine(updateRank(id));
            
            waitUntilKeyIsPressed();
        }

        private void printDisplayOption()
        {
            Console.Clear();
            Console.Write("Enter summoner id: ");
            var id = Console.ReadLine();
            Console.WriteLine(displayRank(id));
            
            waitUntilKeyIsPressed();
        }

        private void printOptions(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("Display rank of player");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Update ranks");
            }
            else if (selectedIndex == 1)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Display rank of player");
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("Update ranks");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        private string displayRank(string summonerId)
        {
            try
            {
                return "player with id: " + summonerId + " has rank: " +
                    db.GetSummonerRank(Convert.ToInt64(summonerId));
            }
            catch (Exception e)
            {
                if (e is RowNotInTableException)
                {
                    return "Summoner with id " + summonerId + " does not exist!";
                }
                if (e is FormatException)
                    return "Id must be a number!";
                return e.Message;
            }
        }

        private string updateRank(string matchId)
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
                if (e is FormatException)
                    return "Id must be a number!";
                return e.Message;
            }
        }

        private void waitUntilKeyIsPressed()
        {
            int timeBetweenDots = 100;
            while (Console.KeyAvailable == false)
            {
                Console.Write("Press any key to continue");
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(timeBetweenDots);
                    Console.Write(".");
                }
                Console.WriteLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                clearLine();
            }
            Console.ReadKey();
        }

        private void clearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
