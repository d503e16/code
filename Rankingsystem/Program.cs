using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rankingsystem.Classes;

namespace Rankingsystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Database database = new Database();
            database.InitDatabase();
            // det her er Krusaas del af projektet. Jeg smutter hjem.
            Console.WriteLine("Det her er et hjernedødt ranking system!");
            Console.ReadKey();
        }
    }
}
