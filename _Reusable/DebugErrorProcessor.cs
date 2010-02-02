#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using Nalarium.Reporting;
//+
namespace Nalarium.Web.Processing
{
    public class DebugErrorProcessor : ErrorProcessor
    {
        private const String ReporterName = "DebugError";

        //+
        //- @OnErrorProcessorExecute -//
        /// <summary>
        /// Sends a notification e-mail to the e-mail address set in web.config when an unhandled ASP.NET exception is thrown.
        /// </summary>
        /// <param name="context">The HttpContext object.</param>
        /// <param name="parameterArray">The parameter array.</param>
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
                reporter = ReportController.Create(ReporterName, "HttpContext", "Debug", formatterType);
            }
            reporter.SendSingle(new Object[] { "Uncaught Exception", Context });
        }
    }
}