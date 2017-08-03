using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class UpdateNewsBulletinArgs : EventArgs
{
public int MsgId { get;}
public int MsgType { get;}
public string Message { get;}
public string OrigExchange { get;}

public UpdateNewsBulletinArgs(int msgId, int msgType, string message, string origExchange)
{
MsgId = msgId;
MsgType = msgType;
Message = message;
OrigExchange = origExchange;
}
}
}

