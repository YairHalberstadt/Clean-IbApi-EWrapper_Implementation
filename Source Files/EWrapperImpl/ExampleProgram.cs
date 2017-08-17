using IBApi;
using System;
using System.Threading;

namespace EWrapperImpl
{
    class Program
    {
        static void Main(string[] args)
        {
            IBWrapper wrapper = new IBWrapper();
            wrapper.ConnectToIB(7496,1,"");
            Contract contract = new Contract()
            {
                LocalSymbol = "EUR.USD",
                Symbol = "EUR",
                Exchange = "IDEALPRO",
                Currency = "USD",
                SecType="Cash"
            };
            MktDataToken token = wrapper.Requester.ReqMarketData(contract, "", false, null, WriteTickPrice);

            Thread.Sleep(10000);     //In general using thread.sleep is bad coding practice. 
                                     //However as this is only an example program for tutorial purposes, I'll let myself off here.
            wrapper.Receiver.TickPriceSubscribe(token, WriteMktDataTokenId);

            Thread.Sleep(10000);     //In general using thread.sleep is bad coding practice. 
                                     //However as this is only an example program for tutorial purposes, I'll let myself off here.
            wrapper.Receiver.TickPriceUnSubscribe(token, WriteTickPrice);
            Console.Read();
        }

        static void WriteTickPrice(object sender, TickPriceArgs tickPriceArgs)
        {
            Console.WriteLine(tickPriceArgs.Price);
        }
        static void WriteMktDataTokenId(object sender, TickPriceArgs tickPriceArgs)
        {
            Console.WriteLine(tickPriceArgs.Token.ID);
        }
    }
}
