using System;
using System.Collections.Generic;
using System.Text;
using MethodCallLibrary;

namespace LogModels
{
    public class HTTPConnectionLog : LogBase
    {
        public string RequestType { get; set; }

        public bool IsError { get; set; }

        public string Code { get; set; }//Status Code

        public HTTPConnectionLog(string error, string code, string requestType)
            :base(error)
        {
            RequestType = requestType;

            if (String.IsNullOrEmpty(Error))
            {
                IsError = false;
            }
            else
            {
                IsError = true;
            }

            Code = code;
        }

        
    }
}
