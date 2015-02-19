#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using Nalarium.Activation;
using Nalarium.Configuration.AppConfig;
using Nalarium.Web.Globalization;
using Nalarium.Web.Processing.Configuration;
using Nalarium.Web.Processing.Data;
using Nalarium.Web.Security;
using ParameterCollection = Nalarium.Web.Processing.Configuration.ParameterCollection;
using ParameterElement = Nalarium.Web.Processing.Configuration.ParameterElement;
using Nalarium.Configuration;

namespace Nalarium.Web.Processing
{
    internal static class ConfigurationLoader
    {
        //- ~Info -//

        //+
        private static ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

        //+
        //- ~InitWebDomain -//
        internal static void InitWebDomain(String webDomainName, WebDomainElement webDomainElement, WebDomainDataList webDomainDataList)
        {
            //+ based on
            WebDomainData data = null;
            if (!webDomainElement.IsAbstract && webDomainName != Info.Root)
            {
                if (String.IsNullOrEmpty(webDomainElement.Path) && String.IsNullOrEmpty(webDomainElement.Subdomain))
                {
                    throw new ConfigurationErrorsException(Resource.WebDomain_PathAndSubdomainNotFound);
                }
            }
            if (!String.IsNullOrEmpty(webDomainElement.BasedOn))
            {
                data = CopyWebDomain(webDomainElement.BasedOn.ToLower(CultureInfo.CurrentCulture), webDomainDataList);
                if (data == null)
                {
                    throw new InvalidOperationException(String.Format(Resource.WebDomain_Invalid, webDomainElement.BasedOn));
                }
                data.BasedOn = webDomainElement.BasedOn;
                //++ non-subdomain web domain based on subdomain web domain?
                if (String.IsNullOrEmpty(webDomainElement.Subdomain) && !String.IsNullOrEmpty(data.Subdomain))
                {
                    data.Subdomain = String.Empty;
                }
            }
            if (data == null)
            {
                data = new WebDomainData();
            }
            //++ important note:
            //++  there is no way to know if the person wants to set false or just didn't set it.
            //++  therefore, you cannot disable this setting in web domain inheritance.  You must 
            //++  create the web domain fresh.
            data.CatchAllMode = webDomainElement.CatchAllMode;
            data.CatchAllInitParameter = webDomainElement.CatchAllInitParameter;
            data.Name = webDomainName;
            if (!String.IsNullOrEmpty(webDomainElement.AccessRuleGroup))
            {
                data.AccessRuleGroup = webDomainElement.AccessRuleGroup;
            }
            if (!String.IsNullOrEmpty(webDomainElement.Default))
            {
                DefaultType type;
                String parameter;
                String customParameter;
                ParseDefault(webDomainElement.Default, webDomainElement.DefaultParameter, out parameter, out type, out customParameter);
                //+
                if (String.IsNullOrEmpty(parameter))
                {
                    parameter = webDomainElement.DefaultParameter;
                }
                if (!String.IsNullOrEmpty(customParameter))
                {
                    data.CustomParameter = customParameter;
                }
                data.DefaultParameter = UrlCleaner.CleanWebPathHead(parameter);
                data.DefaultType = type;
            }
            if (webDomainName == Info.Root)
            {
                data.Path = String.Empty;
                data.Subdomain = String.Empty;
            }
            else
            {
                data.Path = UrlCleaner.CleanWebPath(webDomainElement.Path.ToLower(CultureInfo.CurrentCulture));
                data.Subdomain = webDomainElement.Subdomain;
            }
            data.IsSealed = webDomainElement.IsSealed;
            data.ProcessorDataList = new ProcessorDataList();
            data.FactoryDataList = new FactoryDataList();
            //+ parameter
            data.ParameterDataList = GetWebDomainParameterData(webDomainElement.Parameters);
            //+ reset
            ResetFlags flags = ResetFlagReader.Read(webDomainElement.ResetSeries);
            if (data.ComponentDataList == null || (flags & ResetFlags.Component) == ResetFlags.Component)
            {
                data.ComponentDataList = new ComponentDataList();
            }
            if (data.InitProcessorDataList == null || (flags & ResetFlags.Init) == ResetFlags.Init)
            {
                data.InitProcessorDataList = new InitProcessorDataList();
            }
            if (data.ErrorProcessorDataList == null || (flags & ResetFlags.Error) == ResetFlags.Error)
            {
                data.ErrorProcessorDataList = new ErrorProcessorDataList();
            }
            if (data.SelectionProcessorDataList == null || (flags & ResetFlags.Selection) == ResetFlags.Selection)
            {
                data.SelectionProcessorDataList = new SelectionProcessorDataList();
            }
            if (data.OverrideProcessorDataList == null || (flags & ResetFlags.Override) == ResetFlags.Override)
            {
                data.OverrideProcessorDataList = new OverrideProcessorDataList();
            }
            if (data.StateProcessorDataList == null || (flags & ResetFlags.State) == ResetFlags.State)
            {
                data.StateProcessorDataList = new StateProcessorDataList();
            }
            if (data.PostRenderProcessorDataList == null || (flags & ResetFlags.PostRender) == ResetFlags.PostRender)
            {
                data.PostRenderProcessorDataList = new PostRenderProcessorDataList();
            }
            if (data.HandlerFactoryDataList == null || (flags & ResetFlags.HandlerFactory) == ResetFlags.HandlerFactory)
            {
                data.HandlerFactoryDataList = new EndpointFactoryDataList();
            }
            if (data.ProcessorFactoryDataList == null || (flags & ResetFlags.ProcessorFactory) == ResetFlags.ProcessorFactory)
            {
                data.ProcessorFactoryDataList = new ProcessorFactoryDataList();
            }
            if (data.EndpointDataList == null || (flags & ResetFlags.Endpoint) == ResetFlags.Endpoint)
            {
                data.EndpointDataList = new EndpointDataList();
            }
            if (data.ObjectFactoryDataList == null || (flags & ResetFlags.ObjectFactory) == ResetFlags.ObjectFactory)
            {
                data.ObjectFactoryDataList = new ObjectFactoryDataList();
            }
            if (data.SecurityData == null || (flags & ResetFlags.Security) == ResetFlags.Security)
            {
                data.SecurityData = new SecurityData();
            }
            if (data.SecurityData.SecurityExceptionDataList == null)
            {
                data.SecurityData.SecurityExceptionDataList = new SecurityExceptionDataList();
            }
            if (data.CatchAllEndpoint == null)
            {
                data.CatchAllEndpoint = new EndpointData();
            }
            //+ rule
            //LoadAccessRuleData(data, webDomainElement.AccessRules);
            //+ component
            LoadComponentData(data, webDomainElement.Components, webDomainElement.BasedOn);
            //+ factory
            LoadFactoryData(data, webDomainElement.Factories);
            //+ processor
            LoadProcessorData(data, webDomainElement.Processors);
            //+ handler
            LoadEndpointData(data, webDomainElement.Endpoints);
            //+ favicon
            FaviconMode faviconMode = ProcessingSection.GetConfigSection().WebDomain.FaviconMode;
            switch (faviconMode)
            {
                case FaviconMode.PassThrough:
                    data.EndpointDataList.Add(EndpointData.Create(SelectorType.EndsWith, "/Favicon.ico", "{ForcePassThrough}"));
                    PassThroughHttpHandler.ForceUse = true;
                    break;
                case FaviconMode.Exclusion:
                    data.EndpointDataList.Add(EndpointData.Create(SelectorType.EndsWith, "/Favicon.ico", "{Exclusion}"));
                    break;
            }
            data.EndpointDataList.Add(EndpointData.Create(SelectorType.Contains, "/WebResource.axd?d=", "{Exclusion}"));
            //+ security
            LoadSecurityData(data, webDomainElement.Security);
            //+
            webDomainDataList.Add(data);
        }

        private static void ParseDefault(String text, String specifiedParameter, out String actualParameter, out DefaultType type, out String customParameter)
        {
            customParameter = String.Empty;
            //++
            //TODO: Consider creating a markup extension system for this.
            //++
            if (text.Equals("page", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DefaultType.Page;
                actualParameter = specifiedParameter;
            }
            else if (text.Equals("sequence", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DefaultType.Sequence;
                actualParameter = specifiedParameter;
            }
            else if (text.Equals("mvc", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DefaultType.Mvc;
                actualParameter = String.Empty;
            }
            else if (text.StartsWith("{page ", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DefaultType.Page;
                //+
                Int32 indexOfSpace = text.IndexOf(' ');
                actualParameter = text.Substring(indexOfSpace + 1, text.Length - indexOfSpace - 2);
            }
            else if (text.StartsWith("{sequence ", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DefaultType.Sequence;
                //+
                Int32 indexOfSpace = text.IndexOf(' ');
                actualParameter = text.Substring(indexOfSpace + 1, text.Length - indexOfSpace - 2);
            }
            else if (text.StartsWith("{handler ", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DefaultType.Handler;
                //+
                Int32 indexOfSpace = text.IndexOf(' ');
                actualParameter = text.Substring(indexOfSpace + 1, text.Length - indexOfSpace - 2);
            }
            else if (text.EndsWith("}") && text.Contains(" "))
            {
                type = DefaultType.Handler;
                //+
                Int32 indexOfSpace = text.IndexOf(' ');
                customParameter = text.Substring(1, indexOfSpace - 1);
                actualParameter = text.Substring(indexOfSpace + 1, text.Length - indexOfSpace - 2);
            }
            else if (text.StartsWith("{", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DefaultType.Sequence;
                actualParameter = text;
            }
            else if (text.Contains("/"))
            {
                type = DefaultType.Page;
                actualParameter = text;
            }
            else
            {
                type = DefaultType.Unknown;
                actualParameter = String.Empty;
            }
        }

        //- ~LoadSecurityData -//
        internal static void LoadSecurityData(WebDomainData data, SecurityElement securityElement)
        {
            if (securityElement.Disabled || securityElement.DefaultAccessMode == null)
            {
                data.SecurityData = new SecurityData
                                    {
                                        Disabled = true
                                    };
                //+
                return;
            }
            data.SecurityData.Disabled = false;
            data.SecurityData.DefaultAccessMode = securityElement.DefaultAccessMode ?? DefaultAccessMode.Block;
            data.SecurityData.ValidatorType = securityElement.ValidatorType;
            data.SecurityData.LoginText = securityElement.LoginText;
            data.SecurityData.LogoutText = securityElement.LogoutText;
            data.SecurityData.DefaultLoggedInTarget = securityElement.DefaultLoggedInTarget;
            SecurityExceptionCollection collection = securityElement.Exceptions;
            if (String.IsNullOrEmpty(securityElement.LoginPage))
            {
                throw new ArgumentException(ResourceAccessor.GetString("Security_LoginTargetRequired"));
            }
            data.SecurityData.LoginPage = securityElement.LoginPage;
            data.SecurityData.LogoutPage = securityElement.LogoutPage;
            List<SecurityExceptionElement> elementList = collection.ToList();
            foreach (SecurityExceptionElement element in elementList)
            {
                if (data.SecurityData.SecurityExceptionDataList.Any(p => p.Key == element.Key))
                {
                    continue;
                }
                data.SecurityData.SecurityExceptionDataList.Add(new SecurityExceptionData
                                                                {
                                                                    Key = element.Key
                                                                });
            }
            String type;
            if (data.SecurityData.LoginText.StartsWith("{"))
            {
                type = "sequence";
            }
            else
            {
                type = "page";
            }
            data.EndpointDataList.Insert(0, new EndpointData
                                            {
                                                Type = type,
                                                Text = UrlCleaner.CleanWebPathTail(data.SecurityData.LoginText) + "/",
                                                TextWithoutSlash = UrlCleaner.CleanWebPathTail(data.SecurityData.LoginText),
                                                Selector = SelectorType.EndsWith,
                                                ParameterValue = data.SecurityData.LoginPage
                                            });
            if (!String.IsNullOrEmpty(data.SecurityData.LogoutPage))
            {
                data.EndpointDataList.Insert(0, new EndpointData
                                                {
                                                    Type = type,
                                                    Text = UrlCleaner.CleanWebPathTail(data.SecurityData.LogoutText) + "/",
                                                    TextWithoutSlash = UrlCleaner.CleanWebPathTail(data.SecurityData.LogoutText),
                                                    Selector = SelectorType.EndsWith,
                                                    ParameterValue = data.SecurityData.LogoutPage
                                                });
            }
        }

        internal static void LoadComponentData(WebDomainData data, ComponentCollection componentCollection, String webDomainBasedOn)
        {
            List<ComponentElement> componentList = componentCollection.OrderBy(p => p.Priority).ToList();
            foreach (ComponentElement element in componentList)
            {
                ComponentData activeComponent = null;
                String componentType = element.ComponentType;
                String key = element.Key;
                ParameterDataList parameterDataList;
                //+ load
                if (String.IsNullOrEmpty(webDomainBasedOn))
                {
                    parameterDataList = GetComponentParameterData(element.Parameters);
                    //+
                    activeComponent = new ComponentData
                                      {
                                          ComponentType = componentType,
                                          Key = key,
                                          ParameterDataList = parameterDataList
                                      };
                    //+ No base
                    data.ComponentDataList.Add(activeComponent);
                }
                else
                {
                    //+ Has base
                    ComponentElement newElement = element;
                    //+ find new
                    ComponentData existingData = data.ComponentDataList.FirstOrDefault(p => p.Key == key);
                    componentType = existingData.ComponentType;
                    activeComponent = existingData;
                    if (existingData == null)
                    {
                        continue;
                    }
                    if (String.IsNullOrEmpty(existingData.ComponentType))
                    {
                        throw new InvalidOperationException(Resource.General_NotFound);
                    }
                    //+ copy new
                    parameterDataList = existingData.ParameterDataList;
                    if (newElement.Parameters.ResetCollection)
                    {
                        parameterDataList = GetComponentParameterData(newElement.Parameters);
                    }
                    Boolean hasDifferentParameter = false;
                    ComponentParameterCollection parameterCollection = newElement.Parameters;
                    foreach (ParameterElement parameterElement in parameterCollection)
                    {
                        ParameterData existingParameterData = parameterDataList.FirstOrDefault(p => p.Name == parameterElement.Name);
                        if (existingParameterData != null)
                        {
                            if (!parameterElement.Value.Equals(existingParameterData.Value))
                            {
                                hasDifferentParameter = true;
                            }
                            existingParameterData.Value = parameterElement.Value;
                        }
                    }
                    if (hasDifferentParameter)
                    {
                        RemoveEachPartInstalledByComponent(data, key);
                    }
                }
                //+
                try
                {
                    var component = ObjectCreator.CreateAs<Component>(componentType);
                    if (component == null)
                    {
                        throw new EntityNotFoundException(String.Format(Resource.General_NotFound, componentType));
                    }
                    component.Key = key;
                    //+
                    if (component != null)
                    {
                        if (!activeComponent.IsInstalled)
                        {
                            component.Connect(data.FactoryDataList, data.ProcessorDataList, data.EndpointDataList);
                            component.ParameterMap = parameterDataList.GetMap();
                            component.Register();
                            activeComponent.IsInstalled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (WebProcessingReportController.Reporter.Initialized)
                    {
                        var map = new Map();
                        map.Add("Section", "Component");
                        map.Add("Type", componentType);
                        map.Add("Message", ex.Message);
                        map.Add("Exception Type", ex.GetType().FullName);
                        //+
                        WebProcessingReportController.Reporter.AddMap(map);
                    }
                }
            }
        }

        //- $RemoveEachPartInstalledByComponent -//
        private static void RemoveEachPartInstalledByComponent(WebDomainData data, String componentKey)
        {
            data.HandlerFactoryDataList.RemoveAll(f => !String.IsNullOrEmpty(f.Source) && f.Source.Equals(componentKey));
            data.ProcessorFactoryDataList.RemoveAll(f => !String.IsNullOrEmpty(f.Source) && f.Source.Equals(componentKey));
            //+
            data.InitProcessorDataList.RemoveAll(f => !String.IsNullOrEmpty(f.Source) && f.Source.Equals(componentKey));
            data.SelectionProcessorDataList.RemoveAll(f => !String.IsNullOrEmpty(f.Source) && f.Source.Equals(componentKey));
            data.OverrideProcessorDataList.RemoveAll(f => !String.IsNullOrEmpty(f.Source) && f.Source.Equals(componentKey));
            data.StateProcessorDataList.RemoveAll(f => !String.IsNullOrEmpty(f.Source) && f.Source.Equals(componentKey));
            data.ErrorProcessorDataList.RemoveAll(f => !String.IsNullOrEmpty(f.Source) && f.Source.Equals(componentKey));
            //+
            data.EndpointDataList.RemoveAll(f => !String.IsNullOrEmpty(f.Source) && f.Source.Equals(componentKey));
        }

        //- ~LoadFactoryData -//
        internal static void LoadFactoryData(WebDomainData data, Nalarium.Configuration.AppConfig.Factory.FactoryCollection collection)
        {
            List<Nalarium.Configuration.AppConfig.Factory.FactoryElement> elementList = collection.ToList();
            foreach (Nalarium.Configuration.AppConfig.Factory.FactoryElement factory in elementList)
            {
                if (!factory.Enabled)
                {
                    continue;
                }
                LoadSingleFactoryData(data, factory.FactoryType, factory.GetParameterArray(), factory.GetParameterMap(), String.Empty);
            }
            foreach (FactoryData factory in data.FactoryDataList)
            {
                LoadSingleFactoryData(data, factory.FactoryType, factory.ParameterArray, factory.ParameterMap, factory.Source);
            }
            //+
            data.HandlerFactoryDataList.OriginalCount = data.HandlerFactoryDataList.Count;
            data.ProcessorFactoryDataList.OriginalCount = data.ProcessorFactoryDataList.Count;
        }

        //- $LoadSingleFactoryData -//
        private static void LoadSingleFactoryData(WebDomainData data, String factoryType, Object[] parameterArray, Map parameterMap, String source)
        {
            var readerWriterLockSlim = new ReaderWriterLockSlim();
            try
            {
                IFactory factory = null;
                //+
                readerWriterLockSlim.EnterUpgradeableReadLock();
                if (!RouteCache.HandlerFactoryCache.ContainsKey(factoryType) && !RouteCache.ProcessorFactoryCache.ContainsKey(factoryType))
                {
                    readerWriterLockSlim.EnterWriteLock();
                    //+
                    try
                    {
                        if (!RouteCache.HandlerFactoryCache.ContainsKey(factoryType) && !RouteCache.ProcessorFactoryCache.ContainsKey(factoryType))
                        {
                            factory = ObjectCreator.CreateAs<IFactory>(factoryType);
                            if (factory == null)
                            {
                                throw new InvalidFactoryException(String.Format(Resource.Factory_Invalid, factoryType));
                            }
                            //+
                            FactoryData factoryData = FactoryData.Create(factoryType, parameterArray, parameterMap);
                            factoryData.Source = String.IsNullOrEmpty(source) ? Info.System : source;
                            if (factory is HandlerFactory)
                            {
                                data.HandlerFactoryDataList.Add(factoryData);
                                RouteCache.HandlerFactoryCache.Add(factoryType, factory);
                            }
                            else if (factory is ProcessorFactory)
                            {
                                data.ProcessorFactoryDataList.Add(factoryData);
                                RouteCache.ProcessorFactoryCache.Add(factoryType, factory);
                            }
                        }
                    }
                    finally
                    {
                        readerWriterLockSlim.ExitWriteLock();
                    }
                }
            }
            catch (Exception ex)
            {
                if (WebProcessingReportController.Reporter.Initialized)
                {
                    var map = new Map();
                    map.Add("Section", "Factory");
                    map.Add("Type", factoryType);
                    map.Add("Message", ex.Message);
                    map.Add("Exception Type", ex.GetType().FullName);
                    //+
                    WebProcessingReportController.Reporter.AddMap(map);
                }
            }
            finally
            {
                readerWriterLockSlim.ExitUpgradeableReadLock();
            }
        }

        //- ~LoadProcessorData -//
        internal static void LoadProcessorData(WebDomainData data, ProcessorCollection collection)
        {
            var readerWriterLockSlim = new ReaderWriterLockSlim();
            List<ProcessorElement> elementList = collection.ToList();
            foreach (ProcessorElement processor in elementList)
            {
                if (!processor.Enabled)
                {
                    continue;
                }
                LoadSingleProcessorData(data, processor.ProcessorType, processor.GetParameterArray(), String.Empty);
            }
            foreach (ProcessorData processor in data.ProcessorDataList)
            {
                LoadSingleProcessorData(data, processor.ProcessorType, processor.ParameterArray, processor.Source);
            }
            //+
            data.InitProcessorDataList.OriginalCount = data.InitProcessorDataList.Count;
            data.SelectionProcessorDataList.OriginalCount = data.SelectionProcessorDataList.Count;
            data.OverrideProcessorDataList.OriginalCount = data.OverrideProcessorDataList.Count;
            data.StateProcessorDataList.OriginalCount = data.StateProcessorDataList.Count;
            data.ErrorProcessorDataList.OriginalCount = data.ErrorProcessorDataList.Count;
        }

        private static void LoadSingleProcessorData(WebDomainData data, String processorType, Object[] parameterArray, String source)
        {
            ProcessEachSettingToken(parameterArray);
            //+
            var readerWriterLockSlim = new ReaderWriterLockSlim();
            try
            {
                readerWriterLockSlim.EnterUpgradeableReadLock();
                try
                {
                    IProcessor processor = null;
                    //+
                    if (RouteCache.ProcessorCache.ContainsKey(processorType))
                    {
                        processor = RouteCache.ProcessorCache[processorType];
                    }
                    else
                    {
                        readerWriterLockSlim.EnterWriteLock();
                        //+
                        try
                        {
                            if (!RouteCache.ProcessorCache.ContainsKey(processorType))
                            {
                                processor = ProcessorActivator.Create<IProcessor>(processorType, RouteCache.ProcessorFactoryCache);
                                if (processor == null)
                                {
                                    throw new InvalidProcessorException(String.Format(Resource.Processor_Invalid, processorType));
                                }
                                //+
                                RouteCache.ProcessorCache.Add(processorType, processor);
                            }
                        }
                        finally
                        {
                            readerWriterLockSlim.ExitWriteLock();
                        }
                    }
                    if (processor == null)
                    {
                        return;
                    }
                    ProcessorData processorData;
                    if (processor is InitProcessor)
                    {
                        processorData = new ProcessorData
                                        {
                                            ProcessorType = processorType, ParameterArray = parameterArray
                                        };
                        processorData.Source = String.IsNullOrEmpty(source) ? Info.System : source;
                        data.InitProcessorDataList.Add(processorData);
                    }
                    else if (processor is SelectionProcessor)
                    {
                        processorData = new ProcessorData
                                        {
                                            ProcessorType = processorType, ParameterArray = parameterArray
                                        };
                        processorData.Source = String.IsNullOrEmpty(source) ? Info.System : source;
                        data.SelectionProcessorDataList.Add(processorData);
                    }
                    else if (processor is OverrideProcessor)
                    {
                        processorData = new ProcessorData
                                        {
                                            ProcessorType = processorType, ParameterArray = parameterArray
                                        };
                        processorData.Source = String.IsNullOrEmpty(source) ? Info.System : source;
                        data.OverrideProcessorDataList.Add(processorData);
                    }
                    else if (processor is StateProcessor)
                    {
                        processorData = new ProcessorData
                                        {
                                            ProcessorType = processorType, ParameterArray = parameterArray
                                        };
                        processorData.Source = String.IsNullOrEmpty(source) ? Info.System : source;
                        data.StateProcessorDataList.Add(processorData);
                    }
                    else if (processor is PostRenderProcessor)
                    {
                        processorData = new ProcessorData
                                        {
                                            ProcessorType = processorType, ParameterArray = parameterArray
                                        };
                        processorData.Source = String.IsNullOrEmpty(source) ? Info.System : source;
                        data.PostRenderProcessorDataList.Add(processorData);
                    }
                    else if (processor is ErrorProcessor)
                    {
                        processorData = new ErrorProcessorData
                                        {
                                            ProcessorType = processorType, ParameterArray = parameterArray
                                        };
                        processorData.Source = String.IsNullOrEmpty(source) ? Info.System : source;
                        var epd = ((ErrorProcessorData)processorData);
                        epd.Init();
                        data.ErrorProcessorDataList.Add(epd);
                    }
                }
                finally
                {
                    readerWriterLockSlim.ExitUpgradeableReadLock();
                }
            }
            catch (Exception ex)
            {
                if (WebProcessingReportController.Reporter.Initialized)
                {
                    var map = new Map();
                    map.Add("Section", "Processor");
                    map.Add("Type", processorType);
                    map.Add("Message", ex.Message);
                    map.Add("Exception Type", ex.GetType().FullName);
                    //+
                    WebProcessingReportController.Reporter.AddMap(map);
                }
            }
        }

        //- $ProcessEachSettingToken -//
        private static void ProcessEachSettingToken(Object[] parameterArray)
        {
            if (parameterArray == null)
            {
                return;
            }
            for (Int32 i = 0; i < parameterArray.Length; i++)
            {
                var parameter = parameterArray[i] as String;
                if (parameter == null)
                {
                    continue;
                }
                parameterArray[i] = ProcessSingleSettingToken(parameter);
            }
        }

        //- $ProcessSingleSettingToken -//
        private static string ProcessSingleSettingToken(String parameter)
        {
            if (parameter[0] == '{' && parameter[parameter.Length - 1] == '}')
            {
                parameter = parameter.Substring(1, parameter.Length - 2);
                if (parameter.StartsWith("AppSetting ", StringComparison.CurrentCultureIgnoreCase))
                {
                    parameter = parameter.Substring(11, parameter.Length - 11);
                    return ConfigAccessor.ApplicationSettings(parameter) ?? String.Empty;
                }
                else if (parameter.StartsWith("ConnectionString ", StringComparison.CurrentCultureIgnoreCase))
                {
                    parameter = parameter.Substring(17, parameter.Length - 17);
                    return ConfigAccessor.ConnectionString(parameter) ?? String.Empty;
                }
            }
            //+
            return parameter;
        }

        //- ~LoadEndpointData -//
        internal static void LoadEndpointData(WebDomainData data, EndpointCollection collection)
        {
            List<EndpointElement> elementList = collection.ToList();
            var matchTextList = new List<String>();
            var referenceKeyList = new List<String>();
            if (collection.Count(p => p.Text == "*") > 1)
            {
                throw new ConfigurationErrorsException(ResourceAccessor.GetString("WebDomain_DuplicateCatchAll"));
            }
            foreach (EndpointElement element in elementList)
            {
                if (element.Disabled)
                {
                    continue;
                }
                String matchText = element.Text;
                Boolean requireSlash = element.RequireSlash;
                String withoutSlash = EndpointData.GetTextWithoutSlash(matchText);
                SelectorType matchType = element.Selector;
                String originalMatchText = matchText;
                EndpointData newElement = AdjustMatchType(element.Text);
                if (newElement != null)
                {
                    matchText = newElement.Text;
                    matchType = newElement.Selector;
                }
                matchTextList.Add(matchText);
                if (element.Text == "*")
                {
                    data.CatchAllEndpoint = new EndpointData
                                            {
                                                AccessRuleGroup = element.AccessRuleGroup,
                                                OriginalMatchText = originalMatchText,
                                                Text = matchText,
                                                TextWithoutSlash = withoutSlash,
                                                Selector = matchType,
                                                Type = element.Type,
                                                ParameterValue = element.Parameter,
                                                ParameterMap = element.GetParameterMap(),
                                                Source = Info.System
                                            };
                }
                else
                {
                    var endpointData = new EndpointData
                                       {
                                           AccessRuleGroup = element.AccessRuleGroup,
                                           OriginalMatchText = originalMatchText,
                                           Text = matchText,
                                           TextWithoutSlash = withoutSlash,
                                           Selector = matchType,
                                           Type = element.Type,
                                           RequireSlash = element.RequireSlash,
                                           ParameterValue = element.Parameter,
                                           ParameterMap = element.GetParameterMap(),
                                           Source = Info.System
                                       };
                    endpointData.SubEndpointDataList = new EndpointDataList();
                    foreach (EndpointElement subElement in element.SubEndpoints)
                    {
                        String subWithoutSlash = EndpointData.GetTextWithoutSlash(matchText);
                        endpointData.SubEndpointDataList.Add(new EndpointData
                                                             {
                                                                 Text = subElement.Text,
                                                                 TextWithoutSlash = subWithoutSlash,
                                                                 Selector = subElement.Selector,
                                                                 Type = subElement.Type,
                                                                 ParameterValue = subElement.Parameter,
                                                                 ParameterMap = subElement.GetParameterMap(),
                                                                 Source = Info.System
                                                             });
                    }
                    data.EndpointDataList.Add(endpointData);
                }
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
            //+
            data.EndpointDataList.OriginalCount = data.EndpointDataList.Count;
        }

        //- ~LoadSequenceData -//
        internal static void LoadSequenceData(SequenceDataList data, SequenceCollection collection)
        {
            List<SequenceElement> elementList = collection.ToList();
            foreach (SequenceElement element in elementList)
            {
                if (data.Any(p => p.Name == element.Name))
                {
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, Resource.Sequence_DuplicateNameInConfig, element.Name));
                }
                var sequenceData = new SequenceData
                                   {
                                       Name = element.Name
                                   };
                sequenceData.ViewList = new ViewDataList();
                data.Add(sequenceData);
                foreach (ViewElement viewElement in element.Views)
                {
                    var viewData = new ViewData
                                   {
                                       Name = viewElement.Name,
                                       ViewUsed = viewElement.ViewUsed
                                   };
                    sequenceData.ViewList.Add(viewData);
                }
                sequenceData.VersionList = new VersionDataList();
                sequenceData.VersionList.ExplicitVersion = element.ExplicitVersion;
                foreach (VersionElement versionElement in element.Versions)
                {
                    sequenceData.VersionList.Add(new VersionData
                                                 {
                                                     Name = versionElement.Name,
                                                     Weight = versionElement.Weight
                                                 });
                }
            }
        }

        //- ~GetParameterData -//
        internal static ParameterDataList GetWebDomainParameterData(ParameterCollection collection)
        {
            List<Configuration.ParameterElement> elementList = collection.ToList();
            var dataList = new ParameterDataList();
            foreach (Configuration.ParameterElement element in elementList)
            {
                //+ parameter
                String value = ProcessSingleSettingToken(element.Value);
                dataList.Add(new ParameterData
                             {
                                 Category = element.Category,
                                 Name = element.Name,
                                 Value = value
                             });
            }
            //+
            return dataList;
        }

        //- ~GetParameterData -//
        internal static ParameterDataList GetComponentParameterData(ComponentParameterCollection collection)
        {
            List<Nalarium.Configuration.AppConfig.Parameter.ParameterElement> elementList = collection.ToList();
            var dataList = new ParameterDataList();
            foreach (ParameterElement element in elementList)
            {
                //+ parameter
                String value = ProcessSingleSettingToken(element.Value);
                dataList.Add(new ParameterData
                             {
                                 Name = element.Name,
                                 Value = value
                             });
            }
            //+
            return dataList;
        }

        //- ~CopyWebDomain -//
        internal static WebDomainData CopyWebDomain(String webDomainName, List<WebDomainData> dataList)
        {
            WebDomainData data = dataList.SingleOrDefault(p => p.Name.Equals(webDomainName, StringComparison.InvariantCultureIgnoreCase));
            if (data == null)
            {
                return null;
            }
            if (data.IsSealed)
            {
                throw new InvalidOperationException(String.Format(Resource.WebDomain_CannotInheritFromSealed, data.Name));
            }
            data.IsBasedOn = true;
            //+
            return data.Clone();
        }

        //- ~AdjustMatchType -//
        internal static EndpointData AdjustMatchType(String matchText)
        {
            EndpointData element = null;
            if (matchText.StartsWith("wdp^", StringComparison.OrdinalIgnoreCase) && matchText.EndsWith("$", StringComparison.OrdinalIgnoreCase))
            {
                element = new EndpointData();
                element.Selector = SelectorType.WebDomainPathEquals;
                element.Text = matchText.Substring(3, matchText.Length - 4);
            }
            if (matchText.StartsWith("p^", StringComparison.OrdinalIgnoreCase) && matchText.EndsWith("$", StringComparison.OrdinalIgnoreCase))
            {
                element = new EndpointData();
                element.Selector = SelectorType.PathEquals;
                element.Text = matchText.Substring(2, matchText.Length - 3);
            }
            if (matchText.StartsWith("^", StringComparison.OrdinalIgnoreCase) && matchText.EndsWith("$", StringComparison.OrdinalIgnoreCase))
            {
                element = new EndpointData();
                element.Selector = SelectorType.Equals;
                element.Text = matchText.Substring(1, matchText.Length - 2);
            }
            if (matchText.StartsWith("wdp^", StringComparison.OrdinalIgnoreCase))
            {
                element = new EndpointData();
                element.Selector = SelectorType.WebDomainPathStartsWith;
                element.Text = matchText.Substring(4, matchText.Length - 4);
            }
            if (matchText.StartsWith("p^", StringComparison.OrdinalIgnoreCase))
            {
                element = new EndpointData();
                element.Selector = SelectorType.PathStartsWith;
                element.Text = matchText.Substring(2, matchText.Length - 2);
            }
            if (matchText.EndsWith("$", StringComparison.OrdinalIgnoreCase))
            {
                element = new EndpointData();
                element.Selector = SelectorType.EndsWith;
                element.Text = matchText.Substring(0, matchText.Length - 1);
            }
            if (matchText.StartsWith("^", StringComparison.OrdinalIgnoreCase))
            {
                element = new EndpointData();
                element.Selector = SelectorType.StartsWith;
                element.Text = matchText.Substring(1, matchText.Length - 1);
            }
            //+
            return element;
        }

        #region Nested type: Info

        internal class Info
        {
            public const String System = "System";
            public const String Root = "root";
        }

        #endregion
    }
}