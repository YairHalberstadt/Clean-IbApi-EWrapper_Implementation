using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public enum Duration
    {
        Null=0,
        TenYear=5256000,
        FiveYear=2628000,
        TwoYear=1051200,
        OneYear=525600,
        SixMonths= 262800,
        ThreeMonths= 131400,
        TwoMonths= 87600,
        OneMonth = 43800,
        FourWeeks= 40320,
        ThreeWeeks= 30240,
        TwoWeeks= 20160,
        TenDays=14400,
        OneWeek= 10080,
        FiveDays=7700,
        TwoDays=2880,
        OneDay=1440,
        TwelveHours=720,
        SixHours=360,
        ThreeHours=180,
        TwoHours=120,
        OneHour=60,
        ThirtyMinutes=30,
        FifteenMinutes=15,
        TenMinutes=10,
        FiveMinutes=5,
        TwoMinutes=2,
        OneMinute=1
    }

    public static class DurationExtensionMethods
    {
        public static TimeSpan ToTimeSpan(this Duration duration)
        {
            return TimeSpan.FromMinutes((int)duration);
        }
        public static string ToDurationStringForHistoricalDataRequest(this Duration duration, CandleSize candleSize)
        {
            if (candleSize == CandleSize.Month)
            {
                switch (duration)
                {
                    case Duration.TenYear:
                        return "10 Y";
                    case Duration.FiveYear:
                        return "5 Y";
                    case Duration.TwoYear:
                        return "2 Y";
                    case Duration.OneYear:
                        return "1 Y";
                    case Duration.SixMonths:
                        return "6 M";
                    case Duration.ThreeMonths:
                        return "3 M";
                    case Duration.TwoMonths:
                        return "2 M";
                    case Duration.OneMonth:
                    default:
                        return "1 M";
                }
            }
            if (candleSize == CandleSize.Week)
            {
                switch (duration)
                {
                    case Duration.TenYear:
                        return "10 Y";
                    case Duration.FiveYear:
                        return "5 Y";
                    case Duration.TwoYear:
                        return "2 Y";
                    case Duration.OneYear:
                        return "1 Y";
                    case Duration.SixMonths:
                        return "6 M";
                    case Duration.ThreeMonths:
                        return "3 M";
                    case Duration.TwoMonths:
                        return "2 M";
                    case Duration.OneMonth:
                        return "1 M";
                    case Duration.FourWeeks:
                        return "4 W";
                    case Duration.ThreeWeeks:
                        return "3 W";
                    case Duration.TwoWeeks:
                    case Duration.TenDays:
                        return "2 W";
                    case Duration.OneWeek:
                    default:
                        return "1 W";
                }
            }
            if (candleSize == CandleSize.Day)
            {
                switch (duration)
                {
                    case Duration.TenYear:
                        return "10 Y";
                    case Duration.FiveYear:
                        return "5 Y";
                    case Duration.TwoYear:
                        return "2 Y";
                    case Duration.OneYear:
                        return "1 Y";
                    case Duration.SixMonths:
                        return "6 M";
                    case Duration.ThreeMonths:
                        return "3 M";
                    case Duration.TwoMonths:
                        return "2 M";
                    case Duration.OneMonth:
                        return "1 M";
                    case Duration.FourWeeks:
                        return "4 W";
                    case Duration.ThreeWeeks:
                        return "3 W";
                    case Duration.TwoWeeks:
                        return "2 W";
                    case Duration.TenDays:
                        return "10 D";
                    case Duration.OneWeek:
                        return "1 W";
                    case Duration.FiveDays:
                        return "5 D";
                    case Duration.TwoDays:
                        return "2 D";
                    case Duration.OneDay:
                    default:
                        return "1 D";
                }
            }

            if (candleSize == CandleSize.FourHours)
            {
                switch (duration)
                {
                    case Duration.TenYear:
                    case Duration.FiveYear:
                    case Duration.TwoYear:
                    case Duration.OneYear:
                    case Duration.SixMonths:
                    case Duration.ThreeMonths:
                    case Duration.TwoMonths:
                    case Duration.OneMonth:
                        return "1 M";
                    case Duration.FourWeeks:
                        return "4 W";
                    case Duration.ThreeWeeks:
                        return "3 W";
                    case Duration.TwoWeeks:
                        return "2 W";
                    case Duration.TenDays:
                        return "10 D";
                    case Duration.OneWeek:
                        return "1 W";
                    case Duration.FiveDays:
                        return "5 D";
                    case Duration.TwoDays:
                        return "2 D";
                    case Duration.OneDay:
                        return "1 D";
                    case Duration.TwelveHours:
                        return "43200 S";
                    default:
                        return "28800 S";
                }
            }
            if (candleSize == CandleSize.Hour)
            {
                switch (duration)
                {
                    case Duration.TenYear:
                    case Duration.FiveYear:
                    case Duration.TwoYear:
                    case Duration.OneYear:
                    case Duration.SixMonths:
                    case Duration.ThreeMonths:
                    case Duration.TwoMonths:
                    case Duration.OneMonth:
                        return "1 M";
                    case Duration.FourWeeks:
                        return "4 W";
                    case Duration.ThreeWeeks:
                        return "3 W";
                    case Duration.TwoWeeks:
                        return "2 W";
                    case Duration.TenDays:
                        return "10 D";
                    case Duration.OneWeek:
                        return "1 W";
                    case Duration.FiveDays:
                        return "5 D";
                    case Duration.TwoDays:
                        return "2 D";
                    case Duration.OneDay:
                        return "1 D";
                    case Duration.TwelveHours:
                        return "43200 S";
                    case Duration.SixHours:
                        return "21600 S";
                    case Duration.ThreeHours:
                        return "10800 S";
                    case Duration.TwoHours:
                        return "7200 S";
                    case Duration.OneHour:
                    default:
                        return "3600 S";
                }
            }
            if (candleSize == CandleSize.FifteenMinutes)
            {
                switch (duration)
                {
                    case Duration.TenYear:
                    case Duration.FiveYear:
                    case Duration.TwoYear:
                    case Duration.OneYear:
                    case Duration.SixMonths:
                    case Duration.ThreeMonths:
                    case Duration.TwoMonths:
                    case Duration.OneMonth:
                    case Duration.FourWeeks:
                        return "4 W";
                    case Duration.ThreeWeeks:
                        return "3 W";
                    case Duration.TwoWeeks:
                        return "2 W";
                    case Duration.TenDays:
                        return "10 D";
                    case Duration.OneWeek:
                        return "1 W";
                    case Duration.FiveDays:
                        return "5 D";
                    case Duration.TwoDays:
                        return "2 D";
                    case Duration.OneDay:
                        return "1 D";
                    case Duration.TwelveHours:
                        return "43200 S";
                    case Duration.SixHours:
                        return "21600 S";
                    case Duration.ThreeHours:
                        return "10800 S";
                    case Duration.TwoHours:
                        return "7200 S";
                    case Duration.OneHour:
                        return "3600 S";
                    case Duration.ThirtyMinutes:
                        return "1800 S";
                    default:
                        return "900 S";
                }
            }
            if (candleSize == CandleSize.FiveMinutes)
            {
                switch (duration)
                {
                    case Duration.TenYear:
                    case Duration.FiveYear:
                    case Duration.TwoYear:
                    case Duration.OneYear:
                    case Duration.SixMonths:
                    case Duration.ThreeMonths:
                    case Duration.TwoMonths:
                    case Duration.OneMonth:
                    case Duration.FourWeeks:
                        return "4 W";
                    case Duration.ThreeWeeks:
                        return "3 W";
                    case Duration.TwoWeeks:
                        return "2 W";
                    case Duration.TenDays:
                        return "10 D";
                    case Duration.OneWeek:
                        return "1 W";
                    case Duration.FiveDays:
                        return "5 D";
                    case Duration.TwoDays:
                        return "2 D";
                    case Duration.OneDay:
                        return "1 D";
                    case Duration.TwelveHours:
                        return "43200 S";
                    case Duration.SixHours:
                        return "21600 S";
                    case Duration.ThreeHours:
                        return "10800 S";
                    case Duration.TwoHours:
                        return "7200 S";
                    case Duration.OneHour:
                        return "3600 S";
                    case Duration.ThirtyMinutes:
                        return "1800 S";
                    case Duration.FifteenMinutes:
                        return "900 S";
                    case Duration.TenMinutes:
                        return "600 S";
                    default:
                        return "300 S";
                }
            }
            if (candleSize == CandleSize.Minute)
            {
                switch (duration)
                {
                    case Duration.TenYear:
                    case Duration.FiveYear:
                    case Duration.TwoYear:
                    case Duration.OneYear:
                    case Duration.SixMonths:
                    case Duration.ThreeMonths:
                    case Duration.TwoMonths:
                    case Duration.OneMonth:
                    case Duration.FourWeeks:
                    case Duration.ThreeWeeks:
                    case Duration.TwoWeeks:
                    case Duration.TenDays:
                    case Duration.OneWeek:
                    case Duration.FiveDays:
                    case Duration.TwoDays:
                    case Duration.OneDay:
                        return "1 D";
                    case Duration.TwelveHours:
                        return "43200 S";
                    case Duration.SixHours:
                        return "21600 S";
                    case Duration.ThreeHours:
                        return "10800 S";
                    case Duration.TwoHours:
                        return "7200 S";
                    case Duration.OneHour:
                        return "3600 S";
                    case Duration.ThirtyMinutes:
                        return "1800 S";
                    case Duration.FifteenMinutes:
                        return "900 S";
                    case Duration.TenMinutes:
                        return "600 S";
                    case Duration.FiveMinutes:
                        return "300 S";
                    case Duration.TwoMinutes:
                        return "120 S";
                    default:
                        return "60 S";
                }
            }
            if (candleSize == CandleSize.FifteenSeconds)
            {
                switch (duration)
                {
                    case Duration.TenYear:
                    case Duration.FiveYear:
                    case Duration.TwoYear:
                    case Duration.OneYear:
                    case Duration.SixMonths:
                    case Duration.ThreeMonths:
                    case Duration.TwoMonths:
                    case Duration.OneMonth:
                    case Duration.FourWeeks:
                    case Duration.ThreeWeeks:
                    case Duration.TwoWeeks:
                    case Duration.TenDays:
                    case Duration.OneWeek:
                    case Duration.FiveDays:
                    case Duration.TwoDays:
                    case Duration.OneDay:
                    case Duration.TwelveHours:
                    case Duration.SixHours:
                        return "14400 S";
                    case Duration.ThreeHours:
                        return "10800 S";
                    case Duration.TwoHours:
                        return "7200 S";
                    case Duration.OneHour:
                        return "3600 S";
                    case Duration.ThirtyMinutes:
                        return "1800 S";
                    case Duration.FifteenMinutes:
                        return "900 S";
                    case Duration.TenMinutes:
                        return "600 S";
                    case Duration.FiveMinutes:
                        return "300 S";
                    case Duration.TwoMinutes:
                        return "120 S";
                    default:
                        return "60 S";
                }
            }
            if (candleSize == CandleSize.FiveSeconds)
            {
                switch (duration)
                {
                    case Duration.TenYear:
                    case Duration.FiveYear:
                    case Duration.TwoYear:
                    case Duration.OneYear:
                    case Duration.SixMonths:
                    case Duration.ThreeMonths:
                    case Duration.TwoMonths:
                    case Duration.OneMonth:
                    case Duration.FourWeeks:
                    case Duration.ThreeWeeks:
                    case Duration.TwoWeeks:
                    case Duration.TenDays:
                    case Duration.OneWeek:
                    case Duration.FiveDays:
                    case Duration.TwoDays:
                    case Duration.OneDay:
                    case Duration.TwelveHours:
                    case Duration.SixHours:
                    case Duration.ThreeHours:
                    case Duration.TwoHours:
                    case Duration.OneHour:
                        return "3600 S";
                    case Duration.ThirtyMinutes:
                        return "1800 S";
                    case Duration.FifteenMinutes:
                        return "900 S";
                    case Duration.TenMinutes:
                        return "600 S";
                    case Duration.FiveMinutes:
                        return "300 S";
                    case Duration.TwoMinutes:
                        return "120 S";
                    default:
                        return "60 S";
                }
            }
            throw new NotImplementedException("Please Check TWS API Reference Guide for the relevant string");
        }
    }
}
