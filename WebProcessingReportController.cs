#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System.Threading;
using Nalarium.Reporting;
//+

namespace Nalarium.Web.Processing
{
    public static class WebProcessingReportController
    {
        public const string ProcessingDebugReporter = "WebProcessingDebug";
        private static readonly ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

        //+ field
        internal static Reporter reporter;

        //+

        //- @Reporter -//
        public static Reporter Reporter
        {
            get
            {
                readerWriterLockSlim.EnterUpgradeableReadLock();
                try
                {
                    if (reporter == null)
                    {
                        readerWriterLockSlim.EnterWriteLock();
                        try
                        {
                            if (reporter == null)
                            {
                                reporter = ReportController.GetReporter(ProcessingDebugReporter);
                            }
                        }
                        finally
                        {
                            readerWriterLockSlim.ExitWriteLock();
                        }
                    }
                    //+
                    return reporter;
                }
                finally
                {
                    readerWriterLockSlim.ExitUpgradeableReadLock();
                }
            }
        }
    }
}