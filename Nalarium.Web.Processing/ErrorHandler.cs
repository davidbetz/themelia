#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web;
using Nalarium.Reporting;
using Nalarium.Web.Processing.Data;
using Nalarium.Reporting.ReportCreator;
//+

namespace Nalarium.Web.Processing
{
    internal static class ErrorHandler
    {
        //- ~OnHandleError -//
        internal static void OnHandleError(Object sender, EventArgs ea)
        {
            var ha = sender as HttpApplication;
            if (ha == null)
            {
                return;
            }
            HttpContext context = ha.Context;
            Exception exception = null;
            if (context.Error != null)
            {
                exception = context.Error.InnerException;
            }
            if (NalariumContext.Current != null && NalariumContext.Current.WebDomain != null && NalariumContext.Current.WebDomain.Configuration != null)
            {
                ErrorProcessorData data;
                ErrorProcessor processor = ProcessorRunner.RunErrorProcessing(out data);
                if (processor != null)
                {
                    CheckAndRun(context, exception, processor, data);
                }
            }
            else
            {
                if (WebProcessingReportController.Reporter.Initialized)
                {
                    var map = new Map();
                    map.Add("Section", "ErrorProcessor");
                    map.Add("Message", "An uncaught exception was thrown, but WebDomain.CurrentData was null.  However, the original error message has been preserved and is packaged in this report.");
                    map.Add("Url", Http.AbsoluteUrlOriginalCase);
                    var creator = new ExceptionReportCreator
                                  {
                                      Formatter = WebProcessingReportController.Reporter.Formatter
                                  };
                    map.Add("Exception Report", creator.Create(exception));
                    //+
                    WebProcessingReportController.Reporter.AddMap(map);
                }
            }
        }

        //- $CheckAndRun -//
        private static void CheckAndRun(HttpContext context, Exception exception, ErrorProcessor processor, ErrorProcessorData data)
        {
            //+ filter
            Type exceptionType = exception.GetType();
            Boolean run = false;
            if (data.AcceptableTypeList.Count == 0)
            {
                run = true;
            }
            else
            {
                foreach (Type type in data.AcceptableTypeList)
                {
                    if (type == exceptionType)
                    {
                        run = true;
                    }
                }
            }
            if (run)
            {
                processor.Initialize(context, exception, data.ParameterArray);
                processor.Execute();
            }
        }
    }
}