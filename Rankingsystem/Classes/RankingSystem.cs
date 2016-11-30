using System;
using System.Collections.Generic;
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Title = "Ranking System";
            int selectedIndex = 0;

            do
            {
                Console.Clear();
                printOptions(selectedIndex);

                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.DownArrow)
                {
                    switch (selectedIndex)
                    {
                        case 0:
                            selectedIndex += 1;
                            break;
                        case 1:
                            selectedIndex = 0;
                            break;
                    }
                }
                else if (input.Key == ConsoleKey.UpArrow)
                {
                    switch (selectedIndex)
                    {
                        case 0:
                            selectedIndex = 1;
                            break;
                        case 1:
                            selectedIndex -= 1;
                            break;
                    }
                }

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
            updateRank(id);

            waitUntilKeyIsPressed();
        }

        private void printDisplayOption()
        {
            Console.Clear();
            Console.Write("Enter summoner id: ");
            var id = Console.ReadLine();
            displayRank(id);
            
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

        private void displayRank(string summonerId)
        {
            try
            {
                Console.WriteLine("player with id: " + summonerId + " has rank: " +
                    db.GetSummonerRank(Convert.ToInt64(summonerId)) + "\n");
            }
            catch (Exception e)
            {
                if (e is RowNotInTableException)
                {
                    Console.WriteLine("Summoner with id " + summonerId + " does not exist!\n");
                }
                else if (e is FormatException)
                    Console.WriteLine("Id must be a number!\n");
                else
                    Console.WriteLine(e.Message);
            }
        }

        private void updateRank(string matchId)
        {
            try
            {
                var match = db.GetMatch(Convert.ToInt64(matchId));
                match.UpdateRanks(db);

                Console.WriteLine("Ranks are now updated!\n");

                Console.WriteLine("Display individual performances (y/n)?: ");
                
                while (true)
                {
                    var input = Console.ReadKey(true);
                    if (input.Key == ConsoleKey.Y)
                    {
                        printIndividualPerformances(match.Team1.Participants);
                        printIndividualPerformances(match.Team2.Participants);
                        break;
                    }
                    else if (input.Key == ConsoleKey.N)
                        break;
                    else if (input.Key == ConsoleKey.Escape)
                        Environment.Exit(0);
                }
            }
            catch (Exception e)
            {
                if (e is NullReferenceException)
                    Console.WriteLine("Match with id " + matchId + " does not exist!\n");
                else if (e is FormatException)
                    Console.WriteLine("Id must be a number!\n");
                else
                    Console.WriteLine(e.Message);
            }
        }

        private void printIndividualPerformances(List<Participant> participants)
        {
            foreach (Participant p in participants)
            {
                Console.WriteLine(p.ToString());
            }
        }

        private void waitUntilKeyIsPressed()
        {
            int timeBetweenDots = 250;
            while (Console.KeyAvailable == false)
            {
                Console.Write("Press any key to continue");
                for (int i = 0; i <= 3; i++)
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
