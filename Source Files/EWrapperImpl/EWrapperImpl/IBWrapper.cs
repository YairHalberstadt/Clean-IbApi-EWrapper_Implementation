using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class IBWrapper :IDisposable
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

        internal ManualResetEvent ResetEvent = new ManualResetEvent(false);
        /// <summary>
        /// Connects to IB.
        /// </summary>
        /// <param name="port">
        /// Port is usually 7496 for TWS, 7497 for paper trading / demo account on TWS, 4001 for IBGateway, 
        /// 4002 for paper trading account on IBGateway.
        /// The port can be changed on Settings.</param>
        /// <param name="clientId">
        /// You can have up to 9 seperate connections to IB at a time by using different ClientId's.</param>
        /// <param name="host">leave blank to default to the local host</param>
        /// <param name="extraAuth"></param>
        public void ConnectToIB(int port, int clientId, string host = "", bool extraAuth=false)
        {
            Requester.ConnectToIB(port, clientId, extraAuth, host);
            ResetEvent.WaitOne();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Requester.EDisconnect();
                disposedValue = true;
            }
        }

        
        ~IBWrapper() {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
