#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Linq;
//+
using Nalarium.Web.Processing.Data;
//+
namespace Nalarium.Web.Processing
{
    internal class WebDomainInitProcessor : ContextInitProcessor
    {
        //- @Setting -//
        public static class Setting
        {
            public const String WebDomain = "WebDomain";
        }

        //- @OnPreHttpHandlerExecute -//
        public override InitProcessor Execute()
        {
            WebDomain webDomain = new WebDomain();
            NalariumContext.Current.WebDomain = webDomain;
            //if (webDomain.Configuration == null)
            //{
            WebDomainDataList webDomainDataList = WebDomainDataList.AllWebDomainData;
            if (WebDomainDataList.AllWebDomainData.Count > 0)
            {
                //+ subdomain
                var subdomainData = webDomainDataList.Where(u => !String.IsNullOrEmpty(u.Subdomain) && Http.Domain.StartsWith(u.Subdomain, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (subdomainData.Count > 0)
                {
                    SelectWebDomain(subdomainData);
                    //if (webDomain.Configuration == null)
                    //{
                    //++ take one with no path or with "/" as path
                    webDomain.Configuration = subdomainData.Where(p => String.IsNullOrEmpty(p.Path) || p.Path.Equals("/", StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(p => p.Path.Length).FirstOrDefault();
                    //if (webDomain.Configuration == null)
                    //{
                    //    //++ take one with longest path, if more than one
                    //    webDomain.Configuration = subdomainData.OrderByDescending(p => p.Path.Length).First();
                    //}
                    //}
                }
                if (webDomain.Configuration == null)
                {
                    SelectWebDomain(webDomainDataList);
                }
            }
            if (webDomain.Configuration == null)
            {
                webDomain.Configuration = webDomainDataList.SingleOrDefault(p => p.Name == "root");
                webDomain.Configuration.FoundWithoutTrailingSlash = false;
            }
            //else
            //{
            //    NalariumContext.Current.WebDomain = webDomain;
            //}
            //}
            if (webDomain.Configuration.CatchAllMode != CatchAllMode.Custom && String.IsNullOrEmpty(webDomain.Configuration.CatchAllEndpoint.Text))
            {
                //++ this will only happen on custom with no catch-all processors registered.
                SetCatchAllProcessorByMode(webDomain.Configuration.CatchAllMode, webDomain.Configuration.CatchAllInitParameter, webDomain.Configuration);
            }
            //+
            return null;
        }

        //- $SelectWebDomain -//
        private void SelectWebDomain(System.Collections.Generic.List<WebDomainData> webDomainDataList)
        {
            WebDomain webDomain = NalariumContext.Current.WebDomain;
            String path = UrlCleaner.CleanWebPathTail(Http.Root);
            Boolean requireSlash = Nalarium.Web.Processing.Configuration.ProcessingSection.GetConfigSection().WebDomain.RequireSlash;
            if (Http.AbsoluteUrl.EndsWith("/"))
            {
                var resultData = webDomainDataList.Where(u => !(String.IsNullOrEmpty(u.Path) || u.Path == "/") && Http.AbsoluteUrl.StartsWith(path + "/" + u.Path + "/", StringComparison.OrdinalIgnoreCase));
                if (resultData.Count() > 0)
                {
                    //++ take one with longest path, if more than one
                    webDomain.Configuration = resultData.OrderByDescending(p => p.Path.Length).First();
                    webDomain.Configuration.FoundWithoutTrailingSlash = false;
                }
            }
            else if (!requireSlash)
            {
                var resultData = webDomainDataList.Where(u => !(String.IsNullOrEmpty(u.Path) || u.Path == "/") && Http.AbsoluteUrl.StartsWith(path + "/" + u.Path, StringComparison.OrdinalIgnoreCase));
                if (resultData.Count() > 0)
                {
                    //++ take one with longest path, if more than one
                    webDomain.Configuration = resultData.OrderByDescending(p => p.Path.Length).First();
                    webDomain.Configuration.FoundWithoutTrailingSlash = true;
                }
            }
        }

        //- $SetCatchAllProcessorByMode -//
        private void SetCatchAllProcessorByMode(CatchAllMode catchAllMode, String catchAllInitParameter, WebDomainData webDomainData)
        {
            WebDomain webDomain = NalariumContext.Current.WebDomain;
            switch (webDomain.Configuration.CatchAllMode)
            {
                case CatchAllMode.PassThrough:
                    webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "{passthrough}");
                    break;
                case CatchAllMode.PassToDefault:
                    if (String.IsNullOrEmpty(webDomainData.DefaultParameter))
                    {
                        throw new InvalidOperationException(Nalarium.Web.Globalization.ResourceAccessor.GetString("WebDomain_DefaultPageCatchAllModeRequiresDefaultPage"));
                    }
                    if (!webDomainData.OverrideProcessorDataList.Any(p => p.ProcessorType.Equals("__$defaultpageoverrideprocessor", StringComparison.InvariantCulture)))
                    {
                        webDomainData.OverrideProcessorDataList.Add(ProcessorData.Create<ProcessorData>("__$defaultpageoverrideprocessor"));
                    }
                    break;
                case CatchAllMode.Forbidden:
                    webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "{httpforbidden}");
                    break;
                case CatchAllMode.Blocked:
                    webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "{blocked}", catchAllInitParameter);
                    break;
                case CatchAllMode.Redirect:
                    if (String.IsNullOrEmpty(catchAllInitParameter))
                    {
                        SetCatchAllProcessorByMode(CatchAllMode.NotFound, String.Empty, webDomainData);
                    }
                    else
                    {
                        webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "redirect", catchAllInitParameter);
                    }
                    break;
                case CatchAllMode.RedirectToRoot:
                    webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "redirect", "/");
                    break;
                case CatchAllMode.Page:
                    if (String.IsNullOrEmpty(catchAllInitParameter))
                    {
                        SetCatchAllProcessorByMode(CatchAllMode.NotFound, String.Empty, webDomainData);
                    }
                    else
                    {
                        webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "page", catchAllInitParameter);
                    }
                    break;
                case CatchAllMode.NotFound:
                    webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "{notfound}");
                    break;
            }
        }
    }
}

//#region Copyright
////+ Nalarium Pro 3.0 - Web Module
////+ Copyright © Jampad Technology, Inc. 2008-2010
//#endregion
//using System;
//using System.Linq;
////+
//using Nalarium.Web.Processing.Data;
////+
//namespace Nalarium.Web.Processing
//{
//    internal class WebDomainInitProcessor : InitProcessor
//    {
//        //- @Setting -//
//        public static class Setting
//        {
//            public const String WebDomain = "WebDomain";
//        }

//        //- @OnPreHttpHandlerExecute -//
//        public override InitProcessor Execute()
//        {
//            if (WebDomain.Current == null)
//            {
//                WebDomainDataList webDomainDataList = WebDomainDataList.AllWebDomainData;
//                if (WebDomainDataList.AllWebDomainData.Count > 0)
//                {
//                    //+ subdomain
//                    var subdomainData = webDomainDataList.Where(u => !String.IsNullOrEmpty(u.Subdomain) && Http.Domain.StartsWith(u.Subdomain, StringComparison.InvariantCultureIgnoreCase)).ToList();
//                    if (subdomainData.Count > 0)
//                    {
//                        SelectWebDomain(subdomainData);
//                        if (WebDomain.Current == null)
//                        {
//                            //++ take one with no path or with "/" as path
//                            WebDomain.Current = subdomainData.Where(p => String.IsNullOrEmpty(p.Path) || p.Path.Equals("/", StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(p => p.Path.Length).FirstOrDefault();
//                            //if (WebDomain.Current == null)
//                            //{
//                            //    //++ take one with longest path, if more than one
//                            //    WebDomain.Current = subdomainData.OrderByDescending(p => p.Path.Length).First();
//                            //}
//                        }
//                    }
//                    if (WebDomain.Current == null)
//                    {
//                        SelectWebDomain(webDomainDataList);
//                    }
//                }
//                if (WebDomain.Current == null)
//                {
//                    WebDomain.Current = webDomainDataList.SingleOrDefault(p => p.Name == "root");
//                    NalariumContext.Current.WebDomain.Configuration.FoundWithoutTrailingSlash = false;
//                }
//            }
//            if (NalariumContext.Current.WebDomain.Configuration.CatchAllMode != CatchAllMode.Custom && String.IsNullOrEmpty(NalariumContext.Current.WebDomain.Configuration.CatchAllEndpoint.Text))
//            {
//                //++ this will only happen on custom with no catch-all processors registered.
//                SetCatchAllProcessorByMode(NalariumContext.Current.WebDomain.Configuration.CatchAllMode, NalariumContext.Current.WebDomain.Configuration.CatchAllInitParameter, WebDomain.Current);
//            }
//            //+
//            return null;
//        }

//        //- $SelectWebDomain -//
//        private void SelectWebDomain(System.Collections.Generic.List<WebDomainData> webDomainDataList)
//        {
//            String path = UrlCleaner.FixWebPathTail(Http.Root);
//            Boolean requireSlash = Nalarium.Web.Processing.Configuration.ProcessingSection.GetConfigSection().WebDomain.RequireSlash;
//            if (Http.AbsoluteUrl.EndsWith("/"))
//            {
//                var resultData = webDomainDataList.Where(u => !(String.IsNullOrEmpty(u.Path) || u.Path == "/") && Http.AbsoluteUrl.StartsWith(path + "/" + u.Path + "/", StringComparison.OrdinalIgnoreCase));
//                if (resultData.Count() > 0)
//                {
//                    //++ take one with longest path, if more than one
//                    WebDomain.Current = resultData.OrderByDescending(p => p.Path.Length).First();
//                    NalariumContext.Current.WebDomain.Configuration.FoundWithoutTrailingSlash = false;
//                }
//            }
//            else if (!requireSlash)
//            {
//                var resultData = webDomainDataList.Where(u => !(String.IsNullOrEmpty(u.Path) || u.Path == "/") && Http.AbsoluteUrl.StartsWith(path + "/" + u.Path, StringComparison.OrdinalIgnoreCase));
//                if (resultData.Count() > 0)
//                {
//                    //++ take one with longest path, if more than one
//                    WebDomain.Current = resultData.OrderByDescending(p => p.Path.Length).First();
//                    NalariumContext.Current.WebDomain.Configuration.FoundWithoutTrailingSlash = true;
//                }
//            }
//        }

//        //- $SetCatchAllProcessorByMode -//
//        private void SetCatchAllProcessorByMode(CatchAllMode catchAllMode, String catchAllInitParameter, WebDomainData webDomainData)
//        {
//            switch (NalariumContext.Current.WebDomain.Configuration.CatchAllMode)
//            {
//                case CatchAllMode.PassThrough:
//                    webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "{passthrough}");
//                    break;
//                case CatchAllMode.DefaultPage:
//                    if (String.IsNullOrEmpty(webDomainData.DefaultPage))
//                    {
//                        throw new InvalidOperationException(Nalarium.Web.Globalization.ResourceAccessor.GetString("WebDomain_DefaultPageCatchAllModeRequiresDefaultPage"));
//                    }
//                    if (!webDomainData.OverrideProcessorDataList.Any(p => p.ProcessorType.Equals("__$defaultpageoverrideprocessor", StringComparison.InvariantCulture)))
//                    {
//                        webDomainData.OverrideProcessorDataList.Add(ProcessorData.Create<ProcessorData>("__$defaultpageoverrideprocessor"));
//                    }
//                    break;
//                case CatchAllMode.Forbidden:
//                    webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "{httpforbidden}");
//                    break;
//                case CatchAllMode.Blocked:
//                    webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "{blocked}", catchAllInitParameter);
//                    break;
//                case CatchAllMode.Redirect:
//                    if (String.IsNullOrEmpty(catchAllInitParameter))
//                    {
//                        SetCatchAllProcessorByMode(CatchAllMode.NotFound, String.Empty, webDomainData);
//                    }
//                    else
//                    {
//                        webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "redirect", catchAllInitParameter);
//                    }
//                    break;
//                case CatchAllMode.RedirectToRoot:
//                    webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "redirect", "/");
//                    break;
//                case CatchAllMode.Page:
//                    if (String.IsNullOrEmpty(catchAllInitParameter))
//                    {
//                        SetCatchAllProcessorByMode(CatchAllMode.NotFound, String.Empty, webDomainData);
//                    }
//                    else
//                    {
//                        webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "page", catchAllInitParameter);
//                    }
//                    break;
//                case CatchAllMode.NotFound:
//                    webDomainData.CatchAllEndpoint = EndpointData.Create(SelectorType.CatchAll, "*", "{notfound}");
//                    break;
//            }
//        }
//    }
//}