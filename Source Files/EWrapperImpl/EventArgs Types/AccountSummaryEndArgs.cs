using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class AccountSummaryEndArgs :EventArgs
    {
       public AccountSummaryToken Token { get; }
       public AccountSummaryEndArgs(int reqId)
        {
            Token = new AccountSummaryToken(reqId);
        }
    }
}