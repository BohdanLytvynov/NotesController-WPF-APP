using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace MethodCallLibrary
{
    /// <summary>
    /// A derrived class from this class can be created. 
    /// And it can be used in Method Caller Class
    /// </summary>
    public abstract class LogBase
    {
        public virtual DateTime Date { get; set; }

        public virtual string Error { get; set; }
        
        public LogBase(string error)
        {
            Date = DateTime.Now;

            Error = error;           
        }
    }
}
