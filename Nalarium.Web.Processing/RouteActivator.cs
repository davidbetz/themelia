#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web;
using Nalarium.Web.Processing.Configuration;
using Nalarium.Web.Processing.Data;
//+

namespace Nalarium.Web.Processing
{
    public static class RouteActivator
    {
        //- ~Info -//

        //+ field
        internal static Boolean IsInitialized;
        internal static Object _lock = new Object();

        //- ~Create -//
        internal static IHttpHandler Create(HttpContext context)
        {
            HttpRequest request = context.Request;
            //+
            WebDomainData activeData = NalariumContext.Current.WebDomain.Configuration;
            //+ ensure missing slash text
            activeData.EndpointDataList.ForEach(p =>
                                                {
                                                    if (String.IsNullOrEmpty(p.TextWithoutSlash))
                                                    {
                                                        p.TextWithoutSlash = EndpointData.GetTextWithoutSlash(p.Text);
                                                    }
                                                });
            //+
            //ProcessorRunner.RunSorting(activeData);
            //+
            ProcessorRunner.RunInitProcessing(context, activeData.InitProcessorDataList);
            //+ skip?
            if (PassThroughHttpHandler.ForceUse)
            {
                return new PassThroughHttpHandler();
            }
            //+ service endpoint check
            ProcessorRunner.RunServiceEndpointInitProcessing(context, activeData.EndpointDataList);
            ProcessorRunner.RunFileEndpointInitProcessing(context, activeData.EndpointDataList);
            if (FlowControl.StoppingAfterInitProcessing)
            {
                return null;
            }
            //+
            var endpointContextData = new Endpoint();
            NalariumContext nalariumContext = NalariumContext.Current;
            //+ mid processing
            IHttpHandler hh = new PageProtectionSelectionProcessor().Execute(null);
            if (hh == null)
            {
                if (nalariumContext.WebDomain.Configuration.DefaultType == DefaultType.Mvc || !String.IsNullOrEmpty(HttpData.GetScopedItem<String>(DefaultPageInitProcessor.Info.Scope, DefaultPageInitProcessor.Info.DefaultParameter)))
                {
                    hh = new DefaultPageSelectionProcessor().Execute();
                }
                if (hh == null)
                {
                    hh = ProcessorRunner.RunSelectionProcessing(context, activeData.SelectionProcessorDataList);
                }
            }
            //+ router
            var router = new HttpHandlerSelector
                         {
                             //HandlerFactoryMap = RouteCache.HandlerFactoryCache
                         };
            String originalText = String.Empty;
            var originalType = (SelectorType)0;
            //+ selection
            if (hh != null)
            {
                endpointContextData.SetMode = EndpointSetMode.Selection;
            }
            else
            {
                lock (_lock)
                {
                    foreach (EndpointData ep in activeData.EndpointDataList)
                    {
                        EndpointData endpoint = ep.Clone();
                        try
                        {
                            Boolean matchWithoutTrailingSlash = false;
                            IHttpHandler hh2 = null;
                            if (Http.RawUrl.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
                            {
                                endpoint = TryCreateAndInit(router, ep, false, out hh2);
                            }
                            else if (!endpoint.RequireSlash)
                            {
                                endpoint = TryCreateAndInit(router, endpoint, true, out hh2);
                                if (!(hh2 is DummyHttpHandler))
                                {
                                    matchWithoutTrailingSlash = true;
                                }
                            }
                            if (hh2 != null && !(hh2 is DummyHttpHandler))
                            {
                                hh = hh2;
                                originalText = ep.Text;
                                originalType = ep.Selector;
                                endpointContextData.SetMode = EndpointSetMode.Normal;
                                //+
                                endpointContextData.OriginalText = originalText;
                                endpointContextData.OriginalSelector = originalType;
                                endpointContextData.Text = endpoint.Text;
                                endpointContextData.Selector = endpoint.Selector;
                                endpointContextData.MatchWithoutTrailingSlash = matchWithoutTrailingSlash;
                                endpointContextData.ParameterMap = ep.ParameterMap;
                                endpointContextData.ParameterValue = ep.ParameterValue;
                                //+
                                HttpData.SetScopedItem(Info.Scope, Info.OriginalText, originalText);
                                HttpData.SetScopedItem(Info.Scope, Info.OriginalSelector, originalType);
                                HttpData.SetScopedItem(Info.Scope, Info.Text, endpoint.Text);
                                HttpData.SetScopedItem(Info.Scope, Info.Selector, endpoint.Selector);
                                HttpData.SetScopedItem(Info.Scope, Info.MatchWithoutTrailingSlash, matchWithoutTrailingSlash);
                                //+
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (WebProcessingReportController.Reporter.Initialized)
                            {
                                var map = new Map();
                                map.Add("Section", "Endpoint");
                                map.Add("Name", endpoint.Type);
                                map.Add("MatchText", endpoint.Text);
                                map.Add("MatchType", endpoint.Selector.ToString());
                                map.Add("Message", ex.Message);
                                map.Add("Exception Type", ex.GetType().FullName);
                                //+
                                WebProcessingReportController.Reporter.AddMap(map);
                            }
                        }
                    }
                }
            }
            //+ check catch-all
            if (hh == null && !String.IsNullOrEmpty(activeData.CatchAllEndpoint.Text))
            {
                activeData.CatchAllEndpoint = TryCreateAndInit(router, activeData.CatchAllEndpoint, out hh);
                if (hh is DummyHttpHandler)
                {
                    hh = new PassThroughHttpHandler();
                }
                else
                {
                    HttpData.SetScopedItem(Info.Scope, Info.Text, "*");
                    HttpData.SetScopedItem(Info.Scope, Info.Selector, activeData.CatchAllEndpoint.Selector);
                }
            }
            //+
            if (FlowControl.OverrideProcessingSkipped == false)
            {
                IHttpHandler newHandler = null;
                newHandler = ProcessorRunner.RunOverrideProcessing(hh, context, activeData.OverrideProcessorDataList);
                if (newHandler != null)
                {
                    hh = newHandler;
                    //+
                    endpointContextData.SetMode = EndpointSetMode.Override;
                    endpointContextData.Text = String.Empty;
                    endpointContextData.Selector = 0;
                    endpointContextData.MatchWithoutTrailingSlash = false;
                }
            }
            if (hh == null)
            {
                hh = new DummyHttpHandler();
            }
            else if (nalariumContext.WebDomain.Configuration.DefaultType == DefaultType.Mvc)
            {
                endpointContextData.Text = String.Empty;
            }
            {
                nalariumContext.Endpoint = endpointContextData;
            }
            //+
            return hh;
        }

        //- $TryCreateAndInit -//
        private static EndpointData TryCreateAndInit(HttpHandlerSelector router, EndpointData endpoint, out IHttpHandler hh)
        {
            return TryCreateAndInit(router, endpoint, false, out hh);
        }

        private static EndpointData TryCreateAndInit(HttpHandlerSelector router, EndpointData endpoint, Boolean withoutSlash, out IHttpHandler hh)
        {
            return router.MatchHttpHandler(endpoint, withoutSlash, out hh);
        }

        //- ~RunSystemInitInitProcessors -//
        internal static void RunSystemInitProcessors()
        {
            if (!IsInitialized)
            {
                lock (_lock)
                {
                    new DebugInitProcessor().Execute();
                    new ConfigurationInitProcessor().Execute();
                    new AccessRuleInitProcessor().Execute();
                    new ScannedTypeCacheInitProcessor().Execute();
                    //+
                    Boolean disableProcessing = false;
                    ProcessingSection section = ProcessingSection.GetConfigSection();
                    if (section != null)
                    {
                        disableProcessing = section.DisableProcessing;
                    }
                    HttpData.SetScopedItem(Info.Scope, CoreModule.Info.DisableProcessing, disableProcessing);
                    //+
                    IsInitialized = true;
                }
            }
        }

        //- ~RunContextInitProcessors -//
        internal static void RunContextInitProcessors(HttpContext context)
        {
            //+ nalarium context
            var nalariumContext = new NalariumContext
                                  {
                                      WebDomain = new WebDomain()
                                  };
            NalariumContext.Current = nalariumContext;
            //+ web domain selection
            new WebDomainInitProcessor().Execute();
            //+ post-web domain exclusion and force passthrough
            ProcessorRunner.CheckForExclusionAndForcePassThrough(NalariumContext.Current.WebDomain.Configuration);
            if (FlowControl.IsHalted)
            {
                return;
            }
            //+ default page
            var init = new DefaultPageInitProcessor();
            init.Initialize(Http.Context);
            init.Execute();
            //+ state
            //new StateInitProcessor().Execute();
        }

        #region Nested type: Info

        public static class Info
        {
            public const String Scope = "__$Nalarium$Processing";
            //+
            internal const String OriginalText = "OriginalText";
            internal const String OriginalSelector = "OriginalSelector";
            internal const String PassThroughForceUse = "PassThroughForceUse";
            public const String Text = "Text";
            public const String Selector = "Selector";
            //public const String ReferenceKey = "ReferenceKey";
            public const String MatchWithoutTrailingSlash = "MatchWithoutTrailingSlash";
        }

        #endregion
    }
}