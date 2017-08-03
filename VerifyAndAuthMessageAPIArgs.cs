using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class VerifyAndAuthMessageAPIArgs : EventArgs
{
public string ApiData { get;}
public string XyzChallenge { get;}

public VerifyAndAuthMessageAPIArgs(string apiData, string xyzChallenge)
{
ApiData = apiData;
XyzChallenge = xyzChallenge;
}
}
}

