using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class AccountUpdateMultiArgs :EventArgs
    {
       public AccountUpdatesMultiToken Token { get; }
       public string Account { get; }
       public string ModelCode { get; }
       public string Key { get; }
       public string Value { get; }
       public string Currency { get; }
       public AccountUpdateMultiArgs(int requestId, string account, string modelCode, string key, string value, string currency)
        {
            Token = new AccountUpdatesMultiToken(requestId);
            Account = account;
            ModelCode = modelCode;
            Key = key;
            Value = value;
            Currency = currency;
        }
    }
}