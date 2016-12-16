using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rankingsystem.Classes
{
    public abstract class Role
    {
        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
        public Role(bool fb, bool ft, double kda, long kp)
        {
            this.firstBlood = fb;
            this.firstTurret = ft;
            this._KDA = kda;
            this._KP = kp;
        }

        //Generelle features
        private bool firstBlood;
        private bool firstTurret;
        private double _KDA;
        private long _KP;

        public long KP
        {
            get { return _KP; }
            set { _KP = value; }
        }

        public double KDA
        {
            get { return _KDA; }
            set { _KDA = value; }
        }

        public bool FirstTurret
        {
            get { return firstTurret; }
            set { firstTurret = value; }
        }

        public bool FirstBlood
        {
            get { return firstBlood; }
            set { firstBlood = value; }
        }

        public virtual List<string> GetData()
        {
            List<string> list = new List<string>();
            list.Add(convertBool(FirstBlood).ToString(CultureInfo.InvariantCulture));
            list.Add(convertBool(FirstTurret).ToString(CultureInfo.InvariantCulture));
            list.Add(KP.ToString(CultureInfo.InvariantCulture));
            list.Add(KDA.ToString(CultureInfo.InvariantCulture));
            return list;
        }

        public virtual List<string> GetNormalizedData(int fbMax, int ftMax, int KPMax, double KDAMax)
        {
            List<string> list = new List<string>();
            list.Add((convertBool(FirstBlood) / fbMax).ToString(CultureInfo.InvariantCulture));
            list.Add((convertBool(firstTurret) / ftMax).ToString(CultureInfo.InvariantCulture));
            list.Add((KP / KPMax).ToString(CultureInfo.InvariantCulture));
            list.Add((KDA / KDAMax).ToString(CultureInfo.InvariantCulture));
            return list;
        }

        public double convertBool(bool b)
        {
            if (b == true) return 1.0;
            else return 0.0;
        }

        public int KDAScore {
            get
            {
                if (_KDA >= 0.0 && 1.0 >= _KDA) return 1;
                else if (_KDA > 1.0 && 2.0 >= _KDA) return 2;
                else if (_KDA > 3.0 && 4.0 >= _KDA) return 3;
                else if (_KDA > 4.0 && 5.0 >= _KDA) return 4;
                else return 5;
            }
        }

        public int KPScore
        {
            get
            {
                if (_KP >= 0 && 20 >= _KP) return 1;
                else if (_KP > 20 && 40 >= _KP) return 2;
                else if (_KP > 40 && 60 >= _KP) return 3;
                else if (_KP > 60 && 80 >= _KP) return 4;
                else return 5;
            }
        }

        public int FirstBloodScore {
            get
            {
                return firstBlood == true ? 5 : 1;
            }
        }

        public int FirstTurretScore
        {
            get
            {
                return firstTurret == true ? 5 : 1;
            }
        }

        public virtual long IndividualPerformance()
        {
            return Convert.ToInt64((KDAScore + KPScore + FirstBloodScore + FirstTurretScore + 0.00001) / 4);
        }
        
        public override string ToString()
        {
            return  
                "KDA: " + KDAScore + "\n" +
                "Kill participation: " + KPScore + "\n" +
                "First Blood: " + FirstBloodScore + "\n" +
                "First Turret: " + FirstTurretScore + "\n";
        }
    }
}