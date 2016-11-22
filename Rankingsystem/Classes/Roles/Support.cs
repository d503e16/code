using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    public class Support : Role
    {
        //Support
        private long assists;

        public long Assists
        {
            get { return assists; }
            set { assists = value; }
        }

    }
}