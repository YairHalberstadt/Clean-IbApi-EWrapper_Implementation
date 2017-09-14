using EWrapperImpl;
using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockCandlesCollector;

namespace StockCandlesCollectorProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Contract contract = new Contract()
            {
                LocalSymbol = "GBP.USD",
                Symbol = "GBP",
                Exchange = "IDEALPRO",
                Currency = "USD",
                SecType = "CASH"
            };
            for (int i = 0; i < 10000; i++)
            {
                using (IBWrapper ibWrapper = new IBWrapper())
                {
                    ibWrapper.ConnectToIB(7497, 2);

                    CandlesCollector collector = new CandlesCollector(ibWrapper, CandleSize.FiveSeconds, contract,Duration.Null);
                    Task.Delay(10000).Wait();
                    Console.WriteLine($"{ i}  FINISHED");
                }
            }
        }
    }
}
