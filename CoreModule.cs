#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Web;
//+
using Nalarium.Web.Processing.Configuration;
using Nalarium.Web.Processing;
//+
namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Not for programmatic use.
    /// </summary>
    public class CoreModule : System.Web.IHttpModule
    {
        //- ~Info -//
        internal static class Info
        {
            public const String Scope = "__$Nalarium$Core";
            //+
            public const String DisableProcessing = "DisableProcessing";
        }

        private static Boolean _profilingEnabled = false;
        private static Boolean _profilingIncludeExclusions = false;
        internal static Boolean IsMvpAvailable = false;
        internal static Boolean IsMvcAvailable = false;
        internal static Type mvcHandlerType = null;

        //+ field
        private DateTime _startDateTime;
        private Boolean _isExclusion = false;

        //+
        //- @Dispose -//
        public void Dispose() { }

        //+
        //- @Init -//
        public void Init(HttpApplication httpApplication)
        {
            CheckForAssemblyRequirement();
            //+
            RouteActivator.RunSystemInitProcessors();
            //+
            _profilingEnabled = ProcessingSection.GetConfigSection().Profiling.Enabled;
            _profilingIncludeExclusions = ProcessingSection.GetConfigSection().Profiling.IncludeExclusions;
            //+
            httpApplication.BeginRequest += new EventHandler(OnBeginRequest);
            httpApplication.EndRequest += new EventHandler(OnEndRequest);
            //+
            //+
            httpApplication.PostAuthorizeRequest += new EventHandler(OnPostAuthorizeRequest);
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                httpApplication.PostResolveRequestCache += new EventHandler(OnProcessRoute);
                httpApplication.PostMapRequestHandler += new EventHandler(OnSetHandler);
            }
            else
            {
                httpApplication.PostMapRequestHandler += new EventHandler(OnProcessRoute);
                httpApplication.PostMapRequestHandler += new EventHandler(OnSetHandler);
            }
            httpApplication.PostAcquireRequestState += new EventHandler(OnPostAcquireRequestState);
            //+
            httpApplication.Error += ErrorHandler.OnHandleError;
        }

        //- $CheckForAssemblyRequirement -//
        private void CheckForAssemblyRequirement()
        {
            try
            {
                System.Reflection.Assembly.ReflectionOnlyLoad("Nalarium.Mvp");
            }
            catch (System.IO.FileNotFoundException)
            {
                IsMvpAvailable = false;
                return;
            }
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("Nalarium.Web.Mvc");
                mvcHandlerType = assembly.GetType("Nalarium.Web.Mvc.AspNetMvcHttpHandler", false);
                //if (type != null)
                //{
                //    //++
                //    //TODO: Make some type of "universal handler factory" thing or something so this is ALWAYS checked in ALL web domains.
                //    //++
                //    mvcHandlerType = Nalarium.Activation.ObjectCreator.CreateAs<HandlerFactory>(type);
                //}
            }
            catch (System.IO.FileNotFoundException)
            {
                IsMvcAvailable = false;
                return;
            }
            //+
            IsMvpAvailable = true;
            IsMvcAvailable = true;
        }

        //- $BeginRequest -//
        private void OnBeginRequest(Object sender, EventArgs ea)
        {
            //+ exclusion
            new ExclusionInitProcessor().Execute();
            _isExclusion = FlowControl.IsHalted;
            HttpData.SetScopedItem<String>(Info.Scope, "HTTP_X_ORIGINAL_URL", Http.AbsoluteUrl);
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
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.Append(Http.AbsolutePath + ", ");
                builder.Append(milliSeconds.ToString() + "ms");
                System.Diagnostics.Debug.WriteLine(builder.ToString());
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
            HttpData.SetScopedItem<Boolean>(Info.Scope, Info.DisableProcessing, disableProcessing);
            if (disableProcessing == false)
            {
                HttpApplication ha = sender as HttpApplication;
                RouteActivator.RunContextInitProcessors(ha.Context);
                _isExclusion = FlowControl.IsHalted;
                if (_isExclusion)
                {
                    return;
                }
                //+
                if (Http.IPAddress.Equals("72.47.154.93"))
                {
                    AccessRule.AccessRuleChecker.SystemCheck(Nalarium.Web.Processing.Configuration.ProcessingSection.GetConfigSection().WebDomain.AccessRuleGroup);
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
                HttpApplication ha = sender as HttpApplication;
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
                    String name = Nalarium.Configuration.SystemSection.GetConfigSection().AppInfo.Name;
                    if (!String.IsNullOrEmpty(name))
                    {
                        Map map = new Map();
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
                HttpApplication ha = sender as HttpApplication;
                HttpContext context = ha.Context;
                //+
                FlowControl.ActiveHandler = RouteActivator.Create(context);
                if (FlowControl.ActiveHandler != null
                    && !(FlowControl.ActiveHandler is PassThroughHttpHandler)
                    && !(FlowControl.ActiveHandler is DummyHttpHandler))
                {
                    Type type = typeof(System.Web.Routing.UrlRoutingModule);
                    System.Reflection.FieldInfo fi = type.GetField("_requestDataKey", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
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
                    String name = Nalarium.Configuration.SystemSection.GetConfigSection().AppInfo.Name;
                    if (!String.IsNullOrEmpty(name))
                    {
                        Map map = new Map();
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
                    Map map = new Map();
                    map.Add("Begin", "Post State Processor Section");
                    WebProcessingReportController.Reporter.InsertMap(map, 0);
                    String name = Nalarium.Configuration.SystemSection.GetConfigSection().AppInfo.Name;
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
    }
}