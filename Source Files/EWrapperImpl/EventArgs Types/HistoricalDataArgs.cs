using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class HistoricalDataArgs :EventArgs
    {
       public HistoricalDataToken Token { get; }
       public string Date { get; }
       public double Open { get; }
       public double High { get; }
       public double Low { get; }
       public double Close { get; }
       public int Volume { get; }
       public int Count { get; }
       public double WAP { get; }
       public bool HasGaps { get; }
       public HistoricalDataArgs(int reqId, string date, double open, double high, double low, double close, int volume, int count, double Wap, bool hasGaps)
        {
            Token = new HistoricalDataToken(reqId);
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            Count = count;
            WAP = Wap;
            HasGaps = hasGaps;
        }
    }
}