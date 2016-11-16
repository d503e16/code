using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Classes.Database database = new Classes.Database();
            database.InitDatabase();
            Console.ReadKey();
        }
    }
}
