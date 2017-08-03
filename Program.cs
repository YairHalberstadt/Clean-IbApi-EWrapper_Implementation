using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    class Program
    {
        static void Main(string[] args)
        {
            EWrapperImpl IBApi = new EWrapperImpl();
            IBApi.ConnectToIB(7496, 1);
            
            IBApi.OpenOrder += OnOrder;
            IBApi.ClientSocket.reqAllOpenOrders();
            Console.Read();
        }

        static void OnOrder(object sender, OpenOrderArgs openOrderargs)
        {
            Console.WriteLine(openOrderargs.Order.OrderType);
        }
    }
}
