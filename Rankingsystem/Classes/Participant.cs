using System;

namespace Rankingsystem.Classes
{
    public class Participant : Summoner
    {
        public Participant(long summonerId, string userName, Role role) :
            base(summonerId, userName)
        {
            this.role = role;
        }

        private Role role;

        public Role Role
        {
            get { return role; }
            set { role = value; }
        }

        public override string ToString()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("-----" + UserName + " (" + role.GetType().Name + ")-----");
            Console.ForegroundColor = ConsoleColor.Cyan;

            return role.ToString();
        }
    }
}