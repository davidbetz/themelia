#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using Nalarium.Activation;
using Nalarium.Web.Activation;

namespace Nalarium.Web.Processing
{
    internal static class RouteCache
    {
        internal static Map<String, IProcessor> ProcessorCache = new Map<String, IProcessor>();
        internal static Map<String, IFactory> ProcessorFactoryCache = new Map<String, IFactory>();
        internal static Map<String, IFactory> HandlerFactoryCache = new Map<String, IFactory>();

        //- @Ctor -//
        static RouteCache()
        {
            FactoryCache.TypeFactoryCache.Add("ExceptionTypeObjectFactory", new ExceptionTypeFactory());
            FactoryCache.TypeFactoryCache.Add("WebExceptionTypeObjectFactory", new WebExceptionTypeFactory());
            ProcessorFactoryCache.Add("CommonProcessorFactory", new CommonProcessorFactory());
            HandlerFactoryCache.Add("CommonHandlerFactory", new CommonHandlerFactory());
        }
    }
}