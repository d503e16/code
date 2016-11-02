using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designhelper
{
    class Participant
    {
        public Stats Stats { get; set; }
        public int TeamId { get; set; }
        public ParticipantTimeline TimeLine { get; set; }
    }
}
