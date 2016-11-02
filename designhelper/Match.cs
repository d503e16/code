using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designhelper
{
    class Match
    {
        public List<Participant> Participants { get; set; }
        public List<Team> Teams { get; set; }
        public long MatchId { get; set; }
        public string QueueType { get; set; }
    }
}
