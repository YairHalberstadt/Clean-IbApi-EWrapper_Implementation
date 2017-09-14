using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class RealtimeBarArgs :EventArgs
    {
       public RealTimeBarsToken Token { get; }
       public long Time { get; }
       public double Open { get; }
       public double High { get; }
       public double Low { get; }
       public double Close { get; }
       public long Volume { get; }
       public double WAP { get; }
       public int Count { get; }
       public RealtimeBarArgs(int reqId, long time, double open, double high, double low, double close, long volume, double Wap, int count)
        {
            Token = new RealTimeBarsToken(reqId);
            Time = time;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            WAP = Wap;
            Count = count;
        }
    }
}