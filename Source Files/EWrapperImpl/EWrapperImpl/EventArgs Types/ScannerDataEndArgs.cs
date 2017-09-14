using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class ScannerDataEndArgs :EventArgs
    {
       public ScannerSubscriptionToken Token { get; }
       public ScannerDataEndArgs(int reqId)
        {
            Token = new ScannerSubscriptionToken(reqId);
        }
    }
}