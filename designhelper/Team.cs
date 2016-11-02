using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designhelper
{
    class Team
    {
        public int TeamId { get; set; }
        public bool Winner { get; set; }
        public bool FirstTower { get; set; }
        public bool FirstBlood { get; set; }
        public int BaronKills { get; set; }
        public int DragonKills { get; set; }
    }
}
