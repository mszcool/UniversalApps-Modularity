using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.TED.CompositeLOBDemo.SharedModule
{
    public interface IAppInsightsService
    {
        void LogEvent(string eventName, string eventMessage);
        void LogError(string errorName, string errorMessage, Exception ex);
        object LogStartEvent(string eventName, string eventMessage);
        void LogEndEvent(object context);
    }
}
