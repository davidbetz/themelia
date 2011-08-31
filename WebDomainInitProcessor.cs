#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Nalarium.Web.Globalization;
using Nalarium.Web.Processing.Configuration;
using Nalarium.Web.Processing.Data;
//+

namespace Nalarium.Web.Processing
{
    internal class WebDomainInitProcessor : ContextInitProcessor
    {
        //- @Setting -//

        //- @OnPreHttpHandlerExecute -//
        public override InitProcessor Execute()
        {
            var webDomain = new WebDomain();
            NalariumContext.Current.WebDomain = webDomain;
            WebDomainDataList webDomainDataList = WebDomainDataList.AllWebDomainData;
            if (WebDomainDataList.AllWebDomainData.Count > 0)
            {
                //+ subdomain
                List<WebDomainData> subdomainData = webDomainDataList.Where(u => !String.IsNullOrEmpty(u.Subdomain) && Http.Domain.StartsWith(u.Subdomain, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (subdomainData.Count > 0)
                {
                    SelectWebDomain(subdomainData);
                    //++ take one with no path or with "/" as path
                    webDomain.Configuration = subdomainData.Where(p => String.IsNullOrEmpty(p.Path) || p.Path.Equals("/", StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(p => p.Path.Length).FirstOrDefault();
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
            if (webDomain.Configuration.CatchAllMode != CatchAllMode.Custom && String.IsNullOrEmpty(webDomain.Configuration.CatchAllEndpoint.Text))
            {
                //++ this will only happen on custom with no catch-all processors registered.
                SetCatchAllProcessorByMode(webDomain.Configuration.CatchAllMode, webDomain.Configuration.CatchAllInitParameter, webDomain.Configuration);
            }
            //+
            return null;
        }

        //- $SelectWebDomain -//
        private void SelectWebDomain(List<WebDomainData> webDomainDataList)
        {
            WebDomain webDomain = NalariumContext.Current.WebDomain;
            String path = UrlCleaner.CleanWebPathTail(Http.Root);
            Boolean requireSlash = ProcessingSection.GetConfigSection().WebDomain.RequireSlash;
            if (Http.AbsoluteUrl.EndsWith("/"))
            {
                IEnumerable<WebDomainData> resultData = webDomainDataList.Where(u => !(String.IsNullOrEmpty(u.Path) || u.Path == "/") && Http.AbsoluteUrl.StartsWith(path + "/" + u.Path + "/", StringComparison.OrdinalIgnoreCase));
                if (resultData.Count() > 0)
                {
                    //++ take one with longest path, if more than one
                    webDomain.Configuration = resultData.OrderByDescending(p => p.Path.Length).First();
                    webDomain.Configuration.FoundWithoutTrailingSlash = false;
                }
            }
            else if (!requireSlash)
            {
                IEnumerable<WebDomainData> resultData = webDomainDataList.Where(u => !(String.IsNullOrEmpty(u.Path) || u.Path == "/") && Http.AbsoluteUrl.StartsWith(path + "/" + u.Path, StringComparison.OrdinalIgnoreCase));
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
                        throw new InvalidOperationException(ResourceAccessor.GetString("WebDomain_DefaultPageCatchAllModeRequiresDefaultPage"));
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

        #region Nested type: Setting

        public static class Setting
        {
            public const String WebDomain = "WebDomain";
        }

        #endregion
    }
}