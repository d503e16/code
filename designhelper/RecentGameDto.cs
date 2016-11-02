using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designhelper
{
    class RecentGameDto
    {
        public List<GameDto> Games { get; set; }
        public long SummonerId { get; set; }
    }
}
