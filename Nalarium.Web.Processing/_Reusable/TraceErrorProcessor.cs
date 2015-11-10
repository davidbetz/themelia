#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using Nalarium.Reporting;

namespace Nalarium.Web.Processing
{
    public class TraceErrorProcessor : ErrorProcessor
    {
        private const String ReporterName = "TraceError";

        //+
        //- @OnErrorProcessorExecute -//
        /// <summary>
        /// Sends a notification e-mail to the e-mail address set in web.config when an unhandled ASP.NET exception is thrown.
        /// </summary>
        public override void Execute()
        {
            String formatterType = null;
            if (ParameterArray != null)
            {
                formatterType = ParameterArray[0] as String;
            }
            if (String.IsNullOrEmpty(formatterType))
            {
                formatterType = "Wiki";
            }
            Reporter reporter = ReportController.GetReporter(ReporterName);
            if (!reporter.Initialized)
            {
                reporter = ReportController.Create(ReporterName, "HttpContext", "Trace", formatterType);
            }
            reporter.SendSingle(new Object[]
                                {
                                    "Uncaught Exception", Context
                                });
        }
    }
}