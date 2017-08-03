using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class HistoricalDataEndArgs : EventArgs
{
public int ReqId { get;}
public string Start { get;}
public string End { get;}

public HistoricalDataEndArgs(int reqId, string start, string end)
{
ReqId = reqId;
Start = start;
End = end;
}
}
}

