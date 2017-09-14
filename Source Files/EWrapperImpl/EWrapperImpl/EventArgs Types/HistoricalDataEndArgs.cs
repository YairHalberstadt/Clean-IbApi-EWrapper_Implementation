using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class HistoricalDataEndArgs :EventArgs
    {
       public HistoricalDataToken Token { get; }
       public string Start { get; }
       public string End { get; }
       public HistoricalDataEndArgs(int reqId, string start, string end)
        {
            Token = new HistoricalDataToken(reqId);
            Start = start;
            End = end;
        }
    }
}