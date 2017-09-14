using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCandlesCollector
{
    static class DateTimeExtensionMethods
    {
        public static DateTime RoundUp(this DateTime dateTime, TimeSpan timeSpan)
        {
            return new DateTime(((dateTime.Ticks - 1) / timeSpan.Ticks + 1) * timeSpan.Ticks,dateTime.Kind);
        }
    }
}
