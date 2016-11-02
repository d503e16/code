using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designhelper
{
    class GameDto
    {
        public long GameId { get; set; }
        public string SubType { get; set; }
        public List<PlayerDto> FellowPlayers { get; set; }
        public long TotalDamageDealtToBuildings { get; set; }
    }
}
