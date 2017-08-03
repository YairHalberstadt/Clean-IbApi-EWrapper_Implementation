using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class ScannerDataArgs : EventArgs
{
public int ReqId { get;}
public int Rank { get;}
public ContractDetails ContractDetails { get;}
public string Distance { get;}
public string Benchmark { get;}
public string Projection { get;}
public string LegsStr { get;}

public ScannerDataArgs(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
{
ReqId = reqId;
Rank = rank;
ContractDetails = contractDetails;
Distance = distance;
Benchmark = benchmark;
Projection = projection;
LegsStr = legsStr;
}
}
}

