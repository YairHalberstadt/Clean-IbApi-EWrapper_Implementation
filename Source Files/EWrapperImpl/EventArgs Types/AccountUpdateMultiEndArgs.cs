using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class AccountUpdateMultiEndArgs :EventArgs
    {
       public AccountUpdatesMultiToken Token { get; }
       public AccountUpdateMultiEndArgs(int requestId)
        {
            Token = new AccountUpdatesMultiToken(requestId);
        }
    }
}