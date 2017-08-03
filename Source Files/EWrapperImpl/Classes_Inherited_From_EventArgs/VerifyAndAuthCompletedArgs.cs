using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class VerifyAndAuthCompletedArgs : EventArgs
{
public bool IsSuccessful { get;}
public string ErrorText { get;}

public VerifyAndAuthCompletedArgs(bool isSuccessful, string errorText)
{
IsSuccessful = isSuccessful;
ErrorText = errorText;
}
}
}

