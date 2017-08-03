using IBApi;
using System;

namespace EWrapperImpl
{
    class SampleProgram
    {
        static void Main(string[] args)
        {
            EWrapperImpl IBApi = new EWrapperImpl();
            IBApi.ConnectToIB(7496, 1);
            
            IBApi.OpenOrder += OnOrder;
            IBApi.ClientSocket.reqAllOpenOrders();
            Console.Read();
        }

        static void OnOrder(object sender, OpenOrderArgs openOrderArgs)
        {
            Console.WriteLine(openOrderArgs.Order.OrderType);
        }
    }
}
