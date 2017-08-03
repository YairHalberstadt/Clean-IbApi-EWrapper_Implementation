/* Copyright (C) 2015 Interactive Brokers LLC. All rights reserved.  This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EWrapperImpl
{
    public class EWrapperImpl : EWrapper
    {

        /*Port is usually 7496 for TWS, 7497 for paper Trading/demo account on TWS,
            4001 for IBGateway, 4002 for paper Trading account on IBGateway. The port can be
            changed on Settings.
            You can have up to 9 seperate connections to IB at a time by using different 
            ClientId's.*/
        public EWrapperImpl()
        {
            Signal = new EReaderMonitorSignal();
            ClientSocket = new EClientSocket(this, Signal);
        }

        public void ConnectToIB(int port, int clientId)

        {
            ClientSocket.eConnect("", port, clientId);


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

        public EClientSocket ClientSocket { get; set; }
        public readonly EReaderSignal Signal;

        public int NextOrderId { get { return _NextOrderId++; } private set { _NextOrderId = value; } }
        private int _NextOrderId = 0;

        public event EventHandler<Exception> ExceptionError = delegate { };
        public virtual void error(Exception e)
        {
            ExceptionError?.Invoke(this, e);
        }

        public event EventHandler<string> StringError = delegate { };
        public virtual void error(string str)
        {
            StringError?.Invoke(this, str);
        }

        public event EventHandler<ID_StringErrorArgs> ID_StringError = delegate { };
        public virtual void error(int id, int errorCode, string errorMsg)
        {
            ID_StringError?.Invoke(this, new ID_StringErrorArgs(id, errorCode, errorMsg));

            if (errorCode == 103)
            {
                this.ClientSocket.reqIds(0);
            }
        }

        public event EventHandler<long> CurrentTime = delegate { };
        public virtual void currentTime(long time)
        {
            CurrentTime?.Invoke(this, time);
        }

        public int NextTickerId { get { return _NextTickerId++; } }
        private int _NextTickerId = 0;

        public event EventHandler<TickPriceArgs> TickPrice = delegate { };
        public virtual void tickPrice(int tickerId, int field, double price, int canAutoExecute)
        {
            TickPrice?.Invoke(this, new TickPriceArgs(tickerId, field, price, canAutoExecute));
        }

        public event EventHandler<TickSizeArgs> TickSize = delegate { };
        public virtual void tickSize(int tickerId, int field, int size)
        {
            TickSize?.Invoke(this, new TickSizeArgs(tickerId, field, size));
        }

        public event EventHandler<TickStringArgs> TickString = delegate { };
        public virtual void tickString(int tickerId, int field, string value)
        {
            TickString?.Invoke(this, new TickStringArgs(tickerId, field, value));
        }

        public event EventHandler<TickGenericArgs> TickGeneric = delegate { };
        public virtual void tickGeneric(int tickerId, int field, double value)
        {
            TickGeneric?.Invoke(this, new TickGenericArgs(tickerId, field, value));
        }

        public event EventHandler<TickEfpArgs> TickEFP = delegate { };
        public virtual void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double impliedFuture, int holdDays, string futureLastTradeDate, double dividendImpact, double dividendsToLastTradeDate)
        {
            TickEFP?.Invoke(this, new TickEfpArgs(tickerId, tickType, basisPoints, formattedBasisPoints, impliedFuture, holdDays, futureLastTradeDate, dividendImpact, dividendsToLastTradeDate));
        }

        public int NextReqId { get { return _NextReqId++; } }
        private int _NextReqId = 0;

        public event EventHandler<DeltaNeutralValidationArgs> DeltaNeutralValidation = delegate { };
        public virtual void deltaNeutralValidation(int reqId, UnderComp underComp)
        {
            DeltaNeutralValidation?.Invoke(this, new DeltaNeutralValidationArgs(reqId, underComp));
        }

        public event EventHandler<TickOptionComputationArgs> TickOptionComputation = delegate { };
        public virtual void tickOptionComputation(int tickerId, int field, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {
            TickOptionComputation?.Invoke(this, new TickOptionComputationArgs(tickerId, field, impliedVolatility, delta, optPrice, pvDividend, gamma, vega, theta, undPrice));
        }

        public event EventHandler<TickSnapshotEndArgs> TickSnapshotEnd = delegate { };
        public virtual void tickSnapshotEnd(int tickerId)
        {
            TickSnapshotEnd?.Invoke(this, new TickSnapshotEndArgs(tickerId));
        }

        public event EventHandler<NextValidIdArgs> NextValidId = delegate { };
        public virtual void nextValidId(int orderId)
        {
            NextOrderId = orderId;
            NextValidId?.Invoke(this, new NextValidIdArgs(orderId));
        }

        public event EventHandler<ManagedAccountsArgs> ManagedAccounts = delegate { };
        public virtual void managedAccounts(string accountsList)
        {
            ManagedAccounts?.Invoke(this, new ManagedAccountsArgs(accountsList));
        }

        public event EventHandler<ConnectionClosedArgs> ConnectionClosed = delegate { };
        public virtual void connectionClosed()
        {
            ConnectionClosed?.Invoke(this, new ConnectionClosedArgs());
        }

        public event EventHandler<AccountSummaryArgs> AccountSummary = delegate { };
        public virtual void accountSummary(int reqId, string account, string tag, string value, string currency)
        {
            AccountSummary?.Invoke(this, new AccountSummaryArgs(reqId, account, tag, value, currency));
        }

        public event EventHandler<AccountSummaryEndArgs> AccountSummaryEnd = delegate { };
        public virtual void accountSummaryEnd(int reqId)
        {
            AccountSummaryEnd?.Invoke(this, new AccountSummaryEndArgs(reqId));
        }

        public event EventHandler<BondContractDetailsArgs> BondContractDetails = delegate { };
        public virtual void bondContractDetails(int reqId, ContractDetails contract)
        {
            BondContractDetails?.Invoke(this, new BondContractDetailsArgs(reqId, contract));
        }

        public event EventHandler<UpdateAccountValueArgs> UpdateAccountValue = delegate { };
        public virtual void updateAccountValue(string key, string value, string currency, string accountName)
        {
            UpdateAccountValue?.Invoke(this, new UpdateAccountValueArgs(key, value, currency, accountName));
        }

        public event EventHandler<UpdatePortfolioArgs> UpdatePortfolio = delegate { };
        public virtual void updatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealisedPNL, double realisedPNL, string accountName)
        {
            UpdatePortfolio?.Invoke(this, new UpdatePortfolioArgs(contract, position, marketPrice, marketValue, averageCost, unrealisedPNL, realisedPNL, accountName));
        }

        public event EventHandler<UpdateAccountTimeArgs> UpdateAccountTime = delegate { };
        public virtual void updateAccountTime(string timestamp)
        {
            UpdateAccountTime?.Invoke(this, new UpdateAccountTimeArgs(timestamp));
        }

        public event EventHandler<AccountDownloadEndArgs> AccountDownloadEnd = delegate { };
        public virtual void accountDownloadEnd(string account)
        {
            AccountDownloadEnd?.Invoke(this, new AccountDownloadEndArgs(account));
        }

        public event EventHandler<OrderStatusArgs> OrderStatus = delegate { };
        public virtual void orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld)
        {
            OrderStatus?.Invoke(this, new OrderStatusArgs(orderId, status, filled, remaining, avgFillPrice, permId, parentId, lastFillPrice, clientId, whyHeld));
        }

        public event EventHandler<OpenOrderArgs> OpenOrder = delegate { };
        public virtual void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            OpenOrder?.Invoke(this, new OpenOrderArgs(orderId, contract, order, orderState));
        }

        public event EventHandler<OpenOrderEndArgs> OpenOrderEnd = delegate { };
        public virtual void openOrderEnd()
        {
            OpenOrderEnd?.Invoke(this, new OpenOrderEndArgs());
        }

        public event EventHandler<ContractDetailsArgs> ContractDetails = delegate { };
        public virtual void contractDetails(int reqId, ContractDetails contractDetails)
        {
            ContractDetails?.Invoke(this, new ContractDetailsArgs(reqId, contractDetails));
        }

        public event EventHandler<ContractDetailsEndArgs> ContractDetailsEnd = delegate { };
        public virtual void contractDetailsEnd(int reqId)
        {
            ContractDetailsEnd?.Invoke(this, new ContractDetailsEndArgs(reqId));
        }

        public event EventHandler<ExecDetailsArgs> ExecDetails = delegate { };
        public virtual void execDetails(int reqId, Contract contract, Execution execution)
        {
            ExecDetails?.Invoke(this, new ExecDetailsArgs(reqId, contract, execution));
        }

        public event EventHandler<ExecDetailsEndArgs> ExecDetailsEnd = delegate { };
        public virtual void execDetailsEnd(int reqId)
        {
            ExecDetailsEnd?.Invoke(this, new ExecDetailsEndArgs(reqId));
        }

        public event EventHandler<CommissionReportArgs> CommissionReport = delegate { };
        public virtual void commissionReport(CommissionReport commissionReport)
        {
            CommissionReport?.Invoke(this, new CommissionReportArgs(commissionReport));
        }

        public event EventHandler<FundamentalDataArgs> FundamentalData = delegate { };
        public virtual void fundamentalData(int reqId, string data)
        {
            FundamentalData?.Invoke(this, new FundamentalDataArgs(reqId, data));
        }

        public event EventHandler<HistoricalDataArgs> HistoricalData = delegate { };
        public virtual void historicalData(int reqId, string date, double open, double high, double low, double close, int volume, int count, double WAP, bool hasGaps)
        {
            HistoricalData?.Invoke(this, new HistoricalDataArgs(reqId, date, open, high, low, close, volume, count, WAP, hasGaps));
        }

        public event EventHandler<HistoricalDataEndArgs> HistoricalDataEnd = delegate { };
        public virtual void historicalDataEnd(int reqId, string start, string end)
        {
            HistoricalDataEnd?.Invoke(this, new HistoricalDataEndArgs(reqId, start, end));
        }

        public event EventHandler<MarketDataTypeArgs> MarketDataType = delegate { };
        public virtual void marketDataType(int reqId, int marketDataType)
        {
            MarketDataType?.Invoke(this, new MarketDataTypeArgs(reqId, marketDataType));
        }

        public event EventHandler<UpdateMktDepthArgs> UpdateMktDepth = delegate { };
        public virtual void updateMktDepth(int tickerId, int position, int operation, int side, double price, int size)
        {
            UpdateMktDepth?.Invoke(this, new UpdateMktDepthArgs(tickerId, position, operation, side, price, size));
        }

        public event EventHandler<UpdateMktDepthL2Args> UpdateMktDepthL2 = delegate { };
        public virtual void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, int size)
        {
            UpdateMktDepthL2?.Invoke(this, new UpdateMktDepthL2Args(tickerId, position, marketMaker, operation, side, price, size));
        }

        public int NextMsgId { get { return _NextMsgId++; } }
        private int _NextMsgId = 0;

        public event EventHandler<UpdateNewsBulletinArgs> UpdateNewsBulletin = delegate { };
        public virtual void updateNewsBulletin(int msgId, int msgType, string message, string origExchange)
        {
            UpdateNewsBulletin?.Invoke(this, new UpdateNewsBulletinArgs(msgId, msgType, message, origExchange));
        }

        public event EventHandler<PositionArgs> Position = delegate { };
        public virtual void position(string account, Contract contract, double pos, double avgCost)
        {
            Position?.Invoke(this, new PositionArgs(account, contract, pos, avgCost));
        }

        public event EventHandler<PositionEndArgs> PositionEnd = delegate { };
        public virtual void positionEnd()
        {
            PositionEnd?.Invoke(this, new PositionEndArgs());
        }

        public event EventHandler<RealtimeBarArgs> RealtimeBar = delegate { };
        public virtual void realtimeBar(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
            RealtimeBar?.Invoke(this, new RealtimeBarArgs(reqId, time, open, high, low, close, volume, WAP, count));
        }

        public event EventHandler<ScannerParametersArgs> ScannerParameters = delegate { };
        public virtual void scannerParameters(string xml)
        {
            ScannerParameters?.Invoke(this, new ScannerParametersArgs(xml));
        }

        public event EventHandler<ScannerDataArgs> ScannerData = delegate { };
        public virtual void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
        {
            ScannerData?.Invoke(this, new ScannerDataArgs(reqId, rank, contractDetails, distance, benchmark, projection, legsStr));
        }

        public event EventHandler<ScannerDataEndArgs> ScannerDataEnd = delegate { };
        public virtual void scannerDataEnd(int reqId)
        {
            ScannerDataEnd?.Invoke(this, new ScannerDataEndArgs(reqId));
        }

        public event EventHandler<ReceiveFAArgs> ReceiveFA = delegate { };
        public virtual void receiveFA(int faDataType, string faXmlData)
        {
            ReceiveFA?.Invoke(this, new ReceiveFAArgs(faDataType, faXmlData));
        }

        public event EventHandler<VerifyMessageAPIArgs> VerifyMessageAPI = delegate { };
        public virtual void verifyMessageAPI(string apiData)
        {
            VerifyMessageAPI?.Invoke(this, new VerifyMessageAPIArgs(apiData));
        }

        public event EventHandler<VerifyCompletedArgs> VerifyCompleted = delegate { };
        public virtual void verifyCompleted(bool isSuccessful, string errorText)
        {
            VerifyCompleted?.Invoke(this, new VerifyCompletedArgs(isSuccessful, errorText));
        }

        public event EventHandler<VerifyAndAuthMessageAPIArgs> VerifyAndAuthMessageAPI = delegate { };
        public virtual void verifyAndAuthMessageAPI(string apiData, string xyzChallenge)
        {
            VerifyAndAuthMessageAPI?.Invoke(this, new VerifyAndAuthMessageAPIArgs(apiData, xyzChallenge));
        }

        public event EventHandler<VerifyAndAuthCompletedArgs> VerifyAndAuthCompleted = delegate { };
        public virtual void verifyAndAuthCompleted(bool isSuccessful, string errorText)
        {
            VerifyAndAuthCompleted?.Invoke(this, new VerifyAndAuthCompletedArgs(isSuccessful, errorText));
        }

        public event EventHandler<DisplayGroupListArgs> DisplayGroupList = delegate { };
        public virtual void displayGroupList(int reqId, string groups)
        {
            DisplayGroupList?.Invoke(this, new DisplayGroupListArgs(reqId, groups));
        }

        public event EventHandler<DisplayGroupUpdatedArgs> DisplayGroupUpdated = delegate { };
        public virtual void displayGroupUpdated(int reqId, string contractInfo)
        {
            DisplayGroupUpdated?.Invoke(this, new DisplayGroupUpdatedArgs(reqId, contractInfo));
        }

        public event EventHandler<ConnectAckArgs> ConnectAck = delegate { };
        public void connectAck()
        {
            ConnectAck?.Invoke(this, new ConnectAckArgs());
        }

        public event EventHandler<PositionMultiArgs> PositionMulti = delegate { };
        public void positionMulti(int requestId, string account, string modelCode, Contract contract, double pos, double avgCost)
        {
            PositionMulti?.Invoke(this, new PositionMultiArgs(requestId, account, modelCode, contract, pos, avgCost));
        }

        public event EventHandler<PositionMultiEndArgs> PositionMultiEnd = delegate { };
        public void positionMultiEnd(int requestId)
        {
            PositionMultiEnd?.Invoke(this, new PositionMultiEndArgs(requestId));
        }

        public event EventHandler<AccountUpdateMultiArgs> AccountUpdateMulti = delegate { };
        public void accountUpdateMulti(int requestId, string account, string modelCode, string key, string value, string currency)
        {
            AccountUpdateMulti?.Invoke(this, new AccountUpdateMultiArgs(requestId, account, modelCode, key, value, currency));
        }

        public event EventHandler<AccountUpdateMultiEndArgs> AccountUpdateMultiEnd = delegate { };
        public void accountUpdateMultiEnd(int requestId)
        {
            AccountUpdateMultiEnd?.Invoke(this, new AccountUpdateMultiEndArgs(requestId));
        }

        public event EventHandler<SecurityDefinitionOptionParameterArgs> SecurityDefinitionOptionParameter = delegate { };
        public void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
        {
            SecurityDefinitionOptionParameter?.Invoke(this, new SecurityDefinitionOptionParameterArgs(reqId, exchange, underlyingConId, tradingClass, multiplier, expirations, strikes));
        }

        public event EventHandler<SecurityDefinitionOptionParameterEndArgs> SecurityDefinitionOptionParameterEnd = delegate { };
        public void securityDefinitionOptionParameterEnd(int reqId)
        {
            SecurityDefinitionOptionParameterEnd?.Invoke(this, new SecurityDefinitionOptionParameterEndArgs(reqId));
        }

        public event EventHandler<SoftDollarTiersArgs> SoftDollarTiers = delegate { };
        public void softDollarTiers(int reqId, SoftDollarTier[] tiers)
        {
            SoftDollarTiers?.Invoke(this, new SoftDollarTiersArgs(reqId, tiers));
        }
    }
}

