using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    public class Participant : Summoner
    {
        public Participant(long id, string userName, long points, Role role) :
            base(id, userName, points)
        {
            this.role = role;
        }

        public Participant(long id, Role role)
        {
            base.PlayerId = id;
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