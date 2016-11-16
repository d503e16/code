
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    class Team
    {
        private List<Participant> team;

        public List<Participant> Teamprop
        {   
            get { return team; }
            set { team = value; }
        }
    }
}
