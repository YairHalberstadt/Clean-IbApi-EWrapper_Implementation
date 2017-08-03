using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class SecurityDefinitionOptionParameterArgs : EventArgs
{
public int ReqId { get;}
public string Exchange { get;}
public int UnderlyingConId { get;}
public string TradingClass { get;}
public string Multiplier { get;}
public HashSet<string> Expirations { get;}
public HashSet<double> Strikes { get;}

public SecurityDefinitionOptionParameterArgs(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
{
ReqId = reqId;
Exchange = exchange;
UnderlyingConId = underlyingConId;
TradingClass = tradingClass;
Multiplier = multiplier;
Expirations = expirations;
Strikes = strikes;
}
}
}

