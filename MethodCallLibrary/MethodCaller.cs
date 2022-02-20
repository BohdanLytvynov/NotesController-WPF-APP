using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MethodCallLibrary
{
    public static class MethodCaller
    {
        /// <summary>
        /// Method that gives oportunity to call methods inside and provide some logging
        /// Tdel - delegate that points at the method to perform
        /// Tlog - type of logger class
        /// TLogCol - type of Tlog collection
        /// Tlogerdel - delegate that points at method that creates an instance of the log class and adds it to log collection
        /// param: method - method to perform
        /// param: result - result of performing the method
        /// param: LogCollection - Collection of log instances
        /// param: logMethod - method that creates log and adds it to log collection
        /// param: info_logmethod - aditional info for log method 
        /// param: method_params - params for main method
        /// </summary>
        /// <typeparam name="Tdel"></typeparam>
        /// <typeparam name="Tlog"></typeparam>
        /// <typeparam name="TlogCol"></typeparam>
        /// <typeparam name="Tlogerdel"></typeparam>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="method"></param>
        /// <param name="result"></param>
        /// <param name="LogCollection"></param>
        /// <param name="logMethod"></param>
        /// <param name="info_LogMethod"></param>
        /// <param name="method_params"></param>
        /// <returns></returns>
        public static Tout CallFuncMethodViaEnvelopeWithLoging<Tdel, Tlog, TlogCol, Tlogerdel, Tout>
            (Tdel method, out bool result,
            TlogCol LogCollection, Tlogerdel logMethod,
             object[] info_LogMethod = null, params object[] method_params
            )
            where Tdel : MulticastDelegate
            where Tlogerdel : MulticastDelegate
            where Tlog : LogBase
            where TlogCol : IList<Tlog>
            where Tout : class
        {
            Exception temp = null;

            Tout MethodResult = null;

            try
            {
                MethodResult = (Tout)method?.DynamicInvoke(method_params);

                result = true;
            }
            catch (Exception e)
            {
                temp = e;
                result = false;
            }
            finally
            {
                LogCollection.Add((Tlog)logMethod.DynamicInvoke(temp, info_LogMethod));
            }

            return MethodResult;
        }

    }   
}
