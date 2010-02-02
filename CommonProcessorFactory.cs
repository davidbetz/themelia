#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing
{
    internal class CommonProcessorFactory : Nalarium.Web.Processing.ProcessorFactory
    {
        //- @CreateProcessor -//
        public override IProcessor CreateProcessor(String text)
        {
            switch (text)
            {
                case "__$defaultpageselectionprocessor":
                    return new DefaultPageSelectionProcessor();
                case "{passthrough}":
                    return new PassThroughInitProcessor();
                case "emailsendingerrorprocessor":
                    return new EmailSendingErrorProcessor();
                case "debugerrorprocessor":
                    return new DebugErrorProcessor();
                case "traceerrorprocessor":
                    return new TraceErrorProcessor();
                case "__$defaultpageoverrideprocessor":
                    return new DefaultPageOverrideProcessor();
            }
            //+
            return null;
        }
    }
}