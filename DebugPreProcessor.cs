#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
using Nalarium.Reporting;
//+
namespace Nalarium.Web.Processing
{
    internal class DebugInitProcessor : SystemInitProcessor
    {
        public const String Id = "Debug_";

        //- @Setting -//
        public static class Setting
        {
            public const String Enable = "Enable";
            public const String ReportSenderO = "ReportSenderO";
        }

        //- @OnPreHttpHandlerExecute -//
        public override InitProcessor Execute()
        {
            ReportController.RegisterFactory("Nalarium.Web.Reporting.ReportCreatorFactory, Nalarium.Web");
            Reporter reporter = ReportController.GetReporter(WebProcessingReportController.ProcessingDebugReporter);
            if (reporter.Initialized)
            {
                reporter.ReportCreator = new MapReportCreator();
            }
            //+
            return null;
        }
    }
}