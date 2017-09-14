using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class PositionMultiArgs :EventArgs
    {
       public PositionsMultiToken Token { get; }
       public string Account { get; }
       public string ModelCode { get; }
       public Contract Contract { get; }
       public double Pos { get; }
       public double AvgCost { get; }
       public PositionMultiArgs(int requestId, string account, string modelCode, Contract contract, double pos, double avgCost)
        {
            Token = new PositionsMultiToken(requestId);
            Account = account;
            ModelCode = modelCode;
            Contract = contract;
            Pos = pos;
            AvgCost = avgCost;
        }
    }
}