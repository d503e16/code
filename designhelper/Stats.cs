using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designhelper
{
    class Stats
    {
        public long Assists { get; set; }
        public long Kills { get; set; }
        public long Deaths { get; set; }
        public long MinionsKilled { get; set; }
        public long NeutralMinionsKilled { get; set; }
        public long WardsKilled { get; set; }
        public long WardsPlaced { get; set; }
        public long TotalDamageDealtToChampions { get; set; }
        public long NeutralMinionsKilledEnemyJungle { get; set; }
    }
}
