using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}