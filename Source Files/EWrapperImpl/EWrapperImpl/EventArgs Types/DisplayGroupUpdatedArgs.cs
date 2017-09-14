using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class DisplayGroupUpdatedArgs :EventArgs
    {
       public subscribeToGroupEventsToken Token { get; }
       public string ContractInfo { get; }
       public DisplayGroupUpdatedArgs(int reqId, string contractInfo)
        {
            Token = new subscribeToGroupEventsToken(reqId);
            ContractInfo = contractInfo;
        }
    }
}