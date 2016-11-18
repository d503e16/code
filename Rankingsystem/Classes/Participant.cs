using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    class Participant
    {
        private int teamId;
        private Role role;

        public Role Roleprop
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