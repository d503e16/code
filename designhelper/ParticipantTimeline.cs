using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designhelper
{
    class ParticipantTimeline
    {
        public long FrameInterval { get; set; }
        public List<Frame> Frames { get; set; }
        public string Role { get; set; }
        public string Lane { get; set; }
    }
}
