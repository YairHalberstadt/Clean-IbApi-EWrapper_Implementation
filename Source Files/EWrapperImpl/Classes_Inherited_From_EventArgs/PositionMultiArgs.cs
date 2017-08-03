using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class PositionMultiArgs : EventArgs
{
public int RequestId { get;}
public string Account { get;}
public string ModelCode { get;}
public Contract Contract { get;}
public double Pos { get;}
public double AvgCost { get;}

public PositionMultiArgs(int requestId, string account, string modelCode, Contract contract, double pos, double avgCost)
{
RequestId = requestId;
Account = account;
ModelCode = modelCode;
Contract = contract;
Pos = pos;
AvgCost = avgCost;
}
}
}

