using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class IBRequester
    {
        private EClientSocket ClientSocket { get; }
        private IBReceiver Receiver { get; }
        private EReaderSignal Signal { get; }

        internal IBRequester(EReaderSignal signal, IBReceiver receiver)
        {
            ClientSocket = new EClientSocket(receiver, signal);
            Receiver = receiver;
            Signal = signal;
        }

        internal void ConnectToIB(int port, int clientId, string host="")

        {
            ClientSocket.eConnect(host, port, clientId);

            var reader = new EReader(ClientSocket, Signal);
            reader.Start();
            new Thread(() =>
            {
                while (ClientSocket.IsConnected())
                {
                    Signal.waitForSignal();
                    reader.processMsgs();
                }
            })
            { IsBackground = true }.Start();

            // Pause here until the connection is complete 
            while (NextOrderId <= 0) { }
        }

        internal void ConnectToIB(int port, int clientId, bool extraAuth, string host="")
        {
            ClientSocket.eConnect(host, port, clientId, extraAuth);

            var reader = new EReader(ClientSocket, Signal);
            reader.Start();
            new Thread(() =>
            {
                while (ClientSocket.IsConnected())
                {
                    Signal.waitForSignal();
                    reader.processMsgs();
                }
            })
            { IsBackground = true }.Start();

            // Pause here until the connection is complete 
            while (NextOrderId <= 0) { }
        }

        public int NextOrderId { get { return _NextOrderId++; } internal set { _NextOrderId = value; } }
        private int _NextOrderId = 0;



        /// <summary>
        /// Allows to switch between different current (V100+) and previous connection mechanisms.
        /// </summary>
        public void DisableUseV100Plus()
        {
            ClientSocket.DisableUseV100Plus();
        }

        /// <summary>
        /// Indicates whether the API-TWS connection has been closed. Note: This function is not automatically invoked and must be by the API client.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return ClientSocket.IsConnected();
        }

        /// <summary>
        /// Initiates the message exchange between the client application and the TWS/IB Gateway. 
        /// </summary>
        public void StartApi()
        {
            ClientSocket.startApi();
        }

        /// <summary>
        /// Terminates the connection and notifies IBReciever.
        /// </summary>
        public void Close()
        {
            ClientSocket.Close();
        }

        /// <summary>
        /// Closes the socket connection and terminates its thread. 
        /// </summary>
        public void EDisconnect()
        {
            ClientSocket.eDisconnect();
        }

        /// <summary>
        /// Cancels a historical data request.
        /// </summary>
        /// <param name="token">The Token returned from the original Historical Data Request</param>
        public void CancelHistoricalData(HistoricalDataToken token)
        {
            ClientSocket.cancelHistoricalData(token.ID);
        }

        /// <summary>
        ///Request the calculation of the implied volatility based on hypothetical option and its underlying prices.
        ///The calculation will be returned to IBReceiver's tickOptionComputation callback.
        ///Use the Token returned from this function to Subscribe a function to be called by tickOptionComputation.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="optionPrice"></param>
        /// <param name="underPrice"></param>
        /// <param name="impliedVolatilityOptions"></param>
        /// <param name="tickOptionComputationSubscribers"> A delegate containing all functions you want to subscribe to the tickOptionComputationCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by tickOptionComputation.</returns>
        public TickOptionComputationToken CalculateImpliedVolatility(Contract contract, double optionPrice, double underPrice, List<TagValue> impliedVolatilityOptions = null, Action<object, TickOptionComputationArgs> tickOptionComputationSubscribers = null)
        {
            TickOptionComputationToken token = new TickOptionComputationToken();
            if (tickOptionComputationSubscribers != null)
            {
                Receiver.TickOptionComputationSubscribe(token, tickOptionComputationSubscribers);
            }
            ClientSocket.calculateImpliedVolatility(token.ID, contract, optionPrice, underPrice, impliedVolatilityOptions);
            return token;
        }

        /// <summary>
        ///Request the calculation of the implied volatility based on hypothetical option and its underlying prices.
        ///The calculation will be returned to IBReceiver's tickOptionComputation callback.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="optionPrice"></param>
        /// <param name="underPrice"></param>
        /// <param name="optionPriceOptions"></param>
        /// <param name="tickOptionComputationSubscribers">A delegate containing all functions you want to subscribe to the tickOptionComputationCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by tickOptionComputation.</returns>
        public TickOptionComputationToken CalculateOptionPrice(Contract contract, double optionPrice, double underPrice, List<TagValue> optionPriceOptions = null, Action<object, TickOptionComputationArgs> tickOptionComputationSubscribers = null)
        {
            TickOptionComputationToken token = new TickOptionComputationToken();
            if (tickOptionComputationSubscribers != null)
            {
                Receiver.TickOptionComputationSubscribe(token, tickOptionComputationSubscribers);
            }
            ClientSocket.calculateOptionPrice(token.ID, contract, optionPrice, underPrice, optionPriceOptions);
            return token;
        }

        /// <summary>
        /// Cancels the account's summary request.
        /// </summary>
        /// <param name="token">The Token returned from the original Account Summary Request</param>
        public void CancelAccountSummary(AccountSummaryToken token)
        {
            ClientSocket.cancelAccountSummary(token.ID);
        }

        /// <summary>
        /// Cancels an option's implied volatility calculation request.
        /// </summary>
        /// <param name="token">The Token returned from the original Request</param>
        public void CancelCalculateImpliedVolatility(TickOptionComputationToken token)
        {
            ClientSocket.cancelCalculateImpliedVolatility(token.ID);
        }

        /// <summary>
        /// Cancels an option's price calculation request.
        /// </summary>
        /// <param name="token">The Token returned from the original Request</param>
        public void CancelCalculateOptionPrice(TickOptionComputationToken token)
        {
            ClientSocket.cancelCalculateOptionPrice(token.ID);
        }

        /// <summary>
        /// Cancels Fundamental Data request.
        /// </summary>
        /// <param name="token">The Token returned from the original Request</param>
        public void CancelFundamentalData(FundamentalDataToken token)
        {
            ClientSocket.cancelFundamentalData(token.ID);
        }


        /// <summary>
        /// Cancels Mkt Data request.
        /// </summary>
        /// <param name="token">The Token returned from the original Request</param>
        public void CancelMktData(MktDataToken token)
        {
            ClientSocket.cancelMktData(token.ID);
        }

        /// <summary>
        /// Cancels Mkt Depth request.
        /// </summary>
        /// <param name="token">The Token returned from the original Request</param>
        public void CancelMktDepth(MktDepthToken token)
        {
            ClientSocket.cancelMktDepth(token.ID);
        }

        /// <summary>
        /// Cancels News Bulletin Subscription.
        /// </summary>
        public void CancelNewsBulletin()
        {
            ClientSocket.cancelNewsBulletin();
        }

        /// <summary>
        ///Cancels an active order placed by from the same API client ID.
        ///Note: API clients cannot cancel individual orders placed by other clients.Only reqGlobalCancel is available.
        /// </summary>
        /// <param name="orderID">the order's client id</param>
        public void CancelOrder(int orderID)
        {
            ClientSocket.cancelOrder(orderID);
        }

        /// <summary>
        /// Cancels a previous position subscription request made with reqPositions.
        /// </summary>
        public void CancelPositions()
        {
            ClientSocket.cancelPositions();
        }

        /// <summary>
        /// Cancels Real Time Bars Subscription
        /// </summary>
        /// <param name="token">The Token returned from the original Request</param>
        public void CancelRealTimeBars(RealTimeBarsToken token)
        {
            ClientSocket.cancelRealTimeBars(token.ID);
        }

        /// <summary>
        /// Cancels Scanner Subscription
        /// </summary>
        /// <param name="token">The Token returned from the original Request</param>
        public void CancelScannerSubscription(ScannerSubscriptionToken token)
        {
            ClientSocket.cancelScannerSubscription(token.ID);
        }

        /// <summary>
        ///Exercises an options contract
        ///Note: this function is affected by a TWS setting which specifies if an exercise request must be finalized.
        /// </summary>
        /// <param name="contract">	the option Contract to be exercised.</param>
        /// <param name="exerciseAction">set to 1 to exercise the option, set to 2 to let the option lapse.</param>
        /// <param name="exerciseQuantity">	number of contracts to be exercised</param>
        /// <param name="account">	destination account</param>
        /// <param name="overide">Specifies whether your setting will override the system's natural action. 
        /// For example, if your action is "exercise" and the option is not in-the-money, by natural action the option would not exercise. 
        /// If you have override set to "yes" the natural action would be overridden and the out-of-the money option would be exercised. 
        /// Set to 1 to override, set to 0 not to.</param>
        /// <returns>This Token should not be needed except in case of an error message</returns>
        public ExerciseOptionsToken ExerciseOptions(Contract contract, int exerciseAction, int exerciseQuantity, string account, int overide)
        {
            ExerciseOptionsToken token = new ExerciseOptionsToken();
            ClientSocket.exerciseOptions(token.ID, contract, exerciseAction, exerciseQuantity, account, overide);
            return token;
        }

        /// <summary>
        /// Places or modifies an order.
        /// </summary>
        /// <param name="orderId">the order's unique identifier. If only one IBWrapper is placing orders at a time the nextOrderId property will
        /// return the next available orderID. However if other API's may be connected to IB the nextValidId method should be used. 
        /// Be aware that race consditions could occur in such a case. If a new order is placed with an order ID less than or equal 
        /// to the order ID of a previous order an error will occur.</param>
        /// <param name="contract">the order's contract</param>
        /// <param name="order">the order</param>
        /// <param name="openOrderSubscribers">A delegate containing all functions you want to subscribe to the openOrderCallBack</param>
        /// <param name="orderStatusSubscribers">A delegate containing all functions you want to subscribe to the orderStatusCallBack</param>
        /// <param name="execDetailsSubscribers">A delegate containing all functions you want to subscribe to the execDetailsCallBack</param>
        /// <param name="commissionReportSubscribers">A delegate containing all functions you want to subscribe to thecommisionReportCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by execDetails.</returns>
        public ExecutionsToken PlaceOrder(int orderId, Contract contract, Order order, Action<object, OpenOrderArgs> openOrderSubscribers = null, Action<object, OrderStatusArgs> orderStatusSubscribers = null, Action<object, ExecDetailsArgs> execDetailsSubscribers = null, Action<object, CommissionReportArgs> commissionReportSubscribers = null)
        {
            if (openOrderSubscribers != null)
            {
                Receiver.OpenOrderSubscribe(openOrderSubscribers);
            }

            if (orderStatusSubscribers != null)
            {
                Receiver.OrderStatusSubscribe(orderStatusSubscribers);
            }

            ExecutionsToken token = new ExecutionsToken(-1);
            if (execDetailsSubscribers != null)
            {
                Receiver.ExecDetailsSubscribe(token, execDetailsSubscribers);
            }

            if (commissionReportSubscribers != null)
            {
                Receiver.CommissionReportSubscribe(commissionReportSubscribers);
            }

            ClientSocket.placeOrder(orderId, contract, order);

            return token;
        }

        /// <summary>
        /// Replaces Financial Advisor's settings A Financial Advisor can define three different configurations:
        ///1. Groups: offer traders a way to create a group of accounts and apply a single allocation method to all accounts in the group.
        ///2. Profiles: let you allocate shares on an account-by-account basis using a predefined calculation value.
        ///3. Account Aliases: let you easily identify the accounts by meaningful names rather than account numbers.
        ///More information at https://www.interactivebrokers.com/en/?f=%2Fen%2Fsoftware%2Fpdfhighlights%2FPDF-AdvisorAllocations.php%3Fib_entity%3Dllc
        /// </summary>
        /// <param name="faDataType">the configuration to change. Set to 1, 2 or 3 as defined above.</param>
        /// <param name="xml">the xml-formatted configuration string</param>
        public void ReplaceFa(int faDataType, string xml)
        {
            ClientSocket.replaceFA(faDataType, xml);
        }

        /// <summary>
        /// Replaces Financial Advisor's settings A Financial Advisor can define three different configurations:
        ///1. Groups: offer traders a way to create a group of accounts and apply a single allocation method to all accounts in the group.
        ///2. Profiles: let you allocate shares on an account-by-account basis using a predefined calculation value.
        ///3. Account Aliases: let you easily identify the accounts by meaningful names rather than account numbers.
        ///More information at https://www.interactivebrokers.com/en/?f=%2Fen%2Fsoftware%2Fpdfhighlights%2FPDF-AdvisorAllocations.php%3Fib_entity%3Dllc
        /// </summary>
        /// <param name="faDataType">the configuration to change. Set to 1, 2 or 3 as defined above.</param>
        /// <param name="faDataType"></param>
        public void RequestFa(int faDataType)
        {
            ClientSocket.requestFA(faDataType);
        }

        /// <summary>
        ///Requests a specific account's summary.
        ///This method will subscribe to the account summary as presented in the TWS' Account Summary tab.
        /// The data is returned at IBReceiver::accountSummary
        /// </summary>
        /// <param name="group">set to "All" to return account summary data for all accounts, or set to a specific Advisor Account Group name that has already been created in TWS Global Configuration.</param>
        /// <param name="tags">a comma separated list with the desired tags: https://interactivebrokers.github.io/tws-api/classIBApi_1_1EClient.html#a3e0d55d36cd416639b97ee6e47a86fe9 </param>
        /// <param name="accountSummarySubscribers">A delegate containing all functions you want to subscribe to the accountSummaryCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by accountSummary.</returns>
        public AccountSummaryToken ReqAccountSummary(string group, string tags, Action<object, AccountSummaryArgs> accountSummarySubscribers = null)
        {
            AccountSummaryToken token = new AccountSummaryToken();
            ClientSocket.reqAccountSummary(token.ID, group, tags);
            //TODO subscribe
            return token;
        }


        /// <summary>
        ///Requests a specific account's summary.
        ///This method will subscribe to the account summary as presented in the TWS' Account Summary tab.
        ///Subscribes to an specific account's information and portfolio Through this method, a single account's 
        ///subscription can be started/stopped. As a result from the subscription, the account's information, portfolio
        ///and last update time will be received at IBReceiver::updateAccountValue, IBReceiver::updatePortfolio, 
        ///IBReceiver::updateAccountTime respectively.
        ///All account values and positions will be returned initially, and then there will only be updates when 
        ///there is a change in a position, or to an account value every 3 minutes if it has changed. 
        ///Only one account can be subscribed at a time. A second subscription request for another account when the previous 
        ///one is still active will cause the first one to be canceled in favour of the second one. Consider user reqPositions 
        ///if you want to retrieve all your accounts' portfolios directly.
        /// </summary>
        /// <param name="subscribe">set to true to start the subscription and to false to stop it.</param>
        /// <param name="accountCode">	the account id (e.g. U123456) for which the information is requested.</param>
        /// <param name="updateAccountValueSubscribers">>A delegate containing all functions you want to subscribe to the updateAccountValueCallBack</param>
        /// <param name="updatePortfolioSubscribers">A delegate containing all functions you want to subscribe to the updatePortfolioCallBack</param>
        /// <param name="updateAccountTimeSubscribers">A delegate containing all functions you want to subscribe to the updateAccountTimeCallBack</param>
        public void ReqAccountUpdates(bool subscribe, string accountCode, Action<object, UpdateAccountValueArgs> updateAccountValueSubscribers = null, Action<object, UpdatePortfolioArgs> updatePortfolioSubscribers = null, Action<object, UpdateAccountTimeArgs> updateAccountTimeSubscribers = null)
        {
            ClientSocket.reqAccountUpdates(subscribe, accountCode);
            //TODO subscribe
        }

        /// <summary>
        /// Requests all current open orders in associated accounts at the current moment. 
        /// The existing orders will be received via the openOrder and orderStatus callbacks. 
        /// Open orders are returned once; this function does not initiate a subscription.
        /// </summary>
        /// <param name="openOrderSubscribers">A delegate containing all functions you want to subscribe to the OpenOrderCallBack</param>
        /// <param name="orderStatusSubscribers">A delegate containing all functions you want to subscribe to the OrderStatusCallBack</param>
        public void ReqAllOpenOrders(Action<object, OpenOrderArgs> openOrderSubscribers = null, Action<object, OrderStatusArgs> orderStatusSubscribers = null)
        {
            if (openOrderSubscribers != null)
            {
                Receiver.OpenOrderSubscribe(openOrderSubscribers);
            }

            if (orderStatusSubscribers != null)
            {
                Receiver.OrderStatusSubscribe(orderStatusSubscribers);
            }

            ClientSocket.reqAllOpenOrders();
        }

        /// <summary>
        /// Requests status updates about future orders placed from TWS. Can only be used with client ID 0.
        /// </summary>
        /// <param name="autoBind">if set to true, the newly created orders will be assigned an API order ID and implicitly associated with this client. If set to false, future orders will not be.</param>
        /// <param name="openOrderSubscribers">A delegate containing all functions you want to subscribe to the OpenOrderCallBack</param>
        /// <param name="orderStatusSubscribers">A delegate containing all functions you want to subscribe to the OrderStatusCallBack</param>

        public void ReqAutoOpenOrders(bool autoBind, Action<object, OpenOrderArgs> openOrderSubscribers = null, Action<object, OrderStatusArgs> orderStatusSubscribers = null)
        {
            if (openOrderSubscribers != null)
            {
                Receiver.OpenOrderSubscribe(openOrderSubscribers);
            }

            if (orderStatusSubscribers != null)
            {
                Receiver.OrderStatusSubscribe(orderStatusSubscribers);
            }
            ClientSocket.reqAutoOpenOrders(autoBind);
        }

        /// <summary>
        /// Requests contract information.
        ///This method will provide all the contracts matching the contract provided.
        ///This information will be returned at IBReceiver:contractDetails callback, IBReceiver::bondContractDetails.
        ///It can also be used to retrieve complete options and futures chains.
        ///Though it is now (in API version > 9.72.12) advised to use reqSecDefOptParams for that purpose.
        /// </summary>
        /// <param name="contract">the contract used as sample to query the available contracts. Typically, it will contain the Contract::Symbol, Contract::Currency, Contract::SecType, Contract::Exchange</param>
        /// <param name="contractDetailsSubscribers">A delegate containing all functions you want to subscribe to the ContractDetailsCallBack</param>
        /// <param name="bondContractDetailsSubscribers">A delegate containing all functions you want to subscribe to the bondContractDetailsCallBack</param>
        /// <returns>>Use this token to Subscribe a function to be called by any of the CallBacks</returns>
        public ContractDetailsToken ReqContractDetails(Contract contract, Action<object, ContractDetailsArgs> contractDetailsSubscribers = null, Action<object, BondContractDetailsArgs> bondContractDetailsSubscribers = null)
        {
            ContractDetailsToken token = new ContractDetailsToken();
            if (contractDetailsSubscribers != null)
            {
                Receiver.ContractDetailsSubscribe(token, contractDetailsSubscribers);
            }

            if (bondContractDetailsSubscribers != null)
            {
                Receiver.BondContractDetailsSubscribe(token, bondContractDetailsSubscribers);
            }
            ClientSocket.reqContractDetails(token.ID, contract);
            return token;
        }

        /// <summary>
        /// Requests TWS's current time. Returned at IBReceivers currentTime CallBack 
        /// </summary>
        /// <param name="currentTimeSubscribers">>A delegate containing all functions you want to subscribe to the CurrentTimeCallBack</param>
        public void ReqCurrentTime(Action<object, CurrentTimeArgs> currentTimeSubscribers = null)
        {
            if (currentTimeSubscribers != null)
            {
                Receiver.CurrentTimeSubscribe(currentTimeSubscribers);
            }
            ClientSocket.reqCurrentTime();
        }

        /// <summary>
        /// Requests current day's (since midnight) executions matching the filter. 
        /// Only the current day's executions can be retrieved. Along with the executions, 
        /// the CommissionReport will also be returned. The execution details will arrive at IBReceiver:execDetails.
        /// </summary>
        /// <param name="filter">	the filter criteria used to determine which execution reports are returned. See https://interactivebrokers.github.io/tws-api/classIBApi_1_1ExecutionFilter.html#gsc.tab=0 </param>
        /// <param name="execDetailsSubscribers">A delegate containing all functions you want to subscribe to the ExecDetailsCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by execDetails</returns>
        public ExecutionsToken ReqExecutions(ExecutionFilter filter, Action<object, ExecDetailsArgs> execDetailsSubscribers = null)
        {
            ExecutionsToken token = new ExecutionsToken();
            if (execDetailsSubscribers != null)
            {
                Receiver.ExecDetailsSubscribe(token,execDetailsSubscribers);
            }
            
            ClientSocket.reqExecutions(token.ID, filter);
            return token;
        }

        /// <summary>
        /// Requests the contract's Reuters or Wall Street Horizons fundamental data. 
        /// Fundalmental data is returned at IBReceiver::fundamentalData. 
        /// </summary>
        /// <param name="contract">	the contract's description for which the data will be returned.</param>
        /// <param name="reportType">The available report types are: 1. ReportSnapshot: Company overview
        ///2. ReportsFinSummary: Financial summary
        ///3. ReportRatios: Financial ratios
        ///4. ReportsFinStatements: Financial statements
        ///5. RESC: Analyst estimates
        ///6. CalendarReport: Company calendar from Wall Street Horizons
        ///</param>
        /// <param name="fundamentalDataOptions"></param>
        /// <param name="fundamentalDataSubscribers">A delegate containing all functions you want to subscribe to the fundamentalDataCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by fundamentalData</returns>
        public FundamentalDataToken ReqFundamentalData(Contract contract, String reportType, List<TagValue> fundamentalDataOptions = null, Action<object, FundamentalDataArgs> fundamentalDataSubscribers = null)
        {
            FundamentalDataToken token = new FundamentalDataToken();
            if (fundamentalDataSubscribers != null)
            {
                Receiver.FundamentalDataSubscribe(token, fundamentalDataSubscribers);
            }
            ClientSocket.reqFundamentalData(token.ID, contract, reportType, fundamentalDataOptions);
            return token;
        }

        /// <summary>
        /// Cancels all active orders.
        ///This method will cancel ALL open orders including those placed directly from TWS.
        /// </summary>
        public void ReqGlobalCancel()
        {
            ClientSocket.reqGlobalCancel();
        }

        /// <summary>
        /// Requests contracts' historical data. The resulting bars will be returned in IBReceiver::historicalData
        /// </summary>
        /// <param name="contract">	the contract for which we want to retrieve the data.</param>
        /// <param name="endDateTime">request's ending time</param>
        /// <param name="durationString">the amount of time for which the data needs to be retrieved: eg "3000 S" for 3000 seconds or "5 D" for 5 days. Options are S, D, W (weeks), M (months) and Y (years).</param>
        /// <param name="barSizeSetting">	the size of the bar: options are '1 sec', '5 secs', '15 secs', '30 secs', '1 min', '2 mins', '3 mins', '5 mins', '15 mins', '30 mins', '1 hour' and '1 day'</param>             
        /// <param name="whatToShow">the kind of information being retrieved. Options are : 'TRADES', 'MIDPOINT', 'BID', 'ASK', 'BID_ASK', 'HISTORICAL_VOLATILITY', 'OPTION_IMPLIED_VOLATILITY', 'FEE_RATE' and 'REBATE_RATE'</param>
        /// <param name="useRTH">	false to obtain the data which was also generated outside of the Regular Trading Hours, true to obtain only the RTH data</param>
        /// <param name="formatDate">set to 1 to obtain the bars' time as yyyyMMdd HH:mm:ss, set to 2 to obtain it like system time format in seconds</param>
        /// <param name="keepUpToDate">Whether a subscription is made to return updates of unfinished real time bars as they are available (True),
        /// or all data is returned on a one-time basis (False). 
        /// Available starting with API v973.03+ and TWS v965+. If True, and endDateTime cannot be specified.</param>
        /// <param name="chartOptions"></param>
        /// <param name="historicalDataSubscribers">A delegate containing all functions you want to subscribe to the HistoricalDataCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by HistoricalData</returns>
        public HistoricalDataToken ReqHistoricalData(Contract contract, DateTime endDateTime, string durationString, string barSizeSetting, string whatToShow, bool useRegularTradingHours, int formatDate, bool keepUpToDate, List<TagValue> chartOptions = null, Action<object, HistoricalDataArgs> historicalDataSubscribers = null)
        {
            HistoricalDataToken token = new HistoricalDataToken();
            if (historicalDataSubscribers != null)
            {
                Receiver.HistoricalDataSubscribe(token, historicalDataSubscribers);
            }
            string endDateTimeString = endDateTime.ToString("yyyyMMdd HH:mm:ss");
            ClientSocket.reqHistoricalData(token.ID, contract, endDateTimeString, durationString, barSizeSetting, whatToShow, useRegularTradingHours ? 1 : 0, formatDate, chartOptions);
            return token;
        }

        /// <summary>
        /// Requests the next valid order ID at the current moment. 
        /// Will be received at nextValidId CallBack. Will automatically update NextOrderID property
        /// </summary>
        /// <param name="nextValidIdSubscribers">A delegate containing all functions you want to subscribe to the NextValidIdCallBack</param>
        public void ReqIds(Action<object, int> nextValidIdSubscribers = null)
        {
            if (nextValidIdSubscribers != null)
            {
                Receiver.NextValidIdSubscribe(nextValidIdSubscribers);
            }
            ClientSocket.reqIds(1);
        }

        /// <summary>
        /// Requests the accounts to which the logged user has access to.
        /// Returns at IBReceiver::managedAccounts
        /// </summary>
        /// <param name="ManagedAccountsSubscribers">A delegate containing all functions you want to subscribe to the ManagedAccountsCallBack</param>
        public void ReqManagedAccounts(Action<object, ManagedAccountsArgs> ManagedAccountsSubscribers = null)
        {
            if (ManagedAccountsSubscribers != null)
            {
                Receiver.ManagedAccountsSubscribe(ManagedAccountsSubscribers);
            }
            ClientSocket.reqManagedAccts();
        }

        /// <summary>
        /// Requests real time market data. Returns market data for an instrument either in real time or 10-15 minutes delayed 
        /// (depending on the market data type specified).
        /// Data Returned at IBReceiver::tickPrice, IBReceiver::tickSize, IBReceiver::tickString, IBReceiver::tickEFP, 
        /// IBReceiver::tickGeneric, IBReceiver::tickOptionComputation, IBReceiver::tickSnapshotEnd.
        /// </summary>
        /// <param name="contract">the Contract for which the data is being requested</param>
        /// <param name="genericTickList">comma separated ids of the available generic ticks: "" for price, "100" Option Volume(currently for stocks)
        ///, "101" Option Open Interest(currently for stocks), "104" Historical Volatility(currently for stocks), "106" Option Implied Volatility(currently for stocks)
        ///, "162" Index Future Premium, "165" Miscellaneous Stats, "221" Mark Price(used in TWS P&L computations), "225" Auction values(volume, price and imbalance)
        ///, "233" RTVolume - contains the last trade price, last trade size, last trade time, total volume, VWAP, and single trade flag., "236" Shortable
        ///, "256" Inventory, "258" Fundamental Ratios, "411" Realtime Historical Volatility and "456" IBDividends</param>
        /// <param name="snapshot">for users with corresponding real time market data subscriptions. 
        /// A true value will return a one-time snapshot, while a false value will provide streaming data.</param>
        /// <param name="mktDataOptions">A delegate containing all functions you want to subscribe to the TickPriceCallBack</param>
        /// <param name="tickPriceSubscribers">A delegate containing all functions you want to subscribe to the TickPriceCallBack</param>
        /// <param name="tickSizeSubscribers">>A delegate containing all functions you want to subscribe to the TickSizeCallBack</param>
        /// <param name="tickEfpSubscribers">A delegate containing all functions you want to subscribe to the TickEfpCallBack</param>
        /// <param name="tickGenericSubscribers">A delegate containing all functions you want to subscribe to the TickGenericCallBack</param>
        /// <param name="tickOptionComputationSubscribers">A delegate containing all functions you want to subscribe to the TickOptionComputationCallBack</param>
        /// <param name="tickSnapshotEndSubscribers">A delegate containing all functions you want to subscribe to the TickSnapshotEndCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by any of the CallBacks</returns>
        public MktDataToken ReqMarketData(Contract contract, string genericTickList, bool snapshot, List<TagValue> mktDataOptions = null, Action<object, TickPriceArgs> tickPriceSubscribers = null, Action<object, TickSizeArgs> tickSizeSubscribers = null, Action<object, TickEfpArgs> tickEfpSubscribers = null, Action<object, TickGenericArgs> tickGenericSubscribers = null, Action<object, TickOptionComputationArgs> tickOptionComputationSubscribers = null, Action<object, TickSnapshotEndArgs> tickSnapshotEndSubscribers = null)
        {
            MktDataToken token = new MktDataToken();
            if (tickPriceSubscribers != null)
            {
                Receiver.TickPriceSubscribe(token, tickPriceSubscribers);
            }

            if (tickSizeSubscribers != null)
            {
                Receiver.TickSizeSubscribe(token, tickSizeSubscribers);
            }

            if (tickEfpSubscribers != null)
            {
                Receiver.TickEfpSubscribe(token, tickEfpSubscribers);
            }

            if (tickGenericSubscribers != null)
            {
                Receiver.TickGenericSubscribe(token, tickGenericSubscribers);
            }

            if (tickOptionComputationSubscribers != null)
            {
                Receiver.TickOptionComputationSubscribe(token, tickOptionComputationSubscribers);
            }

            if (tickSnapshotEndSubscribers != null)
            {
                Receiver.TickSnapshotEndSubscribe(token, tickSnapshotEndSubscribers);
            }

            ClientSocket.reqMktData(token.ID, contract, genericTickList, snapshot, mktDataOptions);
            return token;
        }

        /// <summary>
        /// Switches data type returned from reqMktData request to "frozen", "delayed" or "delayed-frozen" market data. Requires TWS/IBG v963+.
        ///The API can receive frozen market data from Trader Workstation. Frozen market data is the last data recorded in our system.
        ///During normal trading hours, the API receives real-time market data. 
        ///Invoking this function with argument 2 requests a switch to frozen data immediately or after the close.
        ///When the market reopens the next data the market data type will automatically switch back to real time if available.
        /// </summary>
        /// <param name="marketDataType">	by default only real-time (1) market data is enabled. Sending 1 (real-time) disables frozen, 
        /// delayed and delayed-frozen market data. Sending 2 (frozen) enables frozen market data. 
        /// Sending 3 (delayed) enables delayed and disables delayed-frozen market data. 
        /// Sending 4 (delayed-frozen) enables delayed and delayed-frozen market data</param>
        public void ReqMarketDataType(int marketDataType)
        {
            ClientSocket.reqMarketDataType(marketDataType);
        }

        /// <summary>
        /// Requests the contract's market depth (order book).
        ///This request must be direct-routed to an exchange and not smart-routed.
        ///The number of simultaneous market depth requests allowed in an account is calculated based on a 
        ///formula that looks at an accounts equity, commissions, and quote booster packs.
        /// </summary>
        /// <param name="numRows">the number of rows on each side of the order book</param>
        /// <param name="contract">the Contract for which the depth is being requested</param>
        /// <param name="mktDepthOptions"></param>
        /// <param name="updateMktDepthSubscribers">A delegate containing all functions you want to subscribe to the UpdateMktDepthCallBack</param>
        /// <param name="updateMktDepthL2Subscribers">A delegate containing all functions you want to subscribe to the UpdateMktDepthL2CallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by any of the CallBacks</returns>
        public MktDepthToken ReqMarketDepth(int numRows, Contract contract, List<TagValue> mktDepthOptions = null, Action<object, UpdateMktDepthArgs> updateMktDepthSubscribers = null, Action<object, UpdateMktDepthL2Args> updateMktDepthL2Subscribers = null)
        {
            MktDepthToken token = new MktDepthToken();
            if (updateMktDepthSubscribers != null)
            {
                Receiver.UpdateMktDepthSubscribe(token, updateMktDepthSubscribers);
            }
            if (updateMktDepthL2Subscribers != null)
            {
                Receiver.UpdateMktDepthL2Subscribe(token, updateMktDepthL2Subscribers);
            }

            ClientSocket.reqMarketDepth(token.ID, contract, numRows, mktDepthOptions);
            return token;
        }

        /// <summary>
        /// Subscribes to IB's News Bulletins. Data returned at IBReceiver::updateNewsBulletin
        /// </summary>
        /// <param name="allMessages">	if set to true, will return all the existing bulletins for the current day, set to false to receive only the new bulletins.</param>
        /// <param name="updateNewsBulletinSubscribers">A delegate containing all functions you want to subscribe to the UpdateNewsBulletinCallBack</param>
        public void ReqNewsBulletin(bool allMessages, Action<object, UpdateNewsBulletinArgs> updateNewsBulletinSubscribers = null)
        {
            if (updateNewsBulletinSubscribers != null)
            {
                Receiver.UpdateNewsBulletinSubscribe(updateNewsBulletinSubscribers);
            }
            ClientSocket.reqNewsBulletins(allMessages);
        }

        /// <summary>
        /// Requests all open orders places by this specific API client (identified by the API client id). For client ID 0, this will bind previous manual TWS orders.
        /// Returns at IBReceiver::openOrder, IBReceiver::orderStatus, IBReceiver::openOrderEnd.
        /// </summary>
        /// <param name="openOrderSubscribers">A delegate containing all functions you want to subscribe to the OpenOrderCallBack</param>
        /// <param name="orderStatusSubscribers">A delegate containing all functions you want to subscribe to the OrderStatusCallBack</param>
        /// <param name="openOrderEndSubscribers">A delegate containing all functions you want to subscribe to the OpenOrderEndCallBack</param>
        public void ReqOpenOrders(Action<object, OpenOrderArgs> openOrderSubscribers = null, Action<object, OrderStatusArgs> orderStatusSubscribers = null, Action<object, OpenOrderEndArgs> openOrderEndSubscribers = null)
        {
            if (openOrderSubscribers != null)
            {
                Receiver.OpenOrderSubscribe(openOrderSubscribers);
            }
            if (orderStatusSubscribers != null)
            {
                Receiver.OrderStatusSubscribe(orderStatusSubscribers);
            }
            if (openOrderEndSubscribers != null)
            {
                Receiver.OpenOrderEndSubscribe(openOrderEndSubscribers);
            }

            ClientSocket.reqOpenOrders();
        }

        /// <summary>
        /// Subscribes to position updates for all accessible accounts. All positions sent initially, and then only updates as positions change.
        /// Data Returned at IBReceiver::position, IBReceiver::positionEnd 
        /// </summary>
        /// <param name="positionSubscribers">A delegate containing all functions you want to subscribe to the PositionCallBack</param>
        /// <param name="positionEndSubscribers">A delegate containing all functions you want to subscribe to the PositionEndCallBack</param>
        public void ReqPositions(Action<object, PositionArgs> positionSubscribers = null, Action<object, PositionEndArgs> positionEndSubscribers = null)
        {
            if (positionSubscribers != null)
            {
                Receiver.PositionSubscribe(positionSubscribers);
            }
            if (positionEndSubscribers != null)
            {
                Receiver.PositionEndSubscribe(positionEndSubscribers);
            }
            ClientSocket.reqPositions();
        }

        /// <summary>
        ///Requests real time bars
        ///Currently, only 5 seconds bars are provided.This request is subject to the same pacing as any historical data request:
        ///no more than 60 API queries in more than 600 seconds.
        ///Real time bars subscriptions are also included in the calculation of the number of
        ///Level 1 market data subscriptions allowed in an account.
        ///Returns Data at IBReceiver::realTimeBar.
        /// </summary>
        /// <param name="contract">	the Contract for which the depth is being requested</param>
        /// <param name="barSize">currently being ignored</param>
        /// <param name="whatToShow">the nature of the data being retrieved: "TRADES", "MIDPOINT", "BID" or "ASK"</param>
        /// <param name="useRegularTradingHours">false to obtain the data which was also generated ourside of the Regular Trading Hours, true to obtain only the RTH data</param>
        /// <param name="realTimeBarsOptions"></param>
        /// <param name="realtimeBarSubscribers">A delegate containing all functions you want to subscribe to the RealTimeBarsCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by the realTimeBarCallBack</returns>
        public RealTimeBarsToken ReqRealTimeBars(Contract contract, int barSize, string whatToShow, bool useRegularTradingHours, List<TagValue> realTimeBarsOptions = null, Action<object, RealtimeBarArgs> realtimeBarSubscribers = null)
        {
            RealTimeBarsToken token = new RealTimeBarsToken();
            if (realtimeBarSubscribers != null)
            {
                Receiver.RealtimeBarSubscribe(token, realtimeBarSubscribers);
            }
            ClientSocket.reqRealTimeBars(token.ID, contract, barSize, whatToShow, useRegularTradingHours, realTimeBarsOptions);
            //TODO subscribe
            return token;
        }

        /// <summary>
        /// Requests an XML list of scanner parameters valid in TWS. all parameters are valid from API scanner.
        /// Returned at IBReceiver::scannerParameter.
        /// </summary>
        /// <param name="ScannerParametersSubscribers"> delegate containing all functions you want to subscribe to the scannerParametersCallBack</param>
        public void ReqScannerParameters(Action<object, ScannerParametersArgs> ScannerParametersSubscribers = null)
        {
            if (ScannerParametersSubscribers != null)
            {
                Receiver.ScannerParametersSubscribe(ScannerParametersSubscribers);
            }
            ClientSocket.reqScannerParameters();
        }

        /// <summary>
        /// Starts a subscription to market scan results based on the provided parameters.
        /// </summary>
        /// <param name="subscription">	summary of the scanner subscription including its filters.</param>
        /// <param name="scannerSubscriptionOptions"></param>
        /// <param name="scannerDataSubscribers">delegate containing all functions you want to subscribe to the scannerDataCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by the scannerDataCallBack</returns>
        public ScannerSubscriptionToken ReqScannerSubscription(ScannerSubscription subscription, List<TagValue> scannerSubscriptionOptions = null, Action<object, ScannerDataArgs> scannerDataSubscribers = null)
        {
            ScannerSubscriptionToken token = new ScannerSubscriptionToken();
            if (scannerDataSubscribers != null)
            {
                Receiver.ScannerDataSubscribe(token,scannerDataSubscribers);
            }
            ClientSocket.reqScannerSubscription(token.ID, subscription, scannerSubscriptionOptions);
            return token;
        }

        /// <summary>
        ///Changes the TWS/GW log level. The default is 2 = ERROR, 5 = DETAIL is required for capturing all API messages and troubleshooting API programs. 
        /// </summary>
        /// <param name="logLevel">Valid values are: 1 = SYSTEM, 2 = ERROR, 3 = WARNING, 4 = INFORMATION, 5 = DETAIL</param>
        public void SetServerLogLevel(int logLevel)
        {
            ClientSocket.setServerLogLevel(logLevel);
        }

        /// <summary>
        /// Requests all available Display Groups in TWS.
        /// Returned at IBReceiver::displayGroupList.
        /// </summary>
        /// <param name="displayGroupListSubscribers">delegate containing all functions you want to subscribe to the displayGroupListCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by the displayGroupListCallBack</returns>
        public QueryDisplayGroupsToken QueryDisplayGroups(Action<object, DisplayGroupListArgs> displayGroupListSubscribers = null)
        {
            QueryDisplayGroupsToken token = new QueryDisplayGroupsToken();
            if (displayGroupListSubscribers != null)
            {
                Receiver.DisplayGroupListSubscribe(token, displayGroupListSubscribers);
            }
            ClientSocket.queryDisplayGroups(token.ID);
            return token;
        }

        /// <summary>
        /// Integrates API client and TWS window grouping.
        /// Returned at IBReceiver::displayGroupUpdated.
        /// </summary>
        /// <param name="groupID">the display group for integration</param>
        /// <param name="displayGroupUpdatedSubscribers">delegate containing all functions you want to subscribe to the displayGroupUpdatedCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by the displayGroupUpdatedCallBack</returns>
        public subscribeToGroupEventsToken SubscribeToGroupEvents(int groupID, Action<object, DisplayGroupUpdatedArgs> displayGroupUpdatedSubscribers = null)
        {
            subscribeToGroupEventsToken token = new subscribeToGroupEventsToken();
            if (displayGroupUpdatedSubscribers != null)
            {
                Receiver.DisplayGroupUpdatedSubscribe(token, displayGroupUpdatedSubscribers);
            }
            ClientSocket.subscribeToGroupEvents(token.ID, groupID);
            return token;
        }

        /// <summary>
        /// Updates the contract displayed in a TWS Window Group.
        /// Does not return except in case of error.
        /// </summary>
        /// <param name="contractInfo">is an encoded value designating a unique IB contract. Possible values include:
        /// none = empty selection,
        /// contractID - any non-combination contract.Examples 8314 for IBM SMART; 8314 for IBM ARCA, 
        ///combo= if any combo is selected Note: This request from the API does not get a TWS response unless an error occurs.
        ///</param>
        /// <returns>Not necessary except in case of error</returns>
        public UpdateDisplayGroupToken UpdateDisplayGroup(string contractInfo)
        {
            UpdateDisplayGroupToken token = new UpdateDisplayGroupToken();
            ClientSocket.updateDisplayGroup(token.ID, contractInfo);
            return token;
        }

        /// <summary>
        /// Cancels a TWS Window Group subscription. 
        /// </summary>
        /// <param name="token">token returned from original subscription</param>
        public void UnsubscribeFromGroupEvents(subscribeToGroupEventsToken token)
        {
            ClientSocket.unsubscribeFromGroupEvents(token.ID);
        }

        /// <summary>
        /// Requests position subscription for account and/or model Initially all positions are returned, 
        /// and then updates are returned for any position changes in real time.
        /// Returned at IBReceiver::positionMulti, IBReceiver::positionMultiEnd
        /// </summary>
        /// <param name="account">If an account Id is provided, only the account's positions belonging to the specified model will be delivered</param>
        /// <param name="modelCode"> The code of the model's positions we are interested in.</param>
        /// <param name="positionMultiSubscribers">delegate containing all functions you want to subscribe to the positionMultiCallBack</param>
        /// <param name="positionMultiEndSubscribers">delegate containing all functions you want to subscribe to the positionMultiEndCallBack</param>
        /// <returns>Use this token to Subscribe a function to any of the CallBacks</returns>
        public PositionsMultiToken ReqPositionsMulti(string account, string modelCode, Action<object, PositionMultiArgs> positionMultiSubscribers = null, Action<object, PositionMultiEndArgs> positionMultiEndSubscribers = null)
        {
            PositionsMultiToken token = new PositionsMultiToken();
            if (positionMultiSubscribers != null)
            {
                Receiver.PositionMultiSubscribe(token, positionMultiSubscribers);
            }
            if (positionMultiEndSubscribers != null)
            {
                Receiver.PositionMultiEndSubscribe(token, positionMultiEndSubscribers);
            }
            ClientSocket.reqPositionsMulti(token.ID, account, modelCode);
            return token;
        }

        /// <summary>
        /// Cancels positions request for account and/or model.
        /// </summary>
        /// <param name="token">token returned from original request</param>
        public void CancelPositionsMulti(PositionsMultiToken token)
        {
            ClientSocket.cancelPositionsMulti(token.ID);
        }

        /// <summary>
        /// Requests account updates for account and/or model.
        /// Returned at IBReceiver::accountUpdateMulti, IBReceiver::accountUpdateMultiEnd.
        /// </summary>
        /// <param name="account">	account values can be requested for a particular account</param>
        /// <param name="modelCode">values can also be requested for a model</param>
        /// <param name="ledgerAndNLV">true to return light-weight request; only currency positions as opposed to account values and current positions</param>
        /// <param name="accountUpdateMultiSubscribers">delegate containing all functions you want to subscribe to the AccountUpdateMultiCallBack</param>
        /// <param name="AccountUpdateMultiEndSubscribers">delegate containing all functions you want to subscribe to the AccountUpdateMultiEndCallBack</param>
        /// <returns>Use this token to Subscribe a function to any of the CallBacks</returns>
        public AccountUpdatesMultiToken ReqAccountUpdatesMulti(string account, string modelCode, bool ledgerAndNLV, Action<object, AccountUpdateMultiArgs> accountUpdateMultiSubscribers = null, Action<object, AccountUpdateMultiEndArgs> accountUpdateMultiEndSubscribers = null)
        {
            AccountUpdatesMultiToken token = new AccountUpdatesMultiToken();
            if (accountUpdateMultiSubscribers != null)
            {
                Receiver.AccountUpdateMultiSubscribe(token, accountUpdateMultiSubscribers);
            }
            if (accountUpdateMultiEndSubscribers != null)
            {
                Receiver.AccountUpdateMultiEndSubscribe(token, accountUpdateMultiEndSubscribers);
            }
            ClientSocket.reqAccountUpdatesMulti(token.ID, account, modelCode, ledgerAndNLV);
            return token;
        }

        /// <summary>
        /// Cancels account updates request for account and/or model.
        /// </summary>
        /// <param name="token">token returned from original request</param>
        public void CancelAccountUpdatesMulti(AccountUpdatesMultiToken token)
        {
            ClientSocket.cancelAccountUpdatesMulti(token.ID);
        }

        /// <summary>
        /// Requests security definition option parameters for viewing a contract's option chain.
        /// </summary>
        /// <param name="underlyingSymbol"></param>
        /// <param name="futFopExchange">The exchange on which the returned options are trading. Can be set to the empty string "" for all exchanges.</param>
        /// <param name="underlyingSecType">The type of the underlying security, i.e. STK</param>
        /// <param name="underlyingConId">	the contract ID of the underlying security</param>
        /// <param name="SecurityDefinitionOptionParameterSubscribers">delegate containing all functions you want to subscribe to the SecurityDefinitionOptionParametersCallBack</param>
        /// <returns>Use this token to Subscribe a function to any of the CallBacks</returns>
        public SecDefOptParamsToken ReqSeqDefOptParams(string underlyingSymbol, string futFopExchange, string underlyingSecType, int underlyingConId, Action<object, SecurityDefinitionOptionParameterArgs> SecurityDefinitionOptionParameterSubscribers = null)
        {
            SecDefOptParamsToken token = new SecDefOptParamsToken();
            if (SecurityDefinitionOptionParameterSubscribers != null)
            {
                Receiver.SecurityDefinitionOptionParameterSubscribe(token, SecurityDefinitionOptionParameterSubscribers);
            }
            ClientSocket.reqSecDefOptParams(token.ID, underlyingSymbol, futFopExchange, underlyingSecType, underlyingConId);
            return token;
        }

        /// <summary>
        /// Requests pre-defined Soft Dollar Tiers. This is only supported for registered professional advisors and hedge 
        /// and mutual funds who have configured Soft Dollar Tiers in Account Management. 
        /// Refer to: https://www.interactivebrokers.com/en/software/am/am/manageaccount/requestsoftdollars.htm?Highlight=soft%20dollar%20tier.
        /// </summary>
        /// <param name="SoftDollarTiersSubscribers">delegate containing all functions you want to subscribe to the SoftDollarTiersCallBack</param>
        /// <returns>Use this token to Subscribe a function to be called by the softDollarTiersCallBack</returns>
        public SoftDollarTiersToken ReqSoftDollarTiers(Action<object, SoftDollarTiersArgs> softDollarTiersSubscribers = null)
        {
            SoftDollarTiersToken token = new SoftDollarTiersToken();
            if (softDollarTiersSubscribers != null)
            {
                Receiver.SoftDollarTiersSubscribe(token, softDollarTiersSubscribers);
            }
            ClientSocket.reqSoftDollarTiers(token.ID);
            return token;
        }
        
    }
}
