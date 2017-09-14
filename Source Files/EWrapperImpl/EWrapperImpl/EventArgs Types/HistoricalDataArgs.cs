using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class HistoricalDataArgs :EventArgs
    {
       public HistoricalDataToken Token { get; }
       public Candle Candle { get; }
       public int Volume { get; }
       public int Count { get; }
       public double WAP { get; }
       public bool HasGaps { get; }
       public HistoricalDataArgs(HistoricalDataToken token, CandleSize candleSize, DateTime startDate, double open, double high, double low, double close, int volume, int count, double Wap, bool hasGaps)
        {
            Token = token; ;
            Candle = new Candle(candleSize, open, high, low, close, startDate);
            Volume = volume;
            Count = count;
            WAP = Wap;
            HasGaps = hasGaps;
        }
    }
}