using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class UpdateAccountValueArgs :EventArgs
    {
       public string Key { get; }
       public string Value { get; }
       public string Currency { get; }
       public string AccountName { get; }
       public UpdateAccountValueArgs(string key, string value, string currency, string accountName)
        {
            Key = key;
            Value = value;
            Currency = currency;
            AccountName = accountName;
        }
    }
}