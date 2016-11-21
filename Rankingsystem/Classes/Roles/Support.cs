using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankingsystem.Classes.Roles
{
    class Support : Role
    {
        //Support
        private int assists;

        public int Assists
        {
            get { return assists; }
            set { assists = value; }
        }

    }
}