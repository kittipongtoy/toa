using System;
using System.Diagnostics;
using System.Text;

namespace TOAMediaPlayer
{
    public enum LogScope
    {
        TOA_System,
        TOA_Player,
        TOA_PlayList,
        TOA_Socket,
        None = 99999,
    }

    public enum LogObjType
    {
        None,
        Service_System,
    }

    public enum LogCategory
    {
        None,
    }
    public class NSEventLog
    {
        public static string FullStackException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }
        private static void CreateEvent(LogScope xLogScope)
        {
            try { if (true == EventLog.SourceExists("TOA?0001-System")) EventLog.Delete("TOA?0001-System"); }
            catch { }
            try { if (true == EventLog.SourceExists("TOA?0002-Security")) EventLog.Delete("TOA?0002-Security"); }
            catch { }
            string xEventName = NSEventLog.Name_From_LogScope(xLogScope);
            if (xEventName.Length > 0)
            {
                if (false == EventLog.SourceExists(xEventName))
                {
                    EventLog.CreateEventSource(xEventName, xEventName);
                    using (EventLog xEventLog = new EventLog(xEventName))
                    {
                        xEventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
                        xEventLog.MaximumKilobytes = 10240;
                    }
                }
            }
        }

        private static string Name_From_LogScope(LogScope xLogScope)
        {
            string xEventName = string.Empty;
            switch (xLogScope)
            {
                case LogScope.TOA_System:
                    xEventName = "TOA-0000-System";
                    break;
                case LogScope.TOA_Player:
                    xEventName = "TOA-0001-Player";
                    break;
                case LogScope.TOA_PlayList:
                    xEventName = "TOA-0002-PlayList";
                    break;
                case LogScope.TOA_Socket:
                    xEventName = "TOA-0003-Socket";
                    break;

                default:
                    break;
            }
            return xEventName;
        }

        public static void Write(EventLogEntryType EntryType, string strTitle, string strMessage, Exception xException, LogScope Scope)
        {
            NSEventLog.Write(EntryType, strTitle, strMessage, xException, Scope, LogObjType.None, LogCategory.None, null);
        }

        public static void Write(EventLogEntryType EntryType, string strTitle, string strMessage, Exception xException, LogScope Scope, LogObjType ObjType)
        {
            NSEventLog.Write(EntryType, strTitle, strMessage, xException, Scope, ObjType, LogCategory.None, null);
        }

        public static void Write(EventLogEntryType EntryType, string strTitle, string strMessage, Exception xException, LogScope Scope, LogObjType ObjType, LogCategory Cate)
        {
            NSEventLog.Write(EntryType, strTitle, strMessage, xException, Scope, ObjType, Cate, null);
        }

        public static void Write(EventLogEntryType EntryType, string strTitle, string strMessage, Exception xException, LogScope Scope, LogObjType ObjType, LogCategory Cate, byte[] rawData)
        {
            string xFullMessage = Get_ErrorMessage_FromException(strMessage, xException);

            NSEventLog.Write(EntryType, strTitle, xFullMessage, Scope, ObjType, Cate, rawData);
        }

        public static string Get_ErrorMessage_FromException(Exception xException)
        {
            return Get_ErrorMessage_FromException(string.Empty, xException);
        }

        public static string Get_ErrorMessage_FromException(string strMessage, Exception xException)
        {
            if (null != xException.InnerException)
            {
                return Get_ErrorMessage_FromException(strMessage, xException.InnerException);
            }

            string xExceptionText = string.Format("Exception Name : {0}", xException.GetType().Name);

            if ((null != xException.Message) && (xException.Message.Length > 0))
            {
                if (xExceptionText.Length > 0) xExceptionText += "\r\n\r\n";
                xExceptionText += string.Format(": Message :\r\n{0}", xException.Message);
            }
            if ((null != xException.Source) && (xException.Source.Length > 0))
            {
                if (xExceptionText.Length > 0) xExceptionText += "\r\n\r\n";
                xExceptionText += string.Format(": Source :\r\n{0}", xException.Source);
            }
            if ((null != xException.StackTrace) && (xException.StackTrace.Length > 0))
            {
                if (xExceptionText.Length > 0) xExceptionText += "\r\n\r\n";
                xExceptionText += string.Format(": StackTrace :\r\n{0}", xException.StackTrace);
            }

            string xFullMessage = string.Empty;

            if ((null != strMessage) && (strMessage.Length > 0))
            {
                if (xFullMessage.Length > 0) xFullMessage += "\r\n\r\n";
                xFullMessage += strMessage;
            }
            if ((null != xExceptionText) && (xExceptionText.Length > 0))
            {
                if (xFullMessage.Length > 0) xFullMessage += "\r\n\r\n";
                xFullMessage += xExceptionText;
            }

            return xFullMessage;
        }

        public static void Write(EventLogEntryType EntryType, string strTitle, string strMessage, LogScope Scope)
        {
            NSEventLog.Write(EntryType, strTitle, strMessage, Scope, LogObjType.None, LogCategory.None, null);
        }

        public static void Write(EventLogEntryType EntryType, string strTitle, string strMessage, LogScope Scope, string ScopeName_Ext)
        {
            NSEventLog.Write(EntryType, strTitle, strMessage, Scope, LogObjType.None, LogCategory.None, null, ScopeName_Ext);
        }

        public static void Write(EventLogEntryType EntryType, string strTitle, string strMessage, LogScope Scope, LogObjType ObjType)
        {
            NSEventLog.Write(EntryType, strTitle, strMessage, Scope, ObjType, LogCategory.None, null, "");
        }

        public static void Write(EventLogEntryType EntryType, string strTitle, string strMessage, LogScope Scope, LogObjType ObjType, LogCategory Cate)
        {
            NSEventLog.Write(EntryType, strTitle, strMessage, Scope, ObjType, Cate, null, "");
        }

        public static void Write(EventLogEntryType EntryType, string strTitle, string strMessage, LogScope Scope, LogObjType ObjType, LogCategory Cate, byte[] rawData)
        {
            NSEventLog.Write(EntryType, strTitle, strMessage, Scope, ObjType, Cate, rawData, "");
        }

        public static void Write(EventLogEntryType EntryType, string strTitle, string strMessage, LogScope xLogScope, LogObjType ObjType, LogCategory Cate, byte[] rawData, string ScopeName_Ext)
        {
            string xEventName = NSEventLog.Name_From_LogScope(xLogScope);
            if (0 == xEventName.Length) return;
            string strObjType = (ObjType != LogObjType.None) ? System.Enum.GetName(typeof(LogObjType), ObjType).Replace('_', ' ') : string.Empty;
            string strHeader = string.Empty;
            NSEventLog.CreateEvent(xLogScope);
            string xEventLogID = Guid.NewGuid().ToString("D").ToUpper();
            if (strMessage.Length < 10000)
            {
                string xLogMessage_Full = string.Empty;
                //string xLogMessage_Full = string.Format("< LOGID : {0} >\r\n", xEventLogID);
                if ((null != strTitle) && (strTitle.Length > 0))
                {
                    xLogMessage_Full += string.Format("{0}\r\n", strTitle);
                    //xLogMessage_Full += "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\r\n\r\n";
                }
                xLogMessage_Full += strMessage;
                EventLog.WriteEntry(xEventName, xLogMessage_Full, EntryType, 0, 0, rawData);
            }
            else
            {
                int nTotal = (strMessage.Length / 10000) + 1;
                //for (int nCount = 0; nCount < nTotal; nCount++)
                int nCount = 0;
                while (nCount < nTotal)
                {
                    string xLogMessage_Full = string.Format("< LOGID : {0} # {1} of {2} >\r\n", xEventLogID, nCount + 1, nTotal);
                    if ((null != strTitle) && (strTitle.Length > 0))
                    {
                        xLogMessage_Full += string.Format("{0}\r\n", strTitle);
                        xLogMessage_Full += "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\r\n";
                    }
                    if (nCount + 1 == nTotal)
                    {
                        xLogMessage_Full += strMessage.Substring(nCount * 10000);
                    }
                    else
                    {
                        xLogMessage_Full += strMessage.Substring(nCount * 10000, 10000);
                    }
                    EventLog.WriteEntry(xEventName, xLogMessage_Full, EntryType, 0, 0, rawData);

                    nCount++;//add
                    break;
                }
            }
        }
    }

}
