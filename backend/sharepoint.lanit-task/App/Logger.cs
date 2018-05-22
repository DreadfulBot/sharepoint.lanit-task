using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Diagnostics;

namespace sharepoint.lanit_task.App
{
    class Logger
    {
        public static void Log(string message, EventLogEntryType type)
        {
            SPUtility.ValidateFormDigest();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                if (!EventLog.SourceExists("SharePoint Custom Solutions"))
                {
                    EventLog.CreateEventSource("SharePoint Custom Solutions", "Application");
                }

                EventLog.WriteEntry("SharePoint Custom Solutions",
                                            message,
                                            type);
            });
        }

        public static void LogMessage(string message)
        {
            Log(message, EventLogEntryType.Information);
        }

        public static void LogException(Exception ex)
        {
            Log(string.Format(
                "HelpLink = {0}\r\nMessage = {1}\r\nSource = {2}\r\nStackTrace = {3}\r\nTargetSite = {4}",
                ex.HelpLink, ex.Message, ex.Source, ex.StackTrace, ex.TargetSite), EventLogEntryType.Error);
        }
    }
}
