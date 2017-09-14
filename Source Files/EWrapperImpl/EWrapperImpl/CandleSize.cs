using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public enum CandleSize : long
    {
        FiveSeconds = 5000,
        FifteenSeconds = 15000,
        Minute = 60000,
        FiveMinutes = 300000,
        FifteenMinutes = 900000,
        Hour = 3600000,
        FourHours =14400000,
        Day = 86400000,
        Week = 604800000,
        Month = 2592000000,
    }

    public static class CandleSizeExtensionMethods
    {
        public static string IbHistoricalDataBarSizeString(this CandleSize candleSize)
        {
            switch (candleSize)
            {
                case CandleSize.FiveSeconds:
                    return "5 secs";
                case CandleSize.FifteenSeconds:
                    return "15 secs";
                case CandleSize.Minute:
                    return "1 min";
                case CandleSize.FiveMinutes:
                    return "5 mins";
                case CandleSize.FifteenMinutes:
                    return "15 mins";
                case CandleSize.Hour:
                    return "1 hour";
                case CandleSize.FourHours:
                    return "4 hours";
                case CandleSize.Day:
                    return "1 day";
                case CandleSize.Week:
                    return "1 week";
                case CandleSize.Month:
                    return "1 month";
                default:
                    throw (new NotImplementedException("Please Check TWS API Reference Guide for the relevant string"));
            }
        }
        public static TimeSpan ToTimeSpan(this CandleSize candleSize)
        {
            return TimeSpan.FromMilliseconds((long)candleSize);
        }
    }
}
