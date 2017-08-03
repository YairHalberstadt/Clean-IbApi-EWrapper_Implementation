using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class AccountDownloadEndArgs : EventArgs
{
public string Account { get;}

public AccountDownloadEndArgs(string account)
{
Account = account;
}
}
}

