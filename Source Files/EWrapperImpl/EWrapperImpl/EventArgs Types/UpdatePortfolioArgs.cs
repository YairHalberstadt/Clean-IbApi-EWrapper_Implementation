using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class UpdatePortfolioArgs :EventArgs
    {
       public Contract Contract { get; }
       public double Position { get; }
       public double MarketPrice { get; }
       public double MarketValue { get; }
       public double AverageCost { get; }
       public double UnrealisedPNL { get; }
       public double RealisedPNL { get; }
       public string AccountName { get; }
       public UpdatePortfolioArgs(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealisedPNL, double realisedPNL, string accountName)
        {
            Contract = contract;
            Position = position;
            MarketPrice = marketPrice;
            MarketValue = marketValue;
            AverageCost = averageCost;
            UnrealisedPNL = unrealisedPNL;
            RealisedPNL = realisedPNL;
            AccountName = accountName;
        }
    }
}