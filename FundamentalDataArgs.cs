using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class FundamentalDataArgs : EventArgs
{
public int ReqId { get;}
public string Data { get;}

public FundamentalDataArgs(int reqId, string data)
{
ReqId = reqId;
Data = data;
}
}
}

