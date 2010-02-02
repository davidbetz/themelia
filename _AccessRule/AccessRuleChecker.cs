#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//+
using Nalarium.Web.Processing.Data;
//+
namespace Nalarium.Web.Processing
{
    internal static class AccessRuleChecker
    {
        //- $OnBeginRequest -//
        internal static Boolean Check(HttpApplication httpApplication)
        {
            HttpContext context = httpApplication.Context;
            HttpRequest request = httpApplication.Request;
            String ipAddress = Http.IPAddress;
            AccessRuleDataList accessDenyList = Nalarium.Web.NalariumContext.Current.WebDomain.Configuration.AccessRuleDataList;
            List<AccessRuleData> enabledList = accessDenyList.Where(p => p.Disabled == false && !String.IsNullOrEmpty(p.Text)).ToList();
            List<AccessRuleData> subset;
            if (accessDenyList.DefaultAccessMode == Nalarium.Web.Security.DefaultAccessMode.Allow)
            {
                //+ ip address
                subset = enabledList.Where(p => p.AccessType == AccessType.IPAddress).ToList();
                foreach (AccessRuleData access in subset)
                {
                    if (ipAddress == access.Text || (String.IsNullOrEmpty(ipAddress) && access.Text.Equals("{blank}", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        BlockAccess(access.Parameter, access.ParameterType);
                        return true;
                    }
                }
                //+ user agent
                subset = enabledList.Where(p => p.AccessType == AccessType.UserAgent).ToList();
                foreach (AccessRuleData access in subset)
                {
                    if (request.UserAgent == null)
                    {
                        if (access.Text.Equals("{blank}", StringComparison.InvariantCultureIgnoreCase))
                        {
                            BlockAccess(access.Parameter, access.ParameterType);
                            return true;
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(request.UserAgent) && access.Text.Equals("{blank}", StringComparison.InvariantCultureIgnoreCase))
                        {
                            BlockAccess(access.Parameter, access.ParameterType);
                            return true;
                        }
                        if (request.UserAgent.ToLower(System.Globalization.CultureInfo.InvariantCulture).Contains(access.Text.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                        {
                            BlockAccess(access.Parameter, access.ParameterType);
                            return true;
                        }
                    }
                }
                //+ url
                subset = enabledList.Where(p => p.AccessType == AccessType.Url).ToList();
                foreach (AccessRuleData access in subset)
                {
                    if (request.Url != null && request.Url.AbsoluteUri != null)
                    {
                        if (request.Url.AbsoluteUri.ToLower(System.Globalization.CultureInfo.InvariantCulture).Contains(access.Text.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                        {
                            BlockAccess(access.Parameter, access.ParameterType);
                            return true;
                        }
                    }
                }
                //+ http referrer
                subset = enabledList.Where(p => p.AccessType == AccessType.HttpReferrer).ToList();
                foreach (AccessRuleData access in subset)
                {
                    Uri urlReferrer = request.UrlReferrer;
                    if (urlReferrer == null)
                    {
                        if (access.Text.Equals("{blank}", StringComparison.InvariantCultureIgnoreCase))
                        {
                            BlockAccess(access.Parameter, access.ParameterType);
                            return true;
                        }
                    }
                    else if (urlReferrer.AbsoluteUri == access.Text)
                    {
                        BlockAccess(access.Parameter, access.ParameterType);
                        return true;
                    }
                }
            }
            else
            {
                //+ ip address
                subset = enabledList.Where(p => p.AccessType == AccessType.IPAddress).ToList();
                foreach (AccessRuleData access in subset)
                {
                    if (ipAddress == access.Text || (String.IsNullOrEmpty(ipAddress) && access.Text.Equals("{blank}", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        return false;
                    }
                }
                //+ user agent
                subset = enabledList.Where(p => p.AccessType == AccessType.UserAgent).ToList();
                foreach (AccessRuleData access in subset)
                {
                    if (request.UserAgent == null)
                    {
                        if (access.Text.Equals("{blank}", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(request.UserAgent) && access.Text.Equals("{blank}", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return false;
                        }
                        if (request.UserAgent.ToLower(System.Globalization.CultureInfo.InvariantCulture).Contains(access.Text.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                        {
                            return false;
                        }
                    }
                }
                //+ url
                subset = enabledList.Where(p => p.AccessType == AccessType.Url).ToList();
                foreach (AccessRuleData access in subset)
                {
                    if (request.Url != null && request.Url.AbsoluteUri != null)
                    {
                        if (request.Url.AbsoluteUri.ToLower(System.Globalization.CultureInfo.InvariantCulture).Contains(access.Text.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                        {
                            return false;
                        }
                    }
                }
                //+ http referrer
                subset = enabledList.Where(p => p.AccessType == AccessType.HttpReferrer).ToList();
                foreach (AccessRuleData access in subset)
                {
                    Uri urlReferrer = request.UrlReferrer;
                    if (urlReferrer == null)
                    {
                        if (access.Text.Equals("{blank}", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return false;
                        }
                    }
                    else if (urlReferrer.AbsoluteUri == access.Text)
                    {
                        return false;
                    }
                }
                BlockAccess(accessDenyList.Parameter, accessDenyList.ParameterType);
                return true;
            }
            //+
            return false;
        }

        //- $BlockAccess -//
        private static void BlockAccess(String parameter, ActionType parameterType)
        {
            HttpResponse response = Http.Response;
            switch (parameterType)
            {
                case ActionType.Write:
                    if (String.IsNullOrEmpty(parameter))
                    {
                        parameter = " ";
                    }
                    SurpressContent(response, parameter);
                    break;
                case ActionType.Redirect:
                    if (!String.IsNullOrEmpty(parameter))
                    {
                        Http.Redirect(parameter);
                        return;
                    }
                    break;
            }
        }

        //- $SurpressContent -//
        private static void SurpressContent(HttpResponse response, String message)
        {
            response.StatusCode = 200;
            response.Write(message);
            response.SuppressContent = false;
            response.End();
        }
    }
}