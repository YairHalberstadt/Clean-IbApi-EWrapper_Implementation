using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class SoftDollarTiersArgs : EventArgs
{
public int ReqId { get;}
public SoftDollarTier[] Tiers { get;}

public SoftDollarTiersArgs(int reqId, SoftDollarTier[] tiers)
{
ReqId = reqId;
Tiers = tiers;
}
}
}

