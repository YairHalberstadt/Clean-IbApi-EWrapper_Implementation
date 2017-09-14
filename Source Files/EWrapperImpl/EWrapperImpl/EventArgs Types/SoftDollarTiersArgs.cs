using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class SoftDollarTiersArgs :EventArgs
    {
       public SoftDollarTiersToken Token { get; }
       public SoftDollarTier[] Tiers { get; }
       public SoftDollarTiersArgs(int reqId, SoftDollarTier[] tiers)
        {
            Token = new SoftDollarTiersToken(reqId);
            Tiers = tiers;
        }
    }
}