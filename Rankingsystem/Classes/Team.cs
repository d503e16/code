using System.Collections.Generic;

namespace Rankingsystem.Classes
{
    public class Team
    {
        public Team(List<Participant> plist, bool winner)
        {
            this.participants = plist;
            this.winner = winner;
        }

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
