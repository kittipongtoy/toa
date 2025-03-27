using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOAMediaPlayer.Utils
{
    public class NSNumber
    {
        public static double IsZero(double vValue)
        {
            if (vValue > 999999999999999.9 || vValue < -999999999999999.9)
            {
                vValue = 0.0;
            }
            return vValue;
        }

        public static double RoundAmount(double dbAmt, int iNoDecimal)
        {
            double dbRounded = 0.0;
            if (0.0 == dbAmt) return dbRounded;
            double dbMultiply = 1.0;
            if (iNoDecimal > 16) iNoDecimal = 16;
            if (iNoDecimal > 0)
            {
                dbMultiply = Math.Pow(10.0, (double)iNoDecimal);
            }

            dbRounded = dbAmt;
            if (dbMultiply > 0.0) dbRounded *= dbMultiply;
            dbRounded = Math.Round(dbRounded, 0, MidpointRounding.AwayFromZero);
            if (dbRounded != 0.0 && dbMultiply > 0.0) dbRounded /= dbMultiply;

            return dbRounded;
        }
    }
}
