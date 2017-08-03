using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class ReceiveFAArgs : EventArgs
{
public int FaDataType { get;}
public string FaXmlData { get;}

public ReceiveFAArgs(int faDataType, string faXmlData)
{
FaDataType = faDataType;
FaXmlData = faXmlData;
}
}
}

