using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class ManagedAccountsArgs : EventArgs
{
public string AccountsList { get;}

public ManagedAccountsArgs(string accountsList)
{
AccountsList = accountsList;
}
}
}

