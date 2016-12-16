using System;
using System.Collections.Generic;
using System.Globalization;
using Rankingsystem.Classes.Roles;

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

        public double[] NormalizedFeatures()
        {
            List<double> result = new List<double>();
            var t = participants.Find(p => p.Role is Top).Role.GetNormalizedData(1, 1, 100, 28);
            var j = participants.Find(p => p.Role is Jungle).Role.GetNormalizedData(1, 1, 100, 36);
            var m = participants.Find(p => p.Role is Mid).Role.GetNormalizedData(1, 1, 100, 37);
            var b = participants.Find(p => p.Role is Bot).Role.GetNormalizedData(1, 1, 100, 33);
            var s = participants.Find(p => p.Role is Support).Role.GetNormalizedData(1, 1, 100, 35);
            foreach (var data in t)
            {
                result.Add(double.Parse(data, new CultureInfo("en-US")));
            }
            foreach (var data in j)
            {
                result.Add(double.Parse(data, new CultureInfo("en-US")));
            }
            foreach (var data in m)
            {
                result.Add(double.Parse(data, new CultureInfo("en-US")));
            }
            foreach (var data in b)
            {
                result.Add(double.Parse(data, new CultureInfo("en-US")));
            }
            foreach (var data in s)
            {
                result.Add(double.Parse(data, new CultureInfo("en-US")));
            }
            
            return result.ToArray();
        }
    }
}
