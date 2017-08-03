using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class VerifyMessageAPIArgs : EventArgs
{
public string ApiData { get;}

public VerifyMessageAPIArgs(string apiData)
{
ApiData = apiData;
}
}
}

