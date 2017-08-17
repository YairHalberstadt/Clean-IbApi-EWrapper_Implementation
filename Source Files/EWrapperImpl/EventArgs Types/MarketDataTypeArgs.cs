using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class MarketDataTypeArgs :EventArgs
    {
       public int ReqId { get; }
       public int MarketDataType { get; }
       public MarketDataTypeArgs(int reqId, int marketDataType)
        {
            ReqId = reqId;
            MarketDataType = marketDataType;
        }
    }
}