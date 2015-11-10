#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using Nalarium.Activation;
//+

namespace Nalarium.Web.Processing
{
    internal static class ProcessorActivator
    {
        //- $Create -//
        internal static T Create<T>(String processorType, Map<String, IFactory> processorFactoryMap) where T : class, IProcessor
        {
            T processor = null;
            if (processorType.Contains(","))
            {
                return ObjectCreator.CreateAs<T>(processorType);
            }
            //+
            if (processor == null && processorFactoryMap != null)
            {
                List<IFactory> processorFactoryList = processorFactoryMap.GetValueList();
                foreach (IFactory factory in processorFactoryList)
                {
                    processorType = processorType.ToLower(CultureInfo.CurrentCulture);
                    processor = (T)((ProcessorFactory)factory).CreateProcessor(processorType);
                    if (processor != null)
                    {
                        break;
                    }
                }
            }
            //+
            return processor;
        }
    }
}