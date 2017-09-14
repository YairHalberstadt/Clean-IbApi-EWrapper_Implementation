using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class PositionArgs :EventArgs
    {
       public string Account { get; }
       public Contract Contract { get; }
       public double Pos { get; }
       public double AvgCost { get; }
       public PositionArgs(string account, Contract contract, double pos, double avgCost)
        {
            Account = account;
            Contract = contract;
            Pos = pos;
            AvgCost = avgCost;
        }
    }
}