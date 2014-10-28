using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Microsoft.TED.CompositeLOBDemo.SharedModule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace Microsoft.TED.CompositeLOBDemo.Insights
{
    public sealed class AppInsightsService : IAppInsightsService
    {
        public AppInsightsService()
        {
            // C#/VB App
            // Add 'using Microsoft.ApplicationInsights.Telemetry.WindowsStore;' to all the pages where you want to use Application Insights APIs.
            // ClientAnalyticsSession.Default.Start("3f8e3799-5d80-4093-b3eb-14cc8c28766a");

            ClientAnalyticsSession.Default.Start("3f8e3799-5d80-4093-b3eb-14cc8c28766a");
            if (!ClientAnalyticsSession.Default.Enabled)
                Debug.WriteLine("App Insights not enabled!!");
        }

#if WINDOWS_APP

        public void LogEvent(string eventName, string eventMessage)
        {
            ClientAnalyticsChannel.Default.LogEvent
                (
                    eventName,
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("message", eventMessage)
                    }
                );
        }

        public void LogError(string errorName, string errorMessage, Exception ex)
        {
            ClientAnalyticsChannel.Default.LogMessage
                (
                    string.Concat(errorName, ": ", ex.Message),
                    Windows.Foundation.Diagnostics.LoggingLevel.Critical
                );
        }

        public object LogStartEvent(string eventName, string eventMessage)
        {
            var context = ClientAnalyticsChannel.Default.StartTimedEvent
                            (
                                eventName,
                                new List<KeyValuePair<string, object>>
                                {
                                    new KeyValuePair<string, object>("message", eventMessage)
                                }
                            );
            return context;
        }

        public void LogEndEvent(object context)
        {
            var appInsightsContext = context as TimedAnalyticsEvent;
            if (appInsightsContext != null)
            {
                appInsightsContext.End();
            }
        }

#endif

#if WINDOWS_PHONE_APP

        public void LogEvent(string eventName, string eventMessage)
        {
            ClientAnalyticsChannel.Default.LogEvent
                (
                    eventName,
                    new Dictionary<string, object>
                    {
                        { "message", eventMessage }
                    }
                );
        }

        public void LogError(string errorName, string errorMessage, Exception ex)
        {

            ClientAnalyticsChannel.Default.LogEvent
                (
                    string.Concat(errorName, ": ", ex.Message),
                    new Dictionary<string, object>
                    {
                        { "errorMessage", errorMessage},
                        { "exceptionDetails", ex.ToString() }
                    }
                );
        }

        public object LogStartEvent(string eventName, string eventMessage)
        {
            var context = ClientAnalyticsChannel.Default.StartTimedEvent
                            (
                                eventName,
                                new Dictionary<string, object>
                                {
                                    { "message", eventMessage }
                                }
                            );
            return context;
        }

        public void LogEndEvent(object context)
        {
            var appInsightsContext = context as TimedAnalyticsEvent;
            if (appInsightsContext != null)
            {
                appInsightsContext.End();
            }
        }

#endif

    }
}
