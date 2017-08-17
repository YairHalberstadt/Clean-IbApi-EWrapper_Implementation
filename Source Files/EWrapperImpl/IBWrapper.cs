using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class IBWrapper
    {
        public IBReceiver Receiver { get; }
        public IBRequester Requester { get; }
        internal EReaderSignal Signal { get; }

        public IBWrapper()
        {
            Signal = new EReaderMonitorSignal();
            Receiver = new IBReceiver(this);
            Requester = new IBRequester(Signal, Receiver);
        }

        /// <summary>
        /// Connects to IB. This older version does not have the ExtraAuth parameter.
        /// </summary>
        /// <param name="port">Port is usually 7496 for TWS, 7497 for paper trading / demo account on TWS, 4001 for IBGateway, 
        /// 4002 for paper trading account on IBGateway.
        /// The port can be changed on Settings.</param>
        /// <param name="clientId">You can have up to 9 seperate connections to IB at a time by using different ClientId's</param>
        /// <param name="host">leave blank to default to the local host</param>
        public void ConnectToIB(int port, int clientId, string host="")
        {
            Requester.ConnectToIB(port, clientId,host);
        }

        /// <summary>
        /// Connects to IB.
        /// </summary>
        /// <param name="port">
        /// Port is usually 7496 for TWS, 7497 for paper trading / demo account on TWS, 4001 for IBGateway, 
        /// 4002 for paper trading account on IBGateway.
        /// The port can be changed on Settings.</param>
        /// <param name="clientId">
        /// You can have up to 9 seperate connections to IB at a time by using different ClientId's.</param>
        /// <param name="extraAuth"></param>
        /// <param name="host">leave blank to default to the local host</param>
        public void ConnectToIB(int port, int clientId, bool extraAuth, string host = "")
        {
            Requester.ConnectToIB(port, clientId, extraAuth, host);
        }
    }
}
