using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class AccountUpdateMultiArgs : EventArgs
{
public int RequestId { get;}
public string Account { get;}
public string ModelCode { get;}
public string Key { get;}
public string Value { get;}
public string Currency { get;}

public AccountUpdateMultiArgs(int requestId, string account, string modelCode, string key, string value, string currency)
{
RequestId = requestId;
Account = account;
ModelCode = modelCode;
Key = key;
Value = value;
Currency = currency;
}
}
}

