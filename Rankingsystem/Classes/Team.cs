
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes
{
    public class Team
    {
        private List<Participant> participants;
        private bool winner;

        public bool Winner
        {
            get { return winner; }
            set { winner = value; }
        }

        public List<Participant> Participants
        {   
            get { return participants; }
            set { participants = value; }
        }
    }
}
