#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Linq;
using System.Threading;
using System.Web;
using Nalarium.Web.Processing.Data;
//+

namespace Nalarium.Web.Processing
{
    internal static class ProcessorRunner
    {
        internal static ReaderWriterLockSlim ProcessorReaderWriterLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        //+ field
        private static Object _lock = new Object();

        //+
        //- ~RunInitProcessing -//
        internal static void RunInitProcessing(HttpContext context, InitProcessorDataList initProcessorDataList)
        {
            if (initProcessorDataList.Count == 0)
            {
                return;
            }
            foreach (ProcessorData data in initProcessorDataList.OrderBy(p => p.Priority))
            {
                String processorType = data.ProcessorType;
                //+
                ProcessorReaderWriterLockSlim.EnterUpgradeableReadLock();
                try
                {
                    InitProcessor processor = null;
                    //+
                    if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                    {
                        ProcessorReaderWriterLockSlim.EnterWriteLock();
                        //+
                        try
                        {
                            if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                            {
                                processor = ProcessorActivator.Create<InitProcessor>(processorType, RouteCache.ProcessorFactoryCache);
                                if (processor == null)
                                {
                                    throw new EntityNotFoundException(String.Format(Resource.General_NotFound, processorType));
                                }
                                RouteCache.ProcessorCache.Add(processorType, processor);
                            }
                        }
                        finally
                        {
                            ProcessorReaderWriterLockSlim.ExitWriteLock();
                        }
                    }
                    if (processor == null)
                    {
                        processor = RouteCache.ProcessorCache.Get<InitProcessor>(processorType);
                    }
                    //+
                    if (processor != null)
                    {
                        //+ chain
                        do
                        {
                            if (processor.IsChained)
                            {
                                processor.Initialize(HttpContext.Current, null);
                            }
                            else
                            {
                                processor.Initialize(HttpContext.Current, data.ParameterArray);
                            }
                            processor = processor.Execute();
                            if (processor != null)
                            {
                                processor.IsChained = true;
                            }
                        } while (processor != null);
                    }
                }
                catch (Exception ex)
                {
                    if (WebProcessingReportController.Reporter.Initialized)
                    {
                        var map = new Map();
                        map.Add("Section", "InitProcessor");
                        map.Add("Type", processorType);
                        map.Add("Message", ex.Message);
                        map.Add("Exception Type", ex.GetType().FullName);
                        //+
                        WebProcessingReportController.Reporter.AddMap(map);
                    }
                }
                finally
                {
                    ProcessorReaderWriterLockSlim.ExitUpgradeableReadLock();
                }
            }
        }

        //- ~RunSelectionProcessing -//
        internal static IHttpHandler RunSelectionProcessing(HttpContext context, SelectionProcessorDataList dataList)
        {
            if (dataList.Count == 0)
            {
                return null;
            }
            foreach (ProcessorData data in dataList.OrderBy(p => p.Priority))
            {
                String processorType = data.ProcessorType;
                //+
                ProcessorReaderWriterLockSlim.EnterUpgradeableReadLock();
                try
                {
                    SelectionProcessor processor = null;
                    //+
                    if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                    {
                        ProcessorReaderWriterLockSlim.EnterWriteLock();
                        try
                        {
                            if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                            {
                                processor = ProcessorActivator.Create<SelectionProcessor>(processorType, RouteCache.ProcessorFactoryCache);
                                if (processor == null)
                                {
                                    throw new EntityNotFoundException(String.Format(Resource.General_NotFound, processorType));
                                }
                                RouteCache.ProcessorCache.Add(processorType, processor);
                            }
                        }
                        finally
                        {
                            ProcessorReaderWriterLockSlim.ExitWriteLock();
                        }
                    }
                    if (processor == null)
                    {
                        processor = RouteCache.ProcessorCache.Get<SelectionProcessor>(processorType);
                    }
                    //+
                    if (processor != null)
                    {
                        ProcessorReaderWriterLockSlim.EnterWriteLock();
                        //+
                        try
                        {
                            if (processor != null)
                            {
                                return processor.Execute(context, data.ParameterArray);
                            }
                        }
                        finally
                        {
                            ProcessorReaderWriterLockSlim.ExitWriteLock();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (WebProcessingReportController.Reporter.Initialized)
                    {
                        var map = new Map();
                        map.Add("Section", "SelectionProcessor");
                        map.Add("Type", processorType);
                        map.Add("Message", ex.Message);
                        map.Add("Exception Type", ex.GetType().FullName);
                        //+
                        WebProcessingReportController.Reporter.AddMap(map);
                    }
                }
                finally
                {
                    ProcessorReaderWriterLockSlim.ExitUpgradeableReadLock();
                }
            }
            //+
            return null;
        }

        //- ~RunOverrideProcessing -//
        internal static IHttpHandler RunOverrideProcessing(IHttpHandler hh, HttpContext context, OverrideProcessorDataList dataList)
        {
            if (dataList.Count == 0)
            {
                return null;
            }
            foreach (ProcessorData data in dataList.OrderBy(p => p.Priority))
            {
                String processorType = data.ProcessorType;
                //+
                try
                {
                    OverrideProcessor processor = null;
                    //+
                    ProcessorReaderWriterLockSlim.EnterUpgradeableReadLock();
                    if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                    {
                        ProcessorReaderWriterLockSlim.EnterWriteLock();
                        //+
                        try
                        {
                            if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                            {
                                processor = ProcessorActivator.Create<OverrideProcessor>(processorType, RouteCache.ProcessorFactoryCache);
                                if (processor == null)
                                {
                                    throw new EntityNotFoundException(String.Format(Resource.General_NotFound, processorType));
                                }
                                RouteCache.ProcessorCache.Add(processorType, processor);
                            }
                        }
                        finally
                        {
                            ProcessorReaderWriterLockSlim.ExitWriteLock();
                        }
                    }
                    if (processor == null)
                    {
                        processor = RouteCache.ProcessorCache.Get<OverrideProcessor>(processorType);
                    }
                    //+
                    if (processor != null)
                    {
                        ProcessorReaderWriterLockSlim.EnterWriteLock();
                        //+
                        try
                        {
                            if (processor != null)
                            {
                                processor.Initialize(HttpContext.Current, data.ParameterArray);
                                IHttpHandler handler = processor.Execute(hh);
                                if (handler != null)
                                {
                                    return handler;
                                }
                            }
                        }
                        finally
                        {
                            ProcessorReaderWriterLockSlim.ExitWriteLock();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (WebProcessingReportController.Reporter.Initialized)
                    {
                        var map = new Map();
                        map.Add("Section", "OverrideProcessor");
                        map.Add("Type", processorType);
                        map.Add("Message", ex.Message);
                        map.Add("Exception Type", ex.GetType().FullName);
                        //+
                        WebProcessingReportController.Reporter.AddMap(map);
                    }
                }
                finally
                {
                    ProcessorReaderWriterLockSlim.ExitUpgradeableReadLock();
                }
            }
            //+
            return null;
        }

        //- @RunStateProcessors -//
        internal static void RunStateProcessors()
        {
            HttpContext context = Http.Context;
            if (SessionStateProcessor.EntryCount > 0)
            {
                new SessionStateProcessor().Execute();
            }
            if (NalariumContext.Current.WebDomain.Configuration.StateProcessorDataList.Count == 0)
            {
                return;
            }
            foreach (ProcessorData data in NalariumContext.Current.WebDomain.Configuration.StateProcessorDataList.OrderBy(p => p.Priority))
            {
                String processorType = data.ProcessorType;
                //+
                try
                {
                    StateProcessor processor = null;
                    //+
                    ProcessorReaderWriterLockSlim.EnterUpgradeableReadLock();
                    if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                    {
                        //+
                        ProcessorReaderWriterLockSlim.EnterWriteLock();
                        try
                        {
                            if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                            {
                                processor = ProcessorActivator.Create<StateProcessor>(processorType, RouteCache.ProcessorFactoryCache);
                                if (processor == null)
                                {
                                    throw new EntityNotFoundException(String.Format(Resource.General_NotFound, processorType));
                                }
                                RouteCache.ProcessorCache.Add(processorType, processor);
                            }
                        }
                        finally
                        {
                            ProcessorReaderWriterLockSlim.ExitWriteLock();
                        }
                    }
                    if (processor == null)
                    {
                        processor = RouteCache.ProcessorCache.Get<StateProcessor>(processorType);
                    }
                    //+
                    if (processor != null)
                    {
                        ProcessorReaderWriterLockSlim.EnterWriteLock();
                        //+
                        try
                        {
                            if (processor != null)
                            {
                                //+ chain
                                do
                                {
                                    if (processor.IsChained)
                                    {
                                        processor.Initialize(HttpContext.Current, null);
                                    }
                                    else
                                    {
                                        processor.Initialize(HttpContext.Current, data.ParameterArray);
                                    }
                                    processor = processor.Execute();
                                    if (processor != null)
                                    {
                                        processor.IsChained = true;
                                    }
                                } while (processor != null);
                            }
                        }
                        finally
                        {
                            ProcessorReaderWriterLockSlim.ExitWriteLock();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (WebProcessingReportController.Reporter.Initialized)
                    {
                        var map = new Map();
                        map.Add("Section", "StateProcessor");
                        map.Add("Type", processorType);
                        map.Add("Message", ex.Message);
                        map.Add("Exception Type", ex.GetType().FullName);
                        //+
                        WebProcessingReportController.Reporter.AddMap(map);
                    }
                }
                finally
                {
                    ProcessorReaderWriterLockSlim.ExitUpgradeableReadLock();
                }
            }
        }

        internal static void RunServiceEndpointInitProcessing(HttpContext context, EndpointDataList endpointDataList)
        {
            try
            {
                if (endpointDataList.Any(p => p.Type.Equals("service", StringComparison.OrdinalIgnoreCase)))
                {
                    if (HttpRuntime.UsingIntegratedPipeline)
                    {
                        var se = new ServiceEndpointInitProcessor();
                        se.Initialize(HttpContext.Current, null);
                        se.Execute();
                    }
                    else
                    {
                        throw new PlatformNotSupportedException(Resource.Server_IntegratedPipelineRequired);
                    }
                }
            }
            catch (Exception ex)
            {
                if (WebProcessingReportController.Reporter.Initialized)
                {
                    var map = new Map();
                    map.Add("Section", "Service Endpoint");
                    map.Add("Message", ex.Message);
                    map.Add("Exception Type", ex.GetType().FullName);
                    //+
                    WebProcessingReportController.Reporter.AddMap(map);
                }
            }
        }

        //- ~RunFileEndpointInitProcessing -//
        internal static void RunFileEndpointInitProcessing(HttpContext context, EndpointDataList endpointDataList)
        {
            try
            {
                if (endpointDataList.Any(p => p.Type.Equals("file", StringComparison.OrdinalIgnoreCase)))
                {
                    var se = new FileAliasInitProcessor();
                    se.Initialize(HttpContext.Current, null);
                    se.Execute();
                }
            }
            catch (Exception ex)
            {
                if (WebProcessingReportController.Reporter.Initialized)
                {
                    var map = new Map();
                    map.Add("Section", "File");
                    map.Add("Message", ex.Message);
                    map.Add("Exception Type", ex.GetType().FullName);
                    //+
                    WebProcessingReportController.Reporter.AddMap(map);
                }
            }
        }

        //- ~RunSorting -//
        internal static void RunSorting(WebDomainData data)
        {
            ProcessorReaderWriterLockSlim.EnterWriteLock();
            try
            {
                if (data.EndpointDataList == null)
                {
                    return;
                }
                EndpointDataList handlerList = data.EndpointDataList.Clone();
                data.EndpointDataList = new EndpointDataList();
                handlerList.Where(p => p.Selector == SelectorType.PathEquals).OrderByDescending(p => p.Text.Length).ToList().ForEach(p => data.EndpointDataList.Add(p));
                handlerList.Where(p => p.Selector == SelectorType.EndsWith).OrderByDescending(p => p.Text.Length).ToList().ForEach(p => data.EndpointDataList.Add(p));
                handlerList.Where(p => p.Selector == SelectorType.WebDomainPathStartsWith).OrderByDescending(p => p.Text.Length).ToList().ForEach(p => data.EndpointDataList.Add(p));
                handlerList.Where(p => p.Selector == SelectorType.WebDomainPathEquals).OrderByDescending(p => p.Text.Length).ToList().ForEach(p => data.EndpointDataList.Add(p));
                handlerList.Where(p => p.Selector == SelectorType.PathStartsWith).OrderByDescending(p => p.Text.Length).ToList().ForEach(p => data.EndpointDataList.Add(p));
                handlerList.Where(p => p.Selector == SelectorType.StartsWith).OrderByDescending(p => p.Text.Length).ToList().ForEach(p => data.EndpointDataList.Add(p));
                handlerList.Where(p => p.Selector == SelectorType.PathContains).OrderByDescending(p => p.Text.Length).ToList().ForEach(p => data.EndpointDataList.Add(p));
                handlerList.Where(p => p.Selector == SelectorType.Contains).OrderByDescending(p => p.Text.Length).ToList().ForEach(p => data.EndpointDataList.Add(p));
            }
            finally
            {
                ProcessorReaderWriterLockSlim.ExitWriteLock();
            }
        }

        //- ~RunErrorProcessing -//
        internal static ErrorProcessor RunErrorProcessing(out ErrorProcessorData activeData)
        {
            if (NalariumContext.Current.WebDomain.Configuration.ErrorProcessorDataList.Count == 0)
            {
                activeData = null;
                return null;
            }
            ErrorProcessorDataList errorProcessorDataList = NalariumContext.Current.WebDomain.Configuration.ErrorProcessorDataList;
            //+
            foreach (ErrorProcessorData data in errorProcessorDataList.OrderBy(p => p.Priority))
            {
                String processorType = data.ProcessorType;
                try
                {
                    ErrorProcessor processor = null;
                    //+
                    ProcessorReaderWriterLockSlim.EnterUpgradeableReadLock();
                    if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                    {
                        ProcessorReaderWriterLockSlim.EnterWriteLock();
                        //+
                        try
                        {
                            if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                            {
                                processor = ProcessorActivator.Create<ErrorProcessor>(processorType, RouteCache.ProcessorFactoryCache);
                                RouteCache.ProcessorCache.Add(processorType, processor);
                            }
                        }
                        finally
                        {
                            ProcessorReaderWriterLockSlim.ExitWriteLock();
                        }
                    }
                    //+
                    processor = RouteCache.ProcessorCache.Get<ErrorProcessor>(processorType);
                    if (processor != null)
                    {
                        activeData = data;
                        //+
                        return processor;
                    }
                }
                catch (Exception ex)
                {
                    if (WebProcessingReportController.Reporter.Initialized)
                    {
                        var map = new Map();
                        map.Add("Section", "ErrorProcessor");
                        map.Add("Type", processorType);
                        map.Add("Message", ex.Message);
                        map.Add("Exception Type", ex.GetType().FullName);
                        //+
                        WebProcessingReportController.Reporter.AddMap(map);
                    }
                }
                finally
                {
                    ProcessorReaderWriterLockSlim.ExitUpgradeableReadLock();
                }
            }
            //+
            activeData = null;
            return null;
        }

        //- ~RunPostRenderProcessors -//
        internal static void RunPostRenderProcessors()
        {
            if (NalariumContext.Current.WebDomain.Configuration.PostRenderProcessorDataList.Count == 0)
            {
                return;
            }
            HttpContext context = Http.Context;
            foreach (ProcessorData data in NalariumContext.Current.WebDomain.Configuration.PostRenderProcessorDataList.OrderBy(p => p.Priority))
            {
                String processorType = data.ProcessorType;
                //+
                try
                {
                    PostRenderProcessor processor = null;
                    //+
                    ProcessorReaderWriterLockSlim.EnterUpgradeableReadLock();
                    if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                    {
                        //+
                        ProcessorReaderWriterLockSlim.EnterWriteLock();
                        try
                        {
                            if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                            {
                                processor = ProcessorActivator.Create<PostRenderProcessor>(processorType, RouteCache.ProcessorFactoryCache);
                                if (processor == null)
                                {
                                    throw new EntityNotFoundException(String.Format(Resource.General_NotFound, processorType));
                                }
                                RouteCache.ProcessorCache.Add(processorType, processor);
                            }
                        }
                        finally
                        {
                            ProcessorReaderWriterLockSlim.ExitWriteLock();
                        }
                    }
                    if (processor == null)
                    {
                        processor = RouteCache.ProcessorCache.Get<PostRenderProcessor>(processorType);
                    }
                    //+
                    if (processor != null)
                    {
                        ProcessorReaderWriterLockSlim.EnterWriteLock();
                        //+
                        try
                        {
                            if (processor != null)
                            {
                                //+ chain
                                do
                                {
                                    if (!processor.IsChained)
                                    {
                                        processor.Initialize(HttpContext.Current, data.ParameterArray);
                                    }
                                    else
                                    {
                                        processor.Initialize(HttpContext.Current, null);
                                    }
                                    processor = processor.Execute();
                                    if (processor != null)
                                    {
                                        processor.IsChained = true;
                                    }
                                } while (processor != null);
                            }
                        }
                        finally
                        {
                            ProcessorReaderWriterLockSlim.ExitWriteLock();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (WebProcessingReportController.Reporter.Initialized)
                    {
                        var map = new Map();
                        map.Add("Section", "PostRenderProcessor");
                        map.Add("Type", processorType);
                        map.Add("Message", ex.Message);
                        map.Add("Exception Type", ex.GetType().FullName);
                        //+
                        WebProcessingReportController.Reporter.AddMap(map);
                    }
                }
                finally
                {
                    ProcessorReaderWriterLockSlim.ExitUpgradeableReadLock();
                }
            }
        }

        //- ~CheckForExclusionAndForcePassThrough -//
        internal static void CheckForExclusionAndForcePassThrough(WebDomainData data)
        {
            foreach (EndpointData endpointData in data.EndpointDataList.Where(p => p.Type.Equals("{exclusion}", StringComparison.InvariantCultureIgnoreCase)))
            {
                if (PathMatcher.Match(endpointData.Selector, endpointData.Text))
                {
                    FlowControl.IsHalted = true;
                    return;
                }
            }
            foreach (EndpointData endpointData in data.EndpointDataList.Where(p => p.Type.Equals("{forcepassthrough}", StringComparison.InvariantCultureIgnoreCase)))
            {
                if (PathMatcher.Match(endpointData.Selector, endpointData.Text))
                {
                    PassThroughHttpHandler.ForceUse = true;
                    return;
                }
            }
        }
    }
}