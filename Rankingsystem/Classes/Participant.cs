using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    public class Participant : Summoner
    {
        public Participant(long id, string userName, long points, int teamId, Role role) :
            base(id, userName, points)
        {
            this.teamId = teamId;
            this.role = role;
        }

        private int teamId;
        private Role role;

        public Role Role
        {
            get { return role; }
            set { role = value; }
        }

        public int TeamId
        {
            get { return teamId; }
            set { teamId = value; }
        }

    }
}