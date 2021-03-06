using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class AccountSummaryArgs :EventArgs
    {
       public AccountSummaryToken Token { get; }
       public string Account { get; }
       public string Tag { get; }
       public string Value { get; }
       public string Currency { get; }
       public AccountSummaryArgs(int reqId, string account, string tag, string value, string currency)
        {
            Token = new AccountSummaryToken(reqId);
            Account = account;
            Tag = tag;
            Value = value;
            Currency = currency;
        }
    }
}