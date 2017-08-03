using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class UpdateAccountTimeArgs : EventArgs
{
public string Timestamp { get;}

public UpdateAccountTimeArgs(string timestamp)
{
Timestamp = timestamp;
}
}
}

