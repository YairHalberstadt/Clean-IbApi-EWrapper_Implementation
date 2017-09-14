/* Copyright (C) 2015 Interactive Brokers LLC. All rights reserved.  This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using IBApi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace EWrapperImpl
{
    public class IBReceiver : EWrapper
    {
        private IBWrapper Wrapper { get; }

        internal IBReceiver(IBWrapper wrapper)
        {
            Wrapper = wrapper;
        }

        object ExceptionErrorLock = new object();
        List<Action<object, Exception>> ExceptionErrorSubscribersList = new List<Action<object, Exception>>();
        public void ErrorSubscribe(Action<object, Exception> action)
        {
            lock (ExceptionErrorLock)
            {
                ExceptionErrorSubscribersList.Add(action);
            }
        }
        public bool ExceptionErrorUnSubscribe(Action<object, Exception> action)
        {
            lock (ExceptionErrorLock)
            {
                return ExceptionErrorSubscribersList.Remove(action);
            }
        }
        public virtual void error(Exception e)
        {
            Wrapper.ResetEvent.Set();
            lock (ExceptionErrorLock)
            {
                foreach (Action<object, Exception> action in ExceptionErrorSubscribersList)
                {
                    action(Wrapper, e);
                }
            }
        }

        object StringErrorLock = new object();
        List<Action<object, string>> StringErrorSubscribersList = new List<Action<object, string>>();
        public void StringErrorSubscribe(Action<object, string> action)
        {
            lock (StringErrorLock)
            {
                StringErrorSubscribersList.Add(action);
            }
        }
        public bool StringErrorUnSubscribe(Action<object, string> action)
        {
            lock (StringErrorLock)
            {
                return StringErrorSubscribersList.Remove(action);
            }
        }
        public virtual void error(string str)
        {
            Wrapper.ResetEvent.Set();
            lock (StringErrorLock)
            {
                foreach (Action<object, string> action in StringErrorSubscribersList)
                {
                    action(Wrapper, str);
                }
            }
        }

        object IdAndStringErrrorLock = new object();
        List<Action<object, IdAndStringErrorArgs>> IdAndStringErrorSubscribersList = new List<Action<object, IdAndStringErrorArgs>>();
        public void IdAndStringErrorSubscribe(Action<object, IdAndStringErrorArgs> action)
        {
            lock (IdAndStringErrrorLock)
            {
                IdAndStringErrorSubscribersList.Add(action);
            }
        }
        public bool IdAndStringErrorUnSubscribe(Action<object, IdAndStringErrorArgs> action)
        {
            lock (IdAndStringErrrorLock)
            {
                return IdAndStringErrorSubscribersList.Remove(action);
            }
        }
        public virtual void error(int id, int errorCode, string errorMsg)
        {
            Wrapper.ResetEvent.Set();
            lock (IdAndStringErrrorLock)
            {
                foreach (Action<object, IdAndStringErrorArgs> action in IdAndStringErrorSubscribersList)
                {
                    action(Wrapper, new IdAndStringErrorArgs(id, errorCode, errorMsg));
                }

                if (errorCode == 103)
                {
                    Wrapper.Requester.ReqIds();
                }
            }
        }
        
        object CurrentTimeLock = new object();
        List<Action<object, CurrentTimeArgs>> CurrentTimeSubscribersList = new List<Action<object, CurrentTimeArgs>>();
        public void CurrentTimeSubscribe(Action<object, CurrentTimeArgs> action)
        {
            lock (CurrentTimeLock)
            {
                CurrentTimeSubscribersList.Add(action);
            }
        }
        public bool CurrentTimeUnSubscribe(Action<object, CurrentTimeArgs> action)
        {
            lock (CurrentTimeLock)
            {
                return CurrentTimeSubscribersList.Remove(action);
            }
        }
        public virtual void currentTime(long time)
        {
            lock (CurrentTimeLock)
            {
                foreach (Action<object, CurrentTimeArgs> action in CurrentTimeSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new CurrentTimeArgs(time), action.EndInvoke, null);
                }
            }
        }

        object TickPriceLock = new object();
        List<TokenActionPair<MktDataToken, TickPriceArgs>> TickPriceSubscribersList = new List<TokenActionPair<MktDataToken, TickPriceArgs>>();
        ILookup<MktDataToken, Action<object, TickPriceArgs>> TickPriceSubscribersLookup;
        public void TickPriceSubscribe(MktDataToken token, Action<object, TickPriceArgs> action)
        {
            lock (TickPriceLock)
            {
                TickPriceSubscribersList.Add(new TokenActionPair<MktDataToken, TickPriceArgs>(token, action));
                TickPriceSubscribersLookup = TickPriceSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool TickPriceUnSubscribe(MktDataToken token, Action<object, TickPriceArgs> action)
        {
            lock (TickPriceLock)
            {
                bool Exists = TickPriceSubscribersList.Remove(new TokenActionPair<MktDataToken, TickPriceArgs>(token, action));
                if (Exists)
                {
                    TickPriceSubscribersLookup = TickPriceSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void tickPrice(int tickerId, int field, double price, int canAutoExecute)
        {
            var time = DateTime.Now;
            lock (TickPriceLock)
            {
                if (TickPriceSubscribersLookup != null)
                {
                    MktDataToken token = new MktDataToken(tickerId);


                    foreach (Action<object, TickPriceArgs> action in TickPriceSubscribersLookup[token])
                    {
                        action.BeginInvoke(Wrapper, new TickPriceArgs(tickerId, field, price, canAutoExecute,time), action.EndInvoke, null);
                    }
                }
            }
        }

        object TickSizeLock = new object();
        List<TokenActionPair<MktDataToken, TickSizeArgs>> TickSizeSubscribersList = new List<TokenActionPair<MktDataToken, TickSizeArgs>>();
        ILookup<MktDataToken, Action<object, TickSizeArgs>> TickSizeSubscribersLookup;
        public void TickSizeSubscribe(MktDataToken token, Action<object, TickSizeArgs> action)
        {
            lock (TickSizeLock)
            {
                TickSizeSubscribersList.Add(new TokenActionPair<MktDataToken, TickSizeArgs>(token, action));
                TickSizeSubscribersLookup = TickSizeSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool TickSizeUnSubscribe(MktDataToken token, Action<object, TickSizeArgs> action)
        {
            lock (TickSizeLock)
            {
                bool Exists = TickSizeSubscribersList.Remove(new TokenActionPair<MktDataToken, TickSizeArgs>(token, action));
                if (Exists)
                {
                    TickSizeSubscribersLookup = TickSizeSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void tickSize(int tickerId, int field, int size)
        {
            lock (TickSizeLock)
            {
                if (TickSizeSubscribersLookup != null)
                {
                    MktDataToken token = new MktDataToken(tickerId);

                    var subscribers = TickSizeSubscribersLookup[token];
                    foreach (Action<object, TickSizeArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new TickSizeArgs(tickerId, field, size), action.EndInvoke, null);
                    }
                }
            }
        }

        object TickStringLock = new object();
        List<TokenActionPair<MktDataToken, TickStringArgs>> TickStringSubscribersList = new List<TokenActionPair<MktDataToken, TickStringArgs>>();
        ILookup<MktDataToken, Action<object, TickStringArgs>> TickStringSubscribersLookup;
        public void TickStringSubscribe(MktDataToken token, Action<object, TickStringArgs> action)
        {
            lock (TickStringLock)
            {
                TickStringSubscribersList.Add(new TokenActionPair<MktDataToken, TickStringArgs>(token, action));
                TickStringSubscribersLookup = TickStringSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool TickStringUnSubscribe(MktDataToken token, Action<object, TickStringArgs> action)
        {
            lock (TickStringLock)
            {
                bool Exists = TickStringSubscribersList.Remove(new TokenActionPair<MktDataToken, TickStringArgs>(token, action));
                if (Exists)
                {
                    TickStringSubscribersLookup = TickStringSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void tickString(int tickerId, int field, string value)
        {
            lock (TickStringLock)
            {
                if (TickStringSubscribersLookup != null)
                {
                    MktDataToken token = new MktDataToken(tickerId);

                    var subscribers = TickStringSubscribersLookup[token];
                    foreach (Action<object, TickStringArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new TickStringArgs(tickerId, field, value), action.EndInvoke, null);
                    }
                }
            }
        }

        object TickGenericLock = new object();
        List<TokenActionPair<MktDataToken, TickGenericArgs>> TickGenericSubscribersList = new List<TokenActionPair<MktDataToken, TickGenericArgs>>();
        ILookup<MktDataToken, Action<object, TickGenericArgs>> TickGenericSubscribersLookup;
        public void TickGenericSubscribe(MktDataToken token, Action<object, TickGenericArgs> action)
        {
            lock (TickGenericLock)
            {
                TickGenericSubscribersList.Add(new TokenActionPair<MktDataToken, TickGenericArgs>(token, action));
                TickGenericSubscribersLookup = TickGenericSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool TickGenericUnSubscribe(MktDataToken token, Action<object, TickGenericArgs> action)
        {
            lock (TickGenericLock)
            {
                bool Exists = TickGenericSubscribersList.Remove(new TokenActionPair<MktDataToken, TickGenericArgs>(token, action));
                if (Exists)
                {
                    TickGenericSubscribersLookup = TickGenericSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void tickGeneric(int tickerId, int field, double value)
        {
            lock (TickGenericLock)
            {
                if (TickGenericSubscribersLookup != null)
                {
                    MktDataToken token = new MktDataToken(tickerId);

                    var subscribers = TickGenericSubscribersLookup[token];
                    foreach (Action<object, TickGenericArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new TickGenericArgs(tickerId, field, value), action.EndInvoke, null);
                    }
                }
            }
        }

        object TickEfpLock = new object();
        List<TokenActionPair<MktDataToken, TickEfpArgs>> TickEfpSubscribersList = new List<TokenActionPair<MktDataToken, TickEfpArgs>>();
        ILookup<MktDataToken, Action<object, TickEfpArgs>> TickEfpSubscribersLookup;
        public void TickEfpSubscribe(MktDataToken token, Action<object, TickEfpArgs> action)
        {
            lock (TickEfpLock)
            {
                TickEfpSubscribersList.Add(new TokenActionPair<MktDataToken, TickEfpArgs>(token, action));
                TickEfpSubscribersLookup = TickEfpSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool TickEfpUnSubscribe(MktDataToken token, Action<object, TickEfpArgs> action)
        {
            lock (TickEfpLock)
            {
                bool Exists = TickEfpSubscribersList.Remove(new TokenActionPair<MktDataToken, TickEfpArgs>(token, action));
                if (Exists)
                {
                    TickEfpSubscribersLookup = TickEfpSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double impliedFuture, int holdDays, string futureLastTradeDate, double dividendImpact, double dividendsToLastTradeDate)
        {
            lock (TickEfpLock)
            {
                if (TickEfpSubscribersLookup != null)
                {
                    MktDataToken token = new MktDataToken(tickerId);

                    var subscribers = TickEfpSubscribersLookup[token];
                    foreach (Action<object, TickEfpArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new TickEfpArgs(tickerId, tickType, basisPoints, formattedBasisPoints, impliedFuture, holdDays, futureLastTradeDate, dividendImpact, dividendsToLastTradeDate), action.EndInvoke, null);
                    }
                }
            }
        }

        object DeltaNeutralValidationLock = new object();
        List<Action<object, DeltaNeutralValidationArgs>> DeltaNeutralValidationSubscribersList = new List<Action<object, DeltaNeutralValidationArgs>>();
        public void DeltaNeutralValidationSubscribe(Action<object, DeltaNeutralValidationArgs> action)
        {
            lock (DeltaNeutralValidationLock)
            {
                DeltaNeutralValidationSubscribersList.Add(action);
            }
        }
        public bool DeltaNeutralValidationUnSubscribe(Action<object, DeltaNeutralValidationArgs> action)
        {
            lock (DeltaNeutralValidationLock)
            {
                return DeltaNeutralValidationSubscribersList.Remove(action);
            }
        }
        public virtual void deltaNeutralValidation(int reqId, UnderComp underComp)
        {
            lock (DeltaNeutralValidationLock)
            {
                foreach (Action<object, DeltaNeutralValidationArgs> action in DeltaNeutralValidationSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new DeltaNeutralValidationArgs(reqId, underComp), action.EndInvoke, null);
                }
            }
        }

        object TickOptionComputationLock = new object();
        List<TokenActionPair<TickOptionComputationToken, TickOptionComputationArgs>> TickOptionComputationSubscribersList = new List<TokenActionPair<TickOptionComputationToken, TickOptionComputationArgs>>();
        ILookup<TickOptionComputationToken, Action<object, TickOptionComputationArgs>> TickOptionComputationSubscribersLookup;
        public void TickOptionComputationSubscribe(TickOptionComputationToken token, Action<object, TickOptionComputationArgs> action)
        {
            lock (TickOptionComputationLock)
            {
                TickOptionComputationSubscribersList.Add(new TokenActionPair<TickOptionComputationToken, TickOptionComputationArgs>(token, action));
                TickOptionComputationSubscribersLookup = TickOptionComputationSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool TickOptionComputationUnSubscribe(TickOptionComputationToken token, Action<object, TickOptionComputationArgs> action)
        {
            lock (TickOptionComputationLock)
            {
                bool Exists = TickOptionComputationSubscribersList.Remove(new TokenActionPair<TickOptionComputationToken, TickOptionComputationArgs>(token, action));
                if (Exists)
                {
                    TickOptionComputationSubscribersLookup = TickOptionComputationSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void tickOptionComputation(int tickerId, int field, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {
            lock (TickOptionComputationLock)
            {
                if (TickOptionComputationSubscribersLookup != null)
                {
                    TickOptionComputationToken token = new TickOptionComputationToken(tickerId);

                    var subscribers = TickOptionComputationSubscribersLookup[token];
                    foreach (Action<object, TickOptionComputationArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new TickOptionComputationArgs(tickerId, field, impliedVolatility, delta, optPrice, pvDividend, gamma, vega, theta, undPrice), action.EndInvoke, null);
                    }
                }
            }
        }

        object TickSnapshotEndLock = new object();
        List<TokenActionPair<MktDataToken, TickSnapshotEndArgs>> TickSnapshotEndSubscribersList = new List<TokenActionPair<MktDataToken, TickSnapshotEndArgs>>();
        ILookup<MktDataToken, Action<object, TickSnapshotEndArgs>> TickSnapshotEndSubscribersLookup;
        public void TickSnapshotEndSubscribe(MktDataToken token, Action<object, TickSnapshotEndArgs> action)
        {
            lock (TickSnapshotEndLock)
            {
                TickSnapshotEndSubscribersList.Add(new TokenActionPair<MktDataToken, TickSnapshotEndArgs>(token, action));
                TickSnapshotEndSubscribersLookup = TickSnapshotEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool TickSnapshotEndUnSubscribe(MktDataToken token, Action<object, TickSnapshotEndArgs> action)
        {
            lock (TickSnapshotEndLock)
            {
                bool Exists = TickSnapshotEndSubscribersList.Remove(new TokenActionPair<MktDataToken, TickSnapshotEndArgs>(token, action));
                if (Exists)
                {
                    TickSnapshotEndSubscribersLookup = TickSnapshotEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void tickSnapshotEnd(int tickerId)
        {
            lock (TickSnapshotEndLock)
            {
                if (TickSnapshotEndSubscribersLookup != null)
                {
                    MktDataToken token = new MktDataToken(tickerId);

                    var subscribers = TickSnapshotEndSubscribersLookup[token];
                    foreach (Action<object, TickSnapshotEndArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new TickSnapshotEndArgs(tickerId), action.EndInvoke, null);
                    }
                }
            }
        }

        object NextValidIdLock = new object();
        List<Action<object,int>> NextValidIdSubscribersList = new List<Action<object,int>>();
        public void NextValidIdSubscribe(Action<object,int> action)
        {
            lock (NextValidIdLock)
            {
                NextValidIdSubscribersList.Add(action);
            }
        }
        public bool TickSizeUnSubscribe(Action<object,int> action)
        {
            lock (NextValidIdLock)
            {
                return NextValidIdSubscribersList.Remove(action);
            }
        }
        public virtual void nextValidId(int orderId)
        {
            lock (NextValidIdLock)
            {
                Wrapper.Requester.NextOrderId = orderId;

                foreach (Action<object, int> action in NextValidIdSubscribersList)
                {
                    action.BeginInvoke(Wrapper, orderId,null,null);
                }
            }
        }

        object ManagedAccountsLock = new object();
        List<Action<object, ManagedAccountsArgs>> ManagedAccountsSubscribersList = new List<Action<object, ManagedAccountsArgs>>();
        public void ManagedAccountsSubscribe(Action<object, ManagedAccountsArgs> action)
        {
            lock (ManagedAccountsLock)
            {
                ManagedAccountsSubscribersList.Add(action);
            }
        }
        public bool ManagedAccountsUnSubscribe(Action<object, ManagedAccountsArgs> action)
        {
            lock (ManagedAccountsLock)
            {
                return ManagedAccountsSubscribersList.Remove(action);
            }
        }
        public virtual void managedAccounts(string accountsList)
        {
            lock (ManagedAccountsLock)
            {
                foreach (Action<object, ManagedAccountsArgs> action in ManagedAccountsSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new ManagedAccountsArgs(accountsList), action.EndInvoke, null);
                }
            }
        }

        object ConnectionClosedLock = new object();
        List<Action<object, ConnectionClosedArgs>> ConnectionClosedSubscribersList = new List<Action<object, ConnectionClosedArgs>>();
        public void ConnectionClosedSubscribe(Action<object, ConnectionClosedArgs> action)
        {
            lock (ConnectionClosedLock)
            {
                ConnectionClosedSubscribersList.Add(action);
            }
        }
        public bool ConnectionClosedUnSubscribe(Action<object, ConnectionClosedArgs> action)
        {
            lock (ConnectionClosedLock)
            {
                return ConnectionClosedSubscribersList.Remove(action);
            }
        }
        public virtual void connectionClosed()
        {
            lock (ConnectionClosedLock)
            {
                foreach (Action<object, ConnectionClosedArgs> action in ConnectionClosedSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new ConnectionClosedArgs(), action.EndInvoke, null);
                }
            }
        }

        object AccountSummaryLock = new object();
        List<TokenActionPair<AccountSummaryToken, AccountSummaryArgs>> AccountSummarySubscribersList = new List<TokenActionPair<AccountSummaryToken, AccountSummaryArgs>>();
        ILookup<AccountSummaryToken, Action<object, AccountSummaryArgs>> AccountSummarySubscribersLookup;
        public void AccountSummarySubscribe(AccountSummaryToken token, Action<object, AccountSummaryArgs> action)
        {
            lock (AccountSummaryLock)
            {
                AccountSummarySubscribersList.Add(new TokenActionPair<AccountSummaryToken, AccountSummaryArgs>(token, action));
                AccountSummarySubscribersLookup = AccountSummarySubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool AccountSummaryUnSubscribe(AccountSummaryToken token, Action<object, AccountSummaryArgs> action)
        {
            lock (AccountSummaryLock)
            {
                bool Exists = AccountSummarySubscribersList.Remove(new TokenActionPair<AccountSummaryToken, AccountSummaryArgs>(token, action));
                if (Exists)
                {
                    AccountSummarySubscribersLookup = AccountSummarySubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void accountSummary(int reqId, string account, string tag, string value, string currency)
        {
            lock (AccountSummaryLock)
            {
                if (AccountSummarySubscribersLookup != null)
                {
                    AccountSummaryToken token = new AccountSummaryToken(reqId);

                    var subscribers = AccountSummarySubscribersLookup[token];
                    foreach (Action<object, AccountSummaryArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new AccountSummaryArgs(reqId, account, tag, value, currency), action.EndInvoke, null);
                    }
                }
            }
        }

        object AccountSummaryEndLock = new object();
        List<TokenActionPair<AccountSummaryToken, AccountSummaryEndArgs>> AccountSummaryEndSubscribersList = new List<TokenActionPair<AccountSummaryToken, AccountSummaryEndArgs>>();
        ILookup<AccountSummaryToken, Action<object, AccountSummaryEndArgs>> AccountSummaryEndSubscribersLookup;
        public void AccountSummaryEndSubscribe(AccountSummaryToken token, Action<object, AccountSummaryEndArgs> action)
        {
            lock (AccountSummaryEndLock)
            {
                AccountSummaryEndSubscribersList.Add(new TokenActionPair<AccountSummaryToken, AccountSummaryEndArgs>(token, action));
                AccountSummaryEndSubscribersLookup = AccountSummaryEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool AccountSummaryEndUnSubscribe(AccountSummaryToken token, Action<object, AccountSummaryEndArgs> action)
        {
            lock (AccountSummaryEndLock)
            {
                bool Exists = AccountSummaryEndSubscribersList.Remove(new TokenActionPair<AccountSummaryToken, AccountSummaryEndArgs>(token, action));
                if (Exists)
                {
                    AccountSummaryEndSubscribersLookup = AccountSummaryEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void accountSummaryEnd(int reqId)
        {
            lock (AccountSummaryEndLock)
            {
                if (AccountSummaryEndSubscribersLookup != null)
                {
                    AccountSummaryToken token = new AccountSummaryToken(reqId);

                    var subscribers = AccountSummaryEndSubscribersLookup[token];
                    foreach (Action<object, AccountSummaryEndArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new AccountSummaryEndArgs(reqId), action.EndInvoke, null);
                    }
                }
            }
        }

        object BondContractDetailsLock = new object();
        List<TokenActionPair<ContractDetailsToken, BondContractDetailsArgs>> BondContractDetailsSubscribersList = new List<TokenActionPair<ContractDetailsToken, BondContractDetailsArgs>>();
        ILookup<ContractDetailsToken, Action<object, BondContractDetailsArgs>> BondContractDetailsSubscribersLookup;
        public void BondContractDetailsSubscribe(ContractDetailsToken token, Action<object, BondContractDetailsArgs> action)
        {
            lock (BondContractDetailsLock)
            {
                BondContractDetailsSubscribersList.Add(new TokenActionPair<ContractDetailsToken, BondContractDetailsArgs>(token, action));
                BondContractDetailsSubscribersLookup = BondContractDetailsSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool BondContractDetailsUnSubscribe(ContractDetailsToken token, Action<object, BondContractDetailsArgs> action)
        {
            lock (BondContractDetailsLock)
            {
                bool Exists = BondContractDetailsSubscribersList.Remove(new TokenActionPair<ContractDetailsToken, BondContractDetailsArgs>(token, action));
                if (Exists)
                {
                    BondContractDetailsSubscribersLookup = BondContractDetailsSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void bondContractDetails(int reqId, ContractDetails contract)
        {
            lock (BondContractDetailsLock)
            {
                if (BondContractDetailsSubscribersLookup != null)
                {
                    ContractDetailsToken token = new ContractDetailsToken(reqId);

                    var subscribers = BondContractDetailsSubscribersLookup[token];
                    foreach (Action<object, BondContractDetailsArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new BondContractDetailsArgs(reqId, contract), action.EndInvoke, null);
                    }
                }
            }
        }

        object UpdateAccountValueLock = new object();
        List<Action<object, UpdateAccountValueArgs>> UpdateAccountValueSubscribersList = new List<Action<object, UpdateAccountValueArgs>>();
        public void UpdateAccountValueSubscribe(Action<object, UpdateAccountValueArgs> action)
        {
            lock (UpdateAccountValueLock)
            {
                UpdateAccountValueSubscribersList.Add(action);
            }
        }
        public bool UpdateAccountValueUnSubscribe(Action<object, UpdateAccountValueArgs> action)
        {
            lock (UpdateAccountValueLock)
            {
                return UpdateAccountValueSubscribersList.Remove(action);
            }
        }
        public virtual void updateAccountValue(string key, string value, string currency, string accountName)
        {
            lock (UpdateAccountValueLock)
            {
                foreach (Action<object, UpdateAccountValueArgs> action in UpdateAccountValueSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new UpdateAccountValueArgs(key, value, currency, accountName), action.EndInvoke, null);
                }
            }
        }

        object UpdatePortfolioLock = new object();
        List<Action<object, UpdatePortfolioArgs>> UpdatePortfolioSubscribersList = new List<Action<object, UpdatePortfolioArgs>>();
        public void UpdatePortfolioSubscribe(Action<object, UpdatePortfolioArgs> action)
        {
            lock (UpdatePortfolioLock)
            {
                UpdatePortfolioSubscribersList.Add(action);
            }
        }
        public bool UpdatePortfolioUnSubscribe(Action<object, UpdatePortfolioArgs> action)
        {
            lock (UpdatePortfolioLock)
            {
                return UpdatePortfolioSubscribersList.Remove(action);
            }
        }
        public virtual void updatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealisedPNL, double realisedPNL, string accountName)
        {
            lock (UpdatePortfolioLock)
            {
                foreach (Action<object, UpdatePortfolioArgs> action in UpdatePortfolioSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new UpdatePortfolioArgs(contract, position, marketPrice, marketValue, averageCost, unrealisedPNL, realisedPNL, accountName), action.EndInvoke, null);
                }
            }
        }

        object UpdateAccountTimeLock = new object();
        List<Action<object, UpdateAccountTimeArgs>> UpdateAccountTimeSubscribersList = new List<Action<object, UpdateAccountTimeArgs>>();
        public void UpdateAccountTimeSubscribe(Action<object, UpdateAccountTimeArgs> action)
        {
            lock (UpdateAccountTimeLock)
            {
                UpdateAccountTimeSubscribersList.Add(action);
            }
        }
        public bool UpdateAccountTimeUnSubscribe(Action<object, UpdateAccountTimeArgs> action)
        {
            lock (UpdateAccountTimeLock)
            {
                return UpdateAccountTimeSubscribersList.Remove(action);
            }
        }
        public virtual void updateAccountTime(string timestamp)
        {
            lock (UpdateAccountTimeLock)
            {
                foreach (Action<object, UpdateAccountTimeArgs> action in UpdateAccountTimeSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new UpdateAccountTimeArgs(timestamp), action.EndInvoke, null);
                }
            }
        }

        object AccountDownloadEndLock = new object();
        List<Action<object, AccountDownloadEndArgs>> AccountDownloadEndSubscribersList = new List<Action<object, AccountDownloadEndArgs>>();
        public void AccountDownloadEndSubscribe(Action<object, AccountDownloadEndArgs> action)
        {
            lock (AccountDownloadEndLock)
            {
                AccountDownloadEndSubscribersList.Add(action);
            }
        }
        public bool AccountDownloadEndUnSubscribe(Action<object, AccountDownloadEndArgs> action)
        {
            lock (AccountDownloadEndLock)
            {
                return AccountDownloadEndSubscribersList.Remove(action);
            }
        }
        public virtual void accountDownloadEnd(string account)
        {
            lock (AccountDownloadEndLock)
            {
                foreach (Action<object, AccountDownloadEndArgs> action in AccountDownloadEndSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new AccountDownloadEndArgs(account), action.EndInvoke, null);
                }
            }
        }

        object OrderStatusLock = new object();
        List<Action<object, OrderStatusArgs>> OrderStatusSubscribersList = new List<Action<object, OrderStatusArgs>>();
        public void OrderStatusSubscribe(Action<object, OrderStatusArgs> action)
        {
            lock (OrderStatusLock)
            {
                OrderStatusSubscribersList.Add(action);
            }
        }
        public bool OrderStatusUnSubscribe(Action<object, OrderStatusArgs> action)
        {
            lock (OrderStatusLock)
            {
                return OrderStatusSubscribersList.Remove(action);
            }
        }
        public virtual void orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld)
        {
            lock (OrderStatusLock)
            {
                foreach (Action<object, OrderStatusArgs> action in OrderStatusSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new OrderStatusArgs(orderId, status, filled, remaining, avgFillPrice, permId, parentId, lastFillPrice, clientId, whyHeld), action.EndInvoke, null);
                }
            }
        }

        object OpenOrderLock = new object();
        List<Action<object, OpenOrderArgs>> OpenOrderSubscribersList = new List<Action<object, OpenOrderArgs>>();
        public void OpenOrderSubscribe(Action<object, OpenOrderArgs> action)
        {
            lock (OpenOrderLock)
            {
                OpenOrderSubscribersList.Add(action);
            }
        }
        public bool OpenOrderUnSubscribe(Action<object, OpenOrderArgs> action)
        {
            lock (OpenOrderLock)
            {
                return OpenOrderSubscribersList.Remove(action);
            }
        }
        public virtual void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            lock (OpenOrderLock)
            {
                foreach (Action<object, OpenOrderArgs> action in OpenOrderSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new OpenOrderArgs(orderId, contract, order, orderState), action.EndInvoke, null);
                }
            }
        }

        object OpenOrderEndLock = new object();
        List<Action<object, OpenOrderEndArgs>> OpenOrderEndSubscribersList = new List<Action<object, OpenOrderEndArgs>>();
        public void OpenOrderEndSubscribe(Action<object, OpenOrderEndArgs> action)
        {
            lock (OpenOrderEndLock)
            {
                OpenOrderEndSubscribersList.Add(action);
            }
        }
        public bool OpenOrderEndUnSubscribe(Action<object, OpenOrderEndArgs> action)
        {
            lock (OpenOrderEndLock)
            {
                return OpenOrderEndSubscribersList.Remove(action);
            }
        }
        public virtual void openOrderEnd()
        {
            lock (OpenOrderEndLock)
            {
                foreach (Action<object, OpenOrderEndArgs> action in OpenOrderEndSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new OpenOrderEndArgs(), action.EndInvoke, null);
                }
            }
        }

        object ContractDetailsLock = new object();
        List<TokenActionPair<ContractDetailsToken, ContractDetailsArgs>> ContractDetailsSubscribersList = new List<TokenActionPair<ContractDetailsToken, ContractDetailsArgs>>();
        ILookup<ContractDetailsToken, Action<object, ContractDetailsArgs>> ContractDetailsSubscribersLookup;
        public void ContractDetailsSubscribe(ContractDetailsToken token, Action<object, ContractDetailsArgs> action)
        {
            lock (ContractDetailsLock)
            {
                ContractDetailsSubscribersList.Add(new TokenActionPair<ContractDetailsToken, ContractDetailsArgs>(token, action));
                ContractDetailsSubscribersLookup = ContractDetailsSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool ContractDetailsUnSubscribe(ContractDetailsToken token, Action<object, ContractDetailsArgs> action)
        {
            lock (ContractDetailsLock)
            {
                bool Exists = ContractDetailsSubscribersList.Remove(new TokenActionPair<ContractDetailsToken, ContractDetailsArgs>(token, action));
                if (Exists)
                {
                    ContractDetailsSubscribersLookup = ContractDetailsSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void contractDetails(int reqId, ContractDetails contractDetails)
        {
            lock (ContractDetailsLock)
            {
                if (ContractDetailsSubscribersLookup != null)
                {
                    ContractDetailsToken token = new ContractDetailsToken(reqId);

                    var subscribers = ContractDetailsSubscribersLookup[token];
                    foreach (Action<object, ContractDetailsArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new ContractDetailsArgs(reqId, contractDetails), action.EndInvoke, null);
                    }
                }
            }
        }

        object ContractDetailsEndLock = new object();
        List<TokenActionPair<ContractDetailsToken, ContractDetailsEndArgs>> ContractDetailsEndSubscribersList = new List<TokenActionPair<ContractDetailsToken, ContractDetailsEndArgs>>();
        ILookup<ContractDetailsToken, Action<object, ContractDetailsEndArgs>> ContractDetailsEndSubscribersLookup;
        public void ContractDetailsEndSubscribe(ContractDetailsToken token, Action<object, ContractDetailsEndArgs> action)
        {
            lock (ContractDetailsEndLock)
            {
                ContractDetailsEndSubscribersList.Add(new TokenActionPair<ContractDetailsToken, ContractDetailsEndArgs>(token, action));
                ContractDetailsEndSubscribersLookup = ContractDetailsEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool ContractDetailsEndUnSubscribe(ContractDetailsToken token, Action<object, ContractDetailsEndArgs> action)
        {
            lock (ContractDetailsEndLock)
            {
                bool Exists = ContractDetailsEndSubscribersList.Remove(new TokenActionPair<ContractDetailsToken, ContractDetailsEndArgs>(token, action));
                if (Exists)
                {
                    ContractDetailsEndSubscribersLookup = ContractDetailsEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void contractDetailsEnd(int reqId)
        {
            lock (ContractDetailsEndLock)
            {
                if (ContractDetailsEndSubscribersLookup != null)
                {
                    ContractDetailsToken token = new ContractDetailsToken(reqId);

                    var subscribers = ContractDetailsEndSubscribersLookup[token];
                    foreach (Action<object, ContractDetailsEndArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new ContractDetailsEndArgs(reqId), action.EndInvoke, null);
                    }
                }
            }
        }

        object ExecDetailsLock = new object();
        List<TokenActionPair<ExecutionsToken, ExecDetailsArgs>> ExecDetailsSubscribersList = new List<TokenActionPair<ExecutionsToken, ExecDetailsArgs>>();
        ILookup<ExecutionsToken, Action<object, ExecDetailsArgs>> ExecDetailsSubscribersLookup;
        public void ExecDetailsSubscribe(ExecutionsToken token, Action<object, ExecDetailsArgs> action)
        {
            lock (ExecDetailsLock)
            {
                ExecDetailsSubscribersList.Add(new TokenActionPair<ExecutionsToken, ExecDetailsArgs>(token, action));
                ExecDetailsSubscribersLookup = ExecDetailsSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool ExecDetailsUnSubscribe(ExecutionsToken token, Action<object, ExecDetailsArgs> action)
        {
            lock (ExecDetailsLock)
            {
                bool Exists = ExecDetailsSubscribersList.Remove(new TokenActionPair<ExecutionsToken, ExecDetailsArgs>(token, action));
                if (Exists)
                {
                    ExecDetailsSubscribersLookup = ExecDetailsSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void execDetails(int reqId, Contract contract, Execution execution)
        {
            lock (ExecDetailsLock)
            {
                if (ExecDetailsSubscribersLookup != null)
                {
                    ExecutionsToken token = new ExecutionsToken(reqId);

                    var subscribers = ExecDetailsSubscribersLookup[token];
                    foreach (Action<object, ExecDetailsArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new ExecDetailsArgs(reqId, contract, execution), action.EndInvoke, null);
                    }
                }
            }
        }

        object ExecDetailsEndLock = new object();
        List<TokenActionPair<ExecutionsToken, ExecDetailsEndArgs>> ExecDetailsEndSubscribersList = new List<TokenActionPair<ExecutionsToken, ExecDetailsEndArgs>>();
        ILookup<ExecutionsToken, Action<object, ExecDetailsEndArgs>> ExecDetailsEndSubscribersLookup;
        public void ExecDetailsEndSubscribe(ExecutionsToken token, Action<object, ExecDetailsEndArgs> action)
        {
            lock (ExecDetailsEndLock)
            {
                ExecDetailsEndSubscribersList.Add(new TokenActionPair<ExecutionsToken, ExecDetailsEndArgs>(token, action));
                ExecDetailsEndSubscribersLookup = ExecDetailsEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool ExecDetailsEndUnSubscribe(ExecutionsToken token, Action<object, ExecDetailsEndArgs> action)
        {
            lock (ExecDetailsEndLock)
            {
                bool Exists = ExecDetailsEndSubscribersList.Remove(new TokenActionPair<ExecutionsToken, ExecDetailsEndArgs>(token, action));
                if (Exists)
                {
                    ExecDetailsEndSubscribersLookup = ExecDetailsEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void execDetailsEnd(int reqId)
        {
            lock (ExecDetailsEndLock)
            {
                if (ExecDetailsEndSubscribersLookup != null)
                {
                    ExecutionsToken token = new ExecutionsToken(reqId);

                    var subscribers = ExecDetailsEndSubscribersLookup[token];
                    foreach (Action<object, ExecDetailsEndArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new ExecDetailsEndArgs(reqId), action.EndInvoke, null);
                    }
                }
            }
        }

        object CommissionReportLock = new object();
        List<Action<object, CommissionReportArgs>> CommissionReportSubscribersList = new List<Action<object, CommissionReportArgs>>();
        public void CommissionReportSubscribe(Action<object, CommissionReportArgs> action)
        {
            lock (CommissionReportLock)
            {
                CommissionReportSubscribersList.Add(action);
            }
        }
        public bool CommissionReportUnSubscribe(Action<object, CommissionReportArgs> action)
        {
            lock (CommissionReportLock)
            {
                return CommissionReportSubscribersList.Remove(action);
            }
        }
        public virtual void commissionReport(CommissionReport commissionReport)
        {
            lock (CommissionReportLock)
            {
                foreach (Action<object, CommissionReportArgs> action in CommissionReportSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new CommissionReportArgs(commissionReport), action.EndInvoke, null);
                }
            }
        }

        object FundamentalDataLock = new object();
        List<TokenActionPair<FundamentalDataToken, FundamentalDataArgs>> FundamentalDataSubscribersList = new List<TokenActionPair<FundamentalDataToken, FundamentalDataArgs>>();
        ILookup<FundamentalDataToken, Action<object, FundamentalDataArgs>> FundamentalDataSubscribersLookup;
        public void FundamentalDataSubscribe(FundamentalDataToken token, Action<object, FundamentalDataArgs> action)
        {
            lock (FundamentalDataLock)
            {
                FundamentalDataSubscribersList.Add(new TokenActionPair<FundamentalDataToken, FundamentalDataArgs>(token, action));
                FundamentalDataSubscribersLookup = FundamentalDataSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool FundamentalDataUnSubscribe(FundamentalDataToken token, Action<object, FundamentalDataArgs> action)
        {
            lock (FundamentalDataLock)
            {
                bool Exists = FundamentalDataSubscribersList.Remove(new TokenActionPair<FundamentalDataToken, FundamentalDataArgs>(token, action));
                if (Exists)
                {
                    FundamentalDataSubscribersLookup = FundamentalDataSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void fundamentalData(int reqId, string data)
        {
            lock (FundamentalDataLock)
            {
                if (FundamentalDataSubscribersLookup != null)
                {
                    FundamentalDataToken token = new FundamentalDataToken(reqId);

                    var subscribers = FundamentalDataSubscribersLookup[token];
                    foreach (Action<object, FundamentalDataArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new FundamentalDataArgs(reqId, data), action.EndInvoke, null);
                    }
                }
            }
        }

        ConcurrentDictionary<HistoricalDataToken, CandleSize> TokenCandlSizeDictionary = new ConcurrentDictionary<HistoricalDataToken, CandleSize>();
        public bool TokenCandleSizeDicionaryAdd(HistoricalDataToken token, CandleSize candleSize)
        {
            return TokenCandlSizeDictionary.TryAdd(token, candleSize);
        }
        object HistoricalDataLock = new object();
        List<TokenActionPair<HistoricalDataToken, HistoricalDataArgs>> HistoricalDataSubscribersList = new List<TokenActionPair<HistoricalDataToken, HistoricalDataArgs>>();
        ILookup<HistoricalDataToken, Action<object, HistoricalDataArgs>> HistoricalDataSubscribersLookup;
        public void HistoricalDataSubscribe(HistoricalDataToken token, Action<object, HistoricalDataArgs> action)
        {
            lock (HistoricalDataLock)
            {
                HistoricalDataSubscribersList.Add(new TokenActionPair<HistoricalDataToken, HistoricalDataArgs>(token, action));
                HistoricalDataSubscribersLookup = HistoricalDataSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool HistoricalDataUnSubscribe(HistoricalDataToken token, Action<object, HistoricalDataArgs> action)
        {
            lock (HistoricalDataLock)
            {
                bool Exists = HistoricalDataSubscribersList.Remove(new TokenActionPair<HistoricalDataToken, HistoricalDataArgs>(token, action));
                if (Exists)
                {
                    HistoricalDataSubscribersLookup = HistoricalDataSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void historicalData(int reqId, string date, double open, double high, double low, double close, int volume, int count, double WAP, bool hasGaps)
        {
            lock (HistoricalDataLock)
            {
                if (HistoricalDataSubscribersLookup != null)
                {
                    HistoricalDataToken token = new HistoricalDataToken(reqId);

                    var subscribers = HistoricalDataSubscribersLookup[token];
                    foreach (Action<object, HistoricalDataArgs> action in subscribers)
                    {
                        DateTime.TryParseExact(date, new string[] { "yyyyMMdd", "yyyyMMdd  HH:mm:ss" }, null, DateTimeStyles.None, out DateTime dateTime);
                        var candleSize = TokenCandlSizeDictionary[token];
                        action.BeginInvoke(Wrapper, new HistoricalDataArgs(token, candleSize,dateTime, open, high, low, close, volume, count, WAP, hasGaps), action.EndInvoke, null);
                    }
                }
            }
        }

        object HistoricalDataEndLock = new object();
        List<TokenActionPair<HistoricalDataToken, HistoricalDataEndArgs>> HistoricalDataEndSubscribersList = new List<TokenActionPair<HistoricalDataToken, HistoricalDataEndArgs>>();
        ILookup<HistoricalDataToken, Action<object, HistoricalDataEndArgs>> HistoricalDataEndSubscribersLookup;
        public void HistoricalDataEndSubscribe(HistoricalDataToken token, Action<object, HistoricalDataEndArgs> action)
        {
            lock (HistoricalDataEndLock)
            {
                HistoricalDataEndSubscribersList.Add(new TokenActionPair<HistoricalDataToken, HistoricalDataEndArgs>(token, action));
                HistoricalDataEndSubscribersLookup = HistoricalDataEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool HistoricalDataEndUnSubscribe(HistoricalDataToken token, Action<object, HistoricalDataEndArgs> action)
        {
            lock (HistoricalDataEndLock)
            {
                bool Exists = HistoricalDataEndSubscribersList.Remove(new TokenActionPair<HistoricalDataToken, HistoricalDataEndArgs>(token, action));
                if (Exists)
                {
                    HistoricalDataEndSubscribersLookup = HistoricalDataEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void historicalDataEnd(int reqId, string start, string end)
        {
            lock (HistoricalDataEndLock)
            {
                if (HistoricalDataEndSubscribersLookup != null)
                {
                    HistoricalDataToken token = new HistoricalDataToken(reqId);

                    var subscribers = HistoricalDataEndSubscribersLookup[token];
                    foreach (Action<object, HistoricalDataEndArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new HistoricalDataEndArgs(reqId, start, end), action.EndInvoke, null);
                    }
                }
            }
        }

        object MarketDataTypeLock = new object();
        List<Action<object, MarketDataTypeArgs>> MarketDataTypeSubscribersList = new List<Action<object, MarketDataTypeArgs>>();
        public void MarketDataTypeSubscribe(Action<object, MarketDataTypeArgs> action)
        {
            lock (MarketDataTypeLock)
            {
                MarketDataTypeSubscribersList.Add(action);
            }
        }
        public bool MarketDataTypeUnSubscribe(Action<object, MarketDataTypeArgs> action)
        {
            lock (MarketDataTypeLock)
            {
                return MarketDataTypeSubscribersList.Remove(action);
            }
        }
        public virtual void marketDataType(int reqId, int marketDataType)
        {
            lock (MarketDataTypeLock)
            {
                foreach (Action<object, MarketDataTypeArgs> action in MarketDataTypeSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new MarketDataTypeArgs(reqId, marketDataType), action.EndInvoke, null);
                }
            }
        }

        object UpdateMktDepthLock = new object();
        List<TokenActionPair<MktDepthToken, UpdateMktDepthArgs>> UpdateMktDepthSubscribersList = new List<TokenActionPair<MktDepthToken, UpdateMktDepthArgs>>();
        ILookup<MktDepthToken, Action<object, UpdateMktDepthArgs>> UpdateMktDepthSubscribersLookup;
        public void UpdateMktDepthSubscribe(MktDepthToken token, Action<object, UpdateMktDepthArgs> action)
        {
            lock (UpdateMktDepthLock)
            {
                UpdateMktDepthSubscribersList.Add(new TokenActionPair<MktDepthToken, UpdateMktDepthArgs>(token, action));
                UpdateMktDepthSubscribersLookup = UpdateMktDepthSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool UpdateMktDepthUnSubscribe(MktDepthToken token, Action<object, UpdateMktDepthArgs> action)
        {
            lock (UpdateMktDepthLock)
            {
                bool Exists = UpdateMktDepthSubscribersList.Remove(new TokenActionPair<MktDepthToken, UpdateMktDepthArgs>(token, action));
                if (Exists)
                {
                    UpdateMktDepthSubscribersLookup = UpdateMktDepthSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void updateMktDepth(int tickerId, int position, int operation, int side, double price, int size)
        {
            lock (UpdateMktDepthLock)
            {
                if (UpdateMktDepthSubscribersLookup != null)
                {
                    MktDepthToken token = new MktDepthToken(tickerId);

                    var subscribers = UpdateMktDepthSubscribersLookup[token];
                    foreach (Action<object, UpdateMktDepthArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new UpdateMktDepthArgs(tickerId, position, operation, side, price, size), action.EndInvoke, null);
                    }
                }
            }
        }

        object UpdateMktDepthL2Lock = new object();
        List<TokenActionPair<MktDepthToken, UpdateMktDepthL2Args>> UpdateMktDepthL2SubscribersList = new List<TokenActionPair<MktDepthToken, UpdateMktDepthL2Args>>();
        ILookup<MktDepthToken, Action<object, UpdateMktDepthL2Args>> UpdateMktDepthL2SubscribersLookup;
        public void UpdateMktDepthL2Subscribe(MktDepthToken token, Action<object, UpdateMktDepthL2Args> action)
        {
            lock (UpdateMktDepthL2Lock)
            {
                UpdateMktDepthL2SubscribersList.Add(new TokenActionPair<MktDepthToken, UpdateMktDepthL2Args>(token, action));
                UpdateMktDepthL2SubscribersLookup = UpdateMktDepthL2SubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool UpdateMktDepthL2UnSubscribe(MktDepthToken token, Action<object, UpdateMktDepthL2Args> action)
        {
            lock (UpdateMktDepthL2Lock)
            {
                bool Exists = UpdateMktDepthL2SubscribersList.Remove(new TokenActionPair<MktDepthToken, UpdateMktDepthL2Args>(token, action));
                if (Exists)
                {
                    UpdateMktDepthL2SubscribersLookup = UpdateMktDepthL2SubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, int size)
        {
            lock (UpdateMktDepthL2Lock)
            {
                if (UpdateMktDepthL2SubscribersLookup != null)
                {
                    MktDepthToken token = new MktDepthToken(tickerId);

                    var subscribers = UpdateMktDepthL2SubscribersLookup[token];
                    foreach (Action<object, UpdateMktDepthL2Args> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new UpdateMktDepthL2Args(tickerId, position, marketMaker, operation, side, price, size), action.EndInvoke, null);
                    }
                }
            }
        }

        object UpdateNewsBulletinLock = new object();
        List<Action<object, UpdateNewsBulletinArgs>> UpdateNewsBulletinSubscribersList = new List<Action<object, UpdateNewsBulletinArgs>>();
        public void UpdateNewsBulletinSubscribe(Action<object, UpdateNewsBulletinArgs> action)
        {
            lock (UpdateNewsBulletinLock)
            {
                UpdateNewsBulletinSubscribersList.Add(action);
            }
        }
        public bool UpdateNewsBulletinUnSubscribe(Action<object, UpdateNewsBulletinArgs> action)
        {
            lock (UpdateNewsBulletinLock)
            {
                return UpdateNewsBulletinSubscribersList.Remove(action);
            }
        }
        public virtual void updateNewsBulletin(int msgId, int msgType, string message, string origExchange)
        {
            lock (UpdateNewsBulletinLock)
            {
                foreach (Action<object, UpdateNewsBulletinArgs> action in UpdateNewsBulletinSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new UpdateNewsBulletinArgs(msgId, msgType, message, origExchange), action.EndInvoke, null);
                }
            }
        }

        object PositionLock = new object();
        List<Action<object, PositionArgs>> PositionSubscribersList = new List<Action<object, PositionArgs>>();
        public void PositionSubscribe(Action<object, PositionArgs> action)
        {
            lock (PositionLock)
            {
                PositionSubscribersList.Add(action);
            }
        }
        public bool PositionUnSubscribe(Action<object, PositionArgs> action)
        {
            lock (PositionLock)
            {
                return PositionSubscribersList.Remove(action);
            }
        }
        public virtual void position(string account, Contract contract, double pos, double avgCost)
        {
            lock (PositionLock)
            {
                foreach (Action<object, PositionArgs> action in PositionSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new PositionArgs(account, contract, pos, avgCost), action.EndInvoke, null);
                }
            }
        }

        object PositionEndLock = new object();
        List<Action<object, PositionEndArgs>> PositionEndSubscribersList = new List<Action<object, PositionEndArgs>>();
        public void PositionEndSubscribe(Action<object, PositionEndArgs> action)
        {
            lock (PositionEndLock)
            {
                PositionEndSubscribersList.Add(action);
            }
        }
        public bool PositionEndUnSubscribe(Action<object, PositionEndArgs> action)
        {
            lock (PositionEndLock)
            {
                return PositionEndSubscribersList.Remove(action);
            }
        }
        public virtual void positionEnd()
        {
            lock (PositionEndLock)
            {
                foreach (Action<object, PositionEndArgs> action in PositionEndSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new PositionEndArgs(), action.EndInvoke, null);
                }
            }
        }

        object RealtimeBarLock = new object();
        List<TokenActionPair<RealTimeBarsToken, RealtimeBarArgs>> RealtimeBarSubscribersList = new List<TokenActionPair<RealTimeBarsToken, RealtimeBarArgs>>();
        ILookup<RealTimeBarsToken, Action<object, RealtimeBarArgs>> RealtimeBarSubscribersLookup;
        public void RealtimeBarSubscribe(RealTimeBarsToken token, Action<object, RealtimeBarArgs> action)
        {
            lock (RealtimeBarLock)
            {
                RealtimeBarSubscribersList.Add(new TokenActionPair<RealTimeBarsToken, RealtimeBarArgs>(token, action));
                RealtimeBarSubscribersLookup = RealtimeBarSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool RealtimeBarUnSubscribe(RealTimeBarsToken token, Action<object, RealtimeBarArgs> action)
        {
            lock (RealtimeBarLock)
            {
                bool Exists = RealtimeBarSubscribersList.Remove(new TokenActionPair<RealTimeBarsToken, RealtimeBarArgs>(token, action));
                if (Exists)
                {
                    RealtimeBarSubscribersLookup = RealtimeBarSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void realtimeBar(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
            lock (RealtimeBarLock)
            {
                if (RealtimeBarSubscribersLookup != null)
                {
                    RealTimeBarsToken token = new RealTimeBarsToken(reqId);

                    var subscribers = RealtimeBarSubscribersLookup[token];
                    foreach (Action<object, RealtimeBarArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new RealtimeBarArgs(reqId, time, open, high, low, close, volume, WAP, count), action.EndInvoke, null);
                    }
                }
            }
        }

        object ScannerParametersLock = new object();
        List<Action<object, ScannerParametersArgs>> ScannerParametersSubscribersList = new List<Action<object, ScannerParametersArgs>>();
        public void ScannerParametersSubscribe(Action<object, ScannerParametersArgs> action)
        {
            lock (ScannerParametersLock)
            {
                ScannerParametersSubscribersList.Add(action);
            }
        }
        public bool ScannerParametersUnSubscribe(Action<object, ScannerParametersArgs> action)
        {
            lock (ScannerParametersLock)
            {
                return ScannerParametersSubscribersList.Remove(action);
            }
        }
        public virtual void scannerParameters(string xml)
        {
            lock (ScannerParametersLock)
            {
                foreach (Action<object, ScannerParametersArgs> action in ScannerParametersSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new ScannerParametersArgs(xml), action.EndInvoke, null);
                }
            }
        }

        object ScannerDataLock = new object();
        List<TokenActionPair<ScannerSubscriptionToken, ScannerDataArgs>> ScannerDataSubscribersList = new List<TokenActionPair<ScannerSubscriptionToken, ScannerDataArgs>>();
        ILookup<ScannerSubscriptionToken, Action<object, ScannerDataArgs>> ScannerDataSubscribersLookup;
        public void ScannerDataSubscribe(ScannerSubscriptionToken token, Action<object, ScannerDataArgs> action)
        {
            lock (ScannerDataLock)
            {
                ScannerDataSubscribersList.Add(new TokenActionPair<ScannerSubscriptionToken, ScannerDataArgs>(token, action));
                ScannerDataSubscribersLookup = ScannerDataSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool ScannerDataUnSubscribe(ScannerSubscriptionToken token, Action<object, ScannerDataArgs> action)
        {
            lock (ScannerDataLock)
            {
                bool Exists = ScannerDataSubscribersList.Remove(new TokenActionPair<ScannerSubscriptionToken, ScannerDataArgs>(token, action));
                if (Exists)
                {
                    ScannerDataSubscribersLookup = ScannerDataSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
        {
            lock (ScannerDataLock)
            {
                if (ScannerDataSubscribersLookup != null)
                {
                    ScannerSubscriptionToken token = new ScannerSubscriptionToken(reqId);

                    var subscribers = ScannerDataSubscribersLookup[token];
                    foreach (Action<object, ScannerDataArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new ScannerDataArgs(reqId, rank, contractDetails, distance, benchmark, projection, legsStr), action.EndInvoke, null);
                    }
                }
            }
        }

        object ScannerDataEndLock = new object();
        List<TokenActionPair<ScannerSubscriptionToken, ScannerDataEndArgs>> ScannerDataEndSubscribersList = new List<TokenActionPair<ScannerSubscriptionToken, ScannerDataEndArgs>>();
        ILookup<ScannerSubscriptionToken, Action<object, ScannerDataEndArgs>> ScannerDataEndSubscribersLookup;
        public void ScannerDataEndSubscribe(ScannerSubscriptionToken token, Action<object, ScannerDataEndArgs> action)
        {
            lock (ScannerDataEndLock)
            {
                ScannerDataEndSubscribersList.Add(new TokenActionPair<ScannerSubscriptionToken, ScannerDataEndArgs>(token, action));
                ScannerDataEndSubscribersLookup = ScannerDataEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool ScannerDataEndUnSubscribe(ScannerSubscriptionToken token, Action<object, ScannerDataEndArgs> action)
        {
            lock (ScannerDataEndLock)
            {
                bool Exists = ScannerDataEndSubscribersList.Remove(new TokenActionPair<ScannerSubscriptionToken, ScannerDataEndArgs>(token, action));
                if (Exists)
                {
                    ScannerDataEndSubscribersLookup = ScannerDataEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void scannerDataEnd(int reqId)
        {
            lock (ScannerDataEndLock)
            {
                if (ScannerDataEndSubscribersLookup != null)
                {
                    ScannerSubscriptionToken token = new ScannerSubscriptionToken(reqId);

                    var subscribers = ScannerDataEndSubscribersLookup[token];
                    foreach (Action<object, ScannerDataEndArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new ScannerDataEndArgs(reqId), action.EndInvoke, null);
                    }
                }
            }
        }

        object ReceiveFALock = new object();
        List<Action<object, ReceiveFAArgs>> ReceiveFASubscribersList = new List<Action<object, ReceiveFAArgs>>();
        public void ReceiveFASubscribe(Action<object, ReceiveFAArgs> action)
        {
            lock (ReceiveFALock)
            {
                ReceiveFASubscribersList.Add(action);
            }
        }
        public bool ReceiveFAUnSubscribe(Action<object, ReceiveFAArgs> action)
        {
            lock (ReceiveFALock)
            {
                return ReceiveFASubscribersList.Remove(action);
            }
        }
        public virtual void receiveFA(int faDataType, string faXmlData)
        {
            lock (ReceiveFALock)
            {
                foreach (Action<object, ReceiveFAArgs> action in ReceiveFASubscribersList)
                {
                    action.BeginInvoke(Wrapper, new ReceiveFAArgs(faDataType, faXmlData), action.EndInvoke, null);
                }
            }
        }

        object VerifyMessageAPILock = new object();
        List<Action<object, VerifyMessageAPIArgs>> VerifyMessageAPISubscribersList = new List<Action<object, VerifyMessageAPIArgs>>();
        public void VerifyMessageAPISubscribe(Action<object, VerifyMessageAPIArgs> action)
        {
            lock (VerifyMessageAPILock)
            {
                VerifyMessageAPISubscribersList.Add(action);
            }
        }
        public bool VerifyMessageAPIUnSubscribe(Action<object, VerifyMessageAPIArgs> action)
        {
            lock (VerifyMessageAPILock)
            {
                return VerifyMessageAPISubscribersList.Remove(action);
            }
        }
        public virtual void verifyMessageAPI(string apiData)
        {
            lock (VerifyMessageAPILock)
            {
                foreach (Action<object, VerifyMessageAPIArgs> action in VerifyMessageAPISubscribersList)
                {
                    action.BeginInvoke(Wrapper, new VerifyMessageAPIArgs(apiData), action.EndInvoke, null);
                }
            }
        }

        object VerifyCompletedLock = new object();
        List<Action<object, VerifyCompletedArgs>> VerifyCompletedSubscribersList = new List<Action<object, VerifyCompletedArgs>>();
        public void VerifyCompletedSubscribe(Action<object, VerifyCompletedArgs> action)
        {
            lock (VerifyCompletedLock)
            {
                VerifyCompletedSubscribersList.Add(action);
            }
        }
        public bool VerifyCompletedUnSubscribe(Action<object, VerifyCompletedArgs> action)
        {
            lock (VerifyCompletedLock)
            {
                return VerifyCompletedSubscribersList.Remove(action);
            }
        }
        public virtual void verifyCompleted(bool isSuccessful, string errorText)
        {
            lock (VerifyCompletedLock)
            {
                foreach (Action<object, VerifyCompletedArgs> action in VerifyCompletedSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new VerifyCompletedArgs(isSuccessful, errorText), action.EndInvoke, null);
                }
            }
        }

        object VerifyAndAuthMessageAPILock = new object();
        List<Action<object, VerifyAndAuthMessageAPIArgs>> VerifyAndAuthMessageAPISubscribersList = new List<Action<object, VerifyAndAuthMessageAPIArgs>>();
        public void VerifyAndAuthMessageAPISubscribe(Action<object, VerifyAndAuthMessageAPIArgs> action)
        {
            lock (VerifyAndAuthMessageAPILock)
            {
                VerifyAndAuthMessageAPISubscribersList.Add(action);
            }
        }
        public bool VerifyAndAuthMessageAPIUnSubscribe(Action<object, VerifyAndAuthMessageAPIArgs> action)
        {
            lock (VerifyAndAuthMessageAPILock)
            {
                return VerifyAndAuthMessageAPISubscribersList.Remove(action);
            }
        }
        public virtual void verifyAndAuthMessageAPI(string apiData, string xyzChallenge)
        {
            lock (VerifyAndAuthMessageAPILock)
            {
                foreach (Action<object, VerifyAndAuthMessageAPIArgs> action in VerifyAndAuthMessageAPISubscribersList)
                {
                    action.BeginInvoke(Wrapper, new VerifyAndAuthMessageAPIArgs(apiData, xyzChallenge), action.EndInvoke, null);
                }
            }
        }

        object VerifyAndAuthCompletedLock = new object();
        List<Action<object, VerifyAndAuthCompletedArgs>> VerifyAndAuthCompletedSubscribersList = new List<Action<object, VerifyAndAuthCompletedArgs>>();
        public void VerifyAndAuthCompletedSubscribe(Action<object, VerifyAndAuthCompletedArgs> action)
        {
            lock (VerifyAndAuthCompletedLock)
            {
                VerifyAndAuthCompletedSubscribersList.Add(action);
            }
        }
        public bool VerifyAndAuthCompletedUnSubscribe(Action<object, VerifyAndAuthCompletedArgs> action)
        {
            lock (VerifyAndAuthCompletedLock)
            {
                return VerifyAndAuthCompletedSubscribersList.Remove(action);
            }
        }
        public virtual void verifyAndAuthCompleted(bool isSuccessful, string errorText)
        {
            lock (VerifyAndAuthCompletedLock)
            {
                foreach (Action<object, VerifyAndAuthCompletedArgs> action in VerifyAndAuthCompletedSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new VerifyAndAuthCompletedArgs(isSuccessful, errorText), action.EndInvoke, null);
                }
            }
        }

        object DisplayGroupListLock = new object();
        List<TokenActionPair<QueryDisplayGroupsToken, DisplayGroupListArgs>> DisplayGroupListSubscribersList = new List<TokenActionPair<QueryDisplayGroupsToken, DisplayGroupListArgs>>();
        ILookup<QueryDisplayGroupsToken, Action<object, DisplayGroupListArgs>> DisplayGroupListSubscribersLookup;
        public void DisplayGroupListSubscribe(QueryDisplayGroupsToken token, Action<object, DisplayGroupListArgs> action)
        {
            lock (DisplayGroupListLock)
            {
                DisplayGroupListSubscribersList.Add(new TokenActionPair<QueryDisplayGroupsToken, DisplayGroupListArgs>(token, action));
                DisplayGroupListSubscribersLookup = DisplayGroupListSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool DisplayGroupListUnSubscribe(QueryDisplayGroupsToken token, Action<object, DisplayGroupListArgs> action)
        {
            lock (DisplayGroupListLock)
            {
                bool Exists = DisplayGroupListSubscribersList.Remove(new TokenActionPair<QueryDisplayGroupsToken, DisplayGroupListArgs>(token, action));
                if (Exists)
                {
                    DisplayGroupListSubscribersLookup = DisplayGroupListSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void displayGroupList(int reqId, string groups)
        {
            lock (DisplayGroupListLock)
            {
                if (DisplayGroupListSubscribersLookup != null)
                {
                    QueryDisplayGroupsToken token = new QueryDisplayGroupsToken(reqId);

                    var subscribers = DisplayGroupListSubscribersLookup[token];
                    foreach (Action<object, DisplayGroupListArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new DisplayGroupListArgs(reqId, groups), action.EndInvoke, null);
                    }
                }
            }
        }

        object DisplayGroupUpdatedLock = new object();
        List<TokenActionPair<subscribeToGroupEventsToken, DisplayGroupUpdatedArgs>> DisplayGroupUpdatedSubscribersList = new List<TokenActionPair<subscribeToGroupEventsToken, DisplayGroupUpdatedArgs>>();
        ILookup<subscribeToGroupEventsToken, Action<object, DisplayGroupUpdatedArgs>> DisplayGroupUpdatedSubscribersLookup;
        public void DisplayGroupUpdatedSubscribe(subscribeToGroupEventsToken token, Action<object, DisplayGroupUpdatedArgs> action)
        {
            lock (DisplayGroupUpdatedLock)
            {
                DisplayGroupUpdatedSubscribersList.Add(new TokenActionPair<subscribeToGroupEventsToken, DisplayGroupUpdatedArgs>(token, action));
                DisplayGroupUpdatedSubscribersLookup = DisplayGroupUpdatedSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool DisplayGroupUpdatedUnSubscribe(subscribeToGroupEventsToken token, Action<object, DisplayGroupUpdatedArgs> action)
        {
            lock (DisplayGroupUpdatedLock)
            {
                bool Exists = DisplayGroupUpdatedSubscribersList.Remove(new TokenActionPair<subscribeToGroupEventsToken, DisplayGroupUpdatedArgs>(token, action));
                if (Exists)
                {
                    DisplayGroupUpdatedSubscribersLookup = DisplayGroupUpdatedSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public virtual void displayGroupUpdated(int reqId, string contractInfo)
        {
            lock (DisplayGroupUpdatedLock)
            {
                if (DisplayGroupUpdatedSubscribersLookup != null)
                {
                    subscribeToGroupEventsToken token = new subscribeToGroupEventsToken(reqId);

                    var subscribers = DisplayGroupUpdatedSubscribersLookup[token];
                    foreach (Action<object, DisplayGroupUpdatedArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new DisplayGroupUpdatedArgs(reqId, contractInfo), action.EndInvoke, null);
                    }
                }
            }
        }

        object ConnectAckLock = new object();
        List<Action<object, ConnectAckArgs>> ConnectAckSubscribersList = new List<Action<object, ConnectAckArgs>>();
        public void ConnectAckSubscribe(Action<object, ConnectAckArgs> action)
        {
            lock (ConnectAckLock)
            {
                ConnectAckSubscribersList.Add(action);
            }
        }
        public bool ConnectAckUnSubscribe(Action<object, ConnectAckArgs> action)
        {
            lock (ConnectAckLock)
            {
                return ConnectAckSubscribersList.Remove(action);
            }
        }
        public void connectAck()
        {
            lock (ConnectAckLock)
            {
                foreach (Action<object, ConnectAckArgs> action in ConnectAckSubscribersList)
                {
                    action.BeginInvoke(Wrapper, new ConnectAckArgs(), action.EndInvoke, null);
                }
            }
        }

        object PositionMultiLock = new object();
        List<TokenActionPair<PositionsMultiToken, PositionMultiArgs>> PositionMultiSubscribersList = new List<TokenActionPair<PositionsMultiToken, PositionMultiArgs>>();
        ILookup<PositionsMultiToken, Action<object, PositionMultiArgs>> PositionMultiSubscribersLookup;
        public void PositionMultiSubscribe(PositionsMultiToken token, Action<object, PositionMultiArgs> action)
        {
            lock (PositionMultiLock)
            {
                PositionMultiSubscribersList.Add(new TokenActionPair<PositionsMultiToken, PositionMultiArgs>(token, action));
                PositionMultiSubscribersLookup = PositionMultiSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool PositionMultiUnSubscribe(PositionsMultiToken token, Action<object, PositionMultiArgs> action)
        {
            lock (PositionMultiLock)
            {
                bool Exists = PositionMultiSubscribersList.Remove(new TokenActionPair<PositionsMultiToken, PositionMultiArgs>(token, action));
                if (Exists)
                {
                    PositionMultiSubscribersLookup = PositionMultiSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public void positionMulti(int requestId, string account, string modelCode, Contract contract, double pos, double avgCost)
        {
            lock (PositionMultiLock)
            {
                if (PositionMultiSubscribersLookup != null)
                {
                    PositionsMultiToken token = new PositionsMultiToken(requestId);

                    var subscribers = PositionMultiSubscribersLookup[token];
                    foreach (Action<object, PositionMultiArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new PositionMultiArgs(requestId, account, modelCode, contract, pos, avgCost), action.EndInvoke, null);
                    }
                }
            }
        }

        object PositionMultiEndLock = new object();
        List<TokenActionPair<PositionsMultiToken, PositionMultiEndArgs>> PositionMultiEndSubscribersList = new List<TokenActionPair<PositionsMultiToken, PositionMultiEndArgs>>();
        ILookup<PositionsMultiToken, Action<object, PositionMultiEndArgs>> PositionMultiEndSubscribersLookup;
        public void PositionMultiEndSubscribe(PositionsMultiToken token, Action<object, PositionMultiEndArgs> action)
        {
            lock (PositionMultiEndLock)
            {
                PositionMultiEndSubscribersList.Add(new TokenActionPair<PositionsMultiToken, PositionMultiEndArgs>(token, action));
                PositionMultiEndSubscribersLookup = PositionMultiEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool PositionMultiEndUnSubscribe(PositionsMultiToken token, Action<object, PositionMultiEndArgs> action)
        {
            lock (PositionMultiEndLock)
            {
                bool Exists = PositionMultiEndSubscribersList.Remove(new TokenActionPair<PositionsMultiToken, PositionMultiEndArgs>(token, action));
                if (Exists)
                {
                    PositionMultiEndSubscribersLookup = PositionMultiEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public void positionMultiEnd(int requestId)
        {
            lock (PositionMultiEndLock)
            {
                if (PositionMultiEndSubscribersLookup != null)
                {
                    PositionsMultiToken token = new PositionsMultiToken(requestId);

                    var subscribers = PositionMultiEndSubscribersLookup[token];
                    foreach (Action<object, PositionMultiEndArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new PositionMultiEndArgs(requestId), action.EndInvoke, null);
                    }
                }
            }
        }

        object AccountUpdateMultiLock = new object();
        List<TokenActionPair<AccountUpdatesMultiToken, AccountUpdateMultiArgs>> AccountUpdateMultiSubscribersList = new List<TokenActionPair<AccountUpdatesMultiToken, AccountUpdateMultiArgs>>();
        ILookup<AccountUpdatesMultiToken, Action<object, AccountUpdateMultiArgs>> AccountUpdateMultiSubscribersLookup;
        public void AccountUpdateMultiSubscribe(AccountUpdatesMultiToken token, Action<object, AccountUpdateMultiArgs> action)
        {
            lock (AccountUpdateMultiLock)
            {
                AccountUpdateMultiSubscribersList.Add(new TokenActionPair<AccountUpdatesMultiToken, AccountUpdateMultiArgs>(token, action));
                AccountUpdateMultiSubscribersLookup = AccountUpdateMultiSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool AccountUpdateMultiUnSubscribe(AccountUpdatesMultiToken token, Action<object, AccountUpdateMultiArgs> action)
        {
            lock (AccountUpdateMultiLock)
            {
                bool Exists = AccountUpdateMultiSubscribersList.Remove(new TokenActionPair<AccountUpdatesMultiToken, AccountUpdateMultiArgs>(token, action));
                if (Exists)
                {
                    AccountUpdateMultiSubscribersLookup = AccountUpdateMultiSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public void accountUpdateMulti(int requestId, string account, string modelCode, string key, string value, string currency)
        {
            lock (AccountUpdateMultiLock)
            {
                if (AccountUpdateMultiSubscribersLookup != null)
                {
                    AccountUpdatesMultiToken token = new AccountUpdatesMultiToken(requestId);

                    var subscribers = AccountUpdateMultiSubscribersLookup[token];
                    foreach (Action<object, AccountUpdateMultiArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new AccountUpdateMultiArgs(requestId, account, modelCode, key, value, currency), action.EndInvoke, null);
                    }
                }
            }
        }

        object AccountUpdateMultiEndLock = new object();
        List<TokenActionPair<AccountUpdatesMultiToken, AccountUpdateMultiEndArgs>> AccountUpdateMultiEndSubscribersList = new List<TokenActionPair<AccountUpdatesMultiToken, AccountUpdateMultiEndArgs>>();
        ILookup<AccountUpdatesMultiToken, Action<object, AccountUpdateMultiEndArgs>> AccountUpdateMultiEndSubscribersLookup;
        public void AccountUpdateMultiEndSubscribe(AccountUpdatesMultiToken token, Action<object, AccountUpdateMultiEndArgs> action)
        {
            lock (AccountUpdateMultiEndLock)
            {
                AccountUpdateMultiEndSubscribersList.Add(new TokenActionPair<AccountUpdatesMultiToken, AccountUpdateMultiEndArgs>(token, action));
                AccountUpdateMultiEndSubscribersLookup = AccountUpdateMultiEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool AccountUpdateMultiEndUnSubscribe(AccountUpdatesMultiToken token, Action<object, AccountUpdateMultiEndArgs> action)
        {
            lock (AccountUpdateMultiEndLock)
            {
                bool Exists = AccountUpdateMultiEndSubscribersList.Remove(new TokenActionPair<AccountUpdatesMultiToken, AccountUpdateMultiEndArgs>(token, action));
                if (Exists)
                {
                    AccountUpdateMultiEndSubscribersLookup = AccountUpdateMultiEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public void accountUpdateMultiEnd(int requestId)
        {
            lock (AccountUpdateMultiEndLock)
            {
                if (AccountUpdateMultiEndSubscribersLookup != null)
                {
                    AccountUpdatesMultiToken token = new AccountUpdatesMultiToken(requestId);

                    var subscribers = AccountUpdateMultiEndSubscribersLookup[token];
                    foreach (Action<object, AccountUpdateMultiEndArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new AccountUpdateMultiEndArgs(requestId), action.EndInvoke, null);
                    }
                }
            }
        }

        object SecurityDefinitionOptionParameterLock = new object();
        List<TokenActionPair<SecDefOptParamsToken, SecurityDefinitionOptionParameterArgs>> SecurityDefinitionOptionParameterSubscribersList = new List<TokenActionPair<SecDefOptParamsToken, SecurityDefinitionOptionParameterArgs>>();
        ILookup<SecDefOptParamsToken, Action<object, SecurityDefinitionOptionParameterArgs>> SecurityDefinitionOptionParameterSubscribersLookup;
        public void SecurityDefinitionOptionParameterSubscribe(SecDefOptParamsToken token, Action<object, SecurityDefinitionOptionParameterArgs> action)
        {
            lock (SecurityDefinitionOptionParameterLock)
            {
                SecurityDefinitionOptionParameterSubscribersList.Add(new TokenActionPair<SecDefOptParamsToken, SecurityDefinitionOptionParameterArgs>(token, action));
                SecurityDefinitionOptionParameterSubscribersLookup = SecurityDefinitionOptionParameterSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool SecurityDefinitionOptionParameterUnSubscribe(SecDefOptParamsToken token, Action<object, SecurityDefinitionOptionParameterArgs> action)
        {
            lock (SecurityDefinitionOptionParameterLock)
            {
                bool Exists = SecurityDefinitionOptionParameterSubscribersList.Remove(new TokenActionPair<SecDefOptParamsToken, SecurityDefinitionOptionParameterArgs>(token, action));
                if (Exists)
                {
                    SecurityDefinitionOptionParameterSubscribersLookup = SecurityDefinitionOptionParameterSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
        {
            lock (SecurityDefinitionOptionParameterLock)
            {
                if (SecurityDefinitionOptionParameterSubscribersLookup != null)
                {
                    SecDefOptParamsToken token = new SecDefOptParamsToken(reqId);

                    var subscribers = SecurityDefinitionOptionParameterSubscribersLookup[token];
                    foreach (Action<object, SecurityDefinitionOptionParameterArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new SecurityDefinitionOptionParameterArgs(reqId, exchange, underlyingConId, tradingClass, multiplier, expirations, strikes), action.EndInvoke, null);
                    }
                }
            }
        }

        object SecurityDefinitionOptionParameterEndLock = new object();
        List<TokenActionPair<SecDefOptParamsToken, SecurityDefinitionOptionParameterEndArgs>> SecurityDefinitionOptionParameterEndSubscribersList = new List<TokenActionPair<SecDefOptParamsToken, SecurityDefinitionOptionParameterEndArgs>>();
        ILookup<SecDefOptParamsToken, Action<object, SecurityDefinitionOptionParameterEndArgs>> SecurityDefinitionOptionParameterEndSubscribersLookup;
        public void SecurityDefinitionOptionParameterEndSubscribe(SecDefOptParamsToken token, Action<object, SecurityDefinitionOptionParameterEndArgs> action)
        {
            lock (SecurityDefinitionOptionParameterEndLock)
            {
                SecurityDefinitionOptionParameterEndSubscribersList.Add(new TokenActionPair<SecDefOptParamsToken, SecurityDefinitionOptionParameterEndArgs>(token, action));
                SecurityDefinitionOptionParameterEndSubscribersLookup = SecurityDefinitionOptionParameterEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool SecurityDefinitionOptionParameterEndUnSubscribe(SecDefOptParamsToken token, Action<object, SecurityDefinitionOptionParameterEndArgs> action)
        {
            lock (SecurityDefinitionOptionParameterEndLock)
            {
                bool Exists = SecurityDefinitionOptionParameterEndSubscribersList.Remove(new TokenActionPair<SecDefOptParamsToken, SecurityDefinitionOptionParameterEndArgs>(token, action));
                if (Exists)
                {
                    SecurityDefinitionOptionParameterEndSubscribersLookup = SecurityDefinitionOptionParameterEndSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public void securityDefinitionOptionParameterEnd(int reqId)
        {
            lock (SecurityDefinitionOptionParameterEndLock)
            {
                if (SecurityDefinitionOptionParameterEndSubscribersLookup != null)
                {
                    SecDefOptParamsToken token = new SecDefOptParamsToken(reqId);

                    var subscribers = SecurityDefinitionOptionParameterEndSubscribersLookup[token];
                    foreach (Action<object, SecurityDefinitionOptionParameterEndArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new SecurityDefinitionOptionParameterEndArgs(reqId), action.EndInvoke, null);
                    }
                }
            }
        }

        object SoftDollarTiersLock = new object();
        List<TokenActionPair<SoftDollarTiersToken, SoftDollarTiersArgs>> SoftDollarTiersSubscribersList = new List<TokenActionPair<SoftDollarTiersToken, SoftDollarTiersArgs>>();
        ILookup<SoftDollarTiersToken, Action<object, SoftDollarTiersArgs>> SoftDollarTiersSubscribersLookup;
        public void SoftDollarTiersSubscribe(SoftDollarTiersToken token, Action<object, SoftDollarTiersArgs> action)
        {
            lock (SoftDollarTiersLock)
            {
                SoftDollarTiersSubscribersList.Add(new TokenActionPair<SoftDollarTiersToken, SoftDollarTiersArgs>(token, action));
                SoftDollarTiersSubscribersLookup = SoftDollarTiersSubscribersList.ToLookup(p => p.Token, p => p.Action);
            }
        }
        public bool SoftDollarTiersUnSubscribe(SoftDollarTiersToken token, Action<object, SoftDollarTiersArgs> action)
        {
            lock (SoftDollarTiersLock)
            {
                bool Exists = SoftDollarTiersSubscribersList.Remove(new TokenActionPair<SoftDollarTiersToken, SoftDollarTiersArgs>(token, action));
                if (Exists)
                {
                    SoftDollarTiersSubscribersLookup = SoftDollarTiersSubscribersList.ToLookup(p => p.Token, p => p.Action);
                }
                return Exists;
            }
        }
        public void softDollarTiers(int reqId, SoftDollarTier[] tiers)
        {
            lock (SoftDollarTiersLock)
            {
                if (SoftDollarTiersSubscribersLookup != null)
                {
                    SoftDollarTiersToken token = new SoftDollarTiersToken(reqId);

                    var subscribers = SoftDollarTiersSubscribersLookup[token];
                    foreach (Action<object, SoftDollarTiersArgs> action in subscribers)
                    {
                        action.BeginInvoke(Wrapper, new SoftDollarTiersArgs(reqId, tiers), action.EndInvoke, null);
                    }
                }
            }
        }
    }
}

