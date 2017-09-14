using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class VerifyCompletedArgs :EventArgs
    {
       public bool IsSuccessful { get; }
       public string ErrorText { get; }
       public VerifyCompletedArgs(bool isSuccessful, string errorText)
        {
            IsSuccessful = isSuccessful;
            ErrorText = errorText;
        }
    }
}