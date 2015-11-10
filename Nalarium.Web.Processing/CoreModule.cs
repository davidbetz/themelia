#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;
using Nalarium.Configuration.AppConfig;
using Nalarium.Web.AccessRule;
using Nalarium.Web.Processing.Configuration;
//+

namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Not for programmatic use.
    /// </summary>
    public class CoreModule : IHttpModule
    {
        //- ~Info -//

        private static Boolean _profilingEnabled;
        private static Boolean _profilingIncludeExclusions;
        //internal static Boolean IsMvpAvailable;
        //internal static Boolean IsMvcAvailable;
        //internal static Type mvcHandlerType;

        //+ field
        private Boolean _isExclusion;
        private DateTime _startDateTime;

        //+
        //- @Dispose -//

        #region IHttpModule Members

        public void Dispose()
        {
        }

        //+
        //- @Init -//
        public void Init(HttpApplication httpApplication)
        {
            RouteActivator.RunSystemInitProcessors();
            //+
            _profilingEnabled = ProcessingSection.GetConfigSection().Profiling.Enabled;
            _profilingIncludeExclusions = ProcessingSection.GetConfigSection().Profiling.IncludeExclusions;
            //+
            httpApplication.BeginRequest += OnBeginRequest;
            httpApplication.EndRequest += OnEndRequest;
            //+
            //+
            httpApplication.PostAuthorizeRequest += OnPostAuthorizeRequest;
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                httpApplication.PostResolveRequestCache += OnProcessRoute;
                httpApplication.PostMapRequestHandler += OnSetHandler;
            }
            else
            {
                httpApplication.PostMapRequestHandler += OnProcessRoute;
                httpApplication.PostMapRequestHandler += OnSetHandler;
            }
            httpApplication.PostAcquireRequestState += OnPostAcquireRequestState;
            //+
            httpApplication.Error += ErrorHandler.OnHandleError;
        }

        #endregion

        //- $BeginRequest -//
        private void OnBeginRequest(Object sender, EventArgs ea)
        {
            //+ exclusion
            new ExclusionInitProcessor().Execute();
            _isExclusion = FlowControl.IsHalted;
            HttpData.SetScopedItem(Info.Scope, "HTTP_X_ORIGINAL_URL", Http.AbsoluteUrl);
            //+ profiling
            if (_profilingEnabled && (!_isExclusion || (_isExclusion && _profilingIncludeExclusions)))
            {
                _startDateTime = DateTime.Now;
            }
        }

        //- $EndRequest -//
        private void OnEndRequest(Object sender, EventArgs ea)
        {
            if (_profilingEnabled && (!_isExclusion || (_isExclusion && _profilingIncludeExclusions)))
            {
                DateTime endDateTime = DateTime.Now;
                Double milliSeconds = (endDateTime - _startDateTime).TotalMilliseconds;
                var builder = new StringBuilder();
                builder.Append(Http.AbsolutePath + ", ");
                builder.Append(milliSeconds.ToString() + "ms");
                Debug.WriteLine(builder.ToString());
            }
        }

        //- $OnPostAuthorizeRequest -//
        private void OnPostAuthorizeRequest(Object sender, EventArgs ea)
        {
            if (_isExclusion)
            {
                return;
            }
            //+
            Boolean disableProcessing = false;
            ProcessingSection section = ProcessingSection.GetConfigSection();
            if (section != null)
            {
                disableProcessing = section.DisableProcessing;
            }
            HttpData.SetScopedItem(Info.Scope, Info.DisableProcessing, disableProcessing);
            if (disableProcessing == false)
            {
                var ha = sender as HttpApplication;
                RouteActivator.RunContextInitProcessors(ha.Context);
                _isExclusion = FlowControl.IsHalted;
                if (_isExclusion)
                {
                    return;
                }
                //+
                if (Http.IPAddress.Equals("72.47.154.93"))
                {
                    AccessRuleChecker.SystemCheck(ProcessingSection.GetConfigSection().WebDomain.AccessRuleGroup);
                }
            }
        }

        //- $OnProcessRoute -//
        private void OnProcessRoute(Object sender, EventArgs ea)
        {
            if (_isExclusion)
            {
                return;
            }
            //+
            if (HttpData.GetScopedItem<Boolean>(Info.Scope, Info.DisableProcessing) == false)
            {
                var ha = sender as HttpApplication;
                HttpContext context = ha.Context;
                //+
                FlowControl.ActiveHandler = RouteActivator.Create(context);
                if (HttpRuntime.UsingIntegratedPipeline)
                {
                    ha.Context.RemapHandler(FlowControl.ActiveHandler);
                }
                else
                {
                    ha.Context.Handler = FlowControl.ActiveHandler;
                }
            }
            //+ first send
            if (WebProcessingReportController.Reporter.Initialized)
            {
                Boolean hasDataToSend = WebProcessingReportController.Reporter.HasDataToSend;
                if (hasDataToSend)
                {
                    String name = SystemSection.GetConfigSection().AppInfo.Name;
                    if (!String.IsNullOrEmpty(name))
                    {
                        var map = new Map();
                        map.Add("App Name", name);
                        WebProcessingReportController.Reporter.InsertMap(map, 0);
                    }
                    WebProcessingReportController.Reporter.Send(true);
                    WebProcessingReportController.Reporter.ReportCreator.Clear();
                }
            }
        }

        //- $OnProcessRoute -//
        private void OnSetHandler(Object sender, EventArgs ea)
        {
            if (_isExclusion)
            {
                return;
            }
            //+
            if (HttpData.GetScopedItem<Boolean>(Info.Scope, Info.DisableProcessing) == false)
            {
                var ha = sender as HttpApplication;
                HttpContext context = ha.Context;
                //+
                FlowControl.ActiveHandler = RouteActivator.Create(context);
                if (FlowControl.ActiveHandler != null
                    && !(FlowControl.ActiveHandler is PassThroughHttpHandler)
                    && !(FlowControl.ActiveHandler is DummyHttpHandler))
                {
                    Type type = typeof(UrlRoutingModule);
                    FieldInfo fi = type.GetField("_requestDataKey", BindingFlags.Static | BindingFlags.NonPublic);
                    if (fi != null)
                    {
                        Object key = fi.GetValue(null);
                        if (key != null)
                        {
                            Object value = Http.Context.Items[key];
                            Http.Context.Items[key] = null;
                        }
                    }
                    ha.Context.Handler = FlowControl.ActiveHandler;
                }
            }
            //+ first send
            if (WebProcessingReportController.Reporter.Initialized)
            {
                Boolean hasDataToSend = WebProcessingReportController.Reporter.HasDataToSend;
                if (hasDataToSend)
                {
                    String name = SystemSection.GetConfigSection().AppInfo.Name;
                    if (!String.IsNullOrEmpty(name))
                    {
                        var map = new Map();
                        map.Add("App Name", name);
                        WebProcessingReportController.Reporter.InsertMap(map, 0);
                    }
                    WebProcessingReportController.Reporter.Send(true);
                    WebProcessingReportController.Reporter.ReportCreator.Clear();
                }
            }
        }

        //- $OnPostAcquireRequestState -//
        private void OnPostAcquireRequestState(Object sender, EventArgs ea)
        {
            if (_isExclusion)
            {
                return;
            }
            //+
            if (HttpData.GetScopedItem<Boolean>(Info.Scope, Info.DisableProcessing) == false)
            {
                if (Http.Session != null)
                {
                    //+ security
                    new SecurityStateProcessor().Execute();
                    //+
                    if (FlowControl.ActiveHandler != null && !(FlowControl.ActiveHandler is PassThroughHttpHandler) && !FlowControl.OverrideProcessingSkipped)
                    {
                        ProcessorRunner.RunStateProcessors();
                    }
                }
            }
            //+ second send
            if (WebProcessingReportController.Reporter.Initialized)
            {
                if (WebProcessingReportController.Reporter.HasDataToSend)
                {
                    var map = new Map();
                    map.Add("Begin", "Post State Processor Section");
                    WebProcessingReportController.Reporter.InsertMap(map, 0);
                    String name = SystemSection.GetConfigSection().AppInfo.Name;
                    if (!String.IsNullOrEmpty(name))
                    {
                        map = new Map();
                        map.Add("App Name", name);
                        WebProcessingReportController.Reporter.InsertMap(map, 0);
                    }
                    //+
                    WebProcessingReportController.Reporter.Send(true);
                }
            }
        }

        #region Nested type: Info

        internal static class Info
        {
            public const String Scope = "__$Nalarium$Core";
            //+
            public const String DisableProcessing = "DisableProcessing";
        }

        #endregion
    }
}