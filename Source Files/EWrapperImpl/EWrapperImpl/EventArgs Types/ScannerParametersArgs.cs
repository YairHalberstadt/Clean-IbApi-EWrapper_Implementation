using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class ScannerParametersArgs :EventArgs
    {
       public string Xml { get; }
       public ScannerParametersArgs(string xml)
        {
            Xml = xml;
        }
    }
}