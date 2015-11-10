#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

//+
//+
using System;
using System.Linq;
using Nalarium.Activation;
using Nalarium.Web.Globalization;
using Nalarium.Web.Processing.Data;
using Nalarium.Web.Security;
using RouteActivatorInfo = Nalarium.Web.Processing.RouteActivator.Info;

namespace Nalarium.Web.Processing
{
    internal class SecurityStateProcessor : StateProcessor
    {
        private static readonly Object _lock = new Object();

        //+
        //- ~Execute -//
        public override StateProcessor Execute()
        {
            WebDomainData data = NalariumContext.Current.WebDomain.Configuration;
            if (data.SecurityData.Disabled)
            {
                return null;
            }
            lock (_lock)
            {
                if (SecurityData.SecurityValidator == null)
                {
                    SecurityData.SecurityValidator = ObjectCreator.CreateAs<ISecurityValidator>(data.SecurityData.ValidatorType);
                    if (SecurityData.SecurityValidator == null)
                    {
                        throw new ArgumentException(ResourceAccessor.GetString("Security_InvalidValidator"));
                    }
                }
            }
            String target = String.Empty;
            if (PathMatcher.Match(SelectorType.EndsWith, data.SecurityData.LogoutText) || (!NalariumContext.Current.WebDomain.Configuration.AcceptMissingTrailingSlash && PathMatcher.Match(SelectorType.EndsWith, data.SecurityData.LogoutText + "/")))
            {
                lock (_lock)
                {
                    if (SecurityData.SecurityValidator != null)
                    {
                        SecurityData.SecurityValidator.Logout();
                        String newPath = "/" + String.Join("/", ArrayModifier.Strip<String>(Http.UrlPartArray));
                        if (!HasAccess(newPath, data))
                        {
                            target = UrlCleaner.CleanWebPathTail(newPath) + "/" + data.SecurityData.LoginText;
                            if (!NalariumContext.Current.WebDomain.Configuration.AcceptMissingTrailingSlash)
                            {
                                target += "/";
                            }
                            //+
                            Http.Redirect(target);
                        }
                        else
                        {
                            Http.Redirect(newPath);
                        }
                    }
                }
            }
            if (PathMatcher.Match(SelectorType.EndsWith, data.SecurityData.LoginText) || (!NalariumContext.Current.WebDomain.Configuration.AcceptMissingTrailingSlash && PathMatcher.Match(SelectorType.EndsWith, data.SecurityData.LoginText + "/")))
            {
                return null;
            }
            //+
            var key = HttpData.GetScopedItem<String>(RouteActivatorInfo.Scope, RouteActivatorInfo.Text);
            if (String.IsNullOrEmpty(key))
            {
                key = "/";
            }
            target = UrlCleaner.CleanWebPathTail(Http.AbsolutePath) + "/" + data.SecurityData.LoginText;
            if (!NalariumContext.Current.WebDomain.Configuration.AcceptMissingTrailingSlash)
            {
                target += "/";
            }
            //+ validator
            lock (_lock)
            {
                if (SecurityData.SecurityValidator != null)
                {
                    if (!HasAccess(key, data))
                    {
                        Http.Redirect(target);
                    }
                }
            }
            //+
            return null;
        }

        //- $HasAccess -//
        private bool HasAccess(String key, WebDomainData data)
        {
            if (data.SecurityData.DefaultAccessMode == DefaultAccessMode.Allow)
            {
                if (data.SecurityData.SecurityExceptionDataList.Any(p => key.Equals(p.Key, StringComparison.InvariantCultureIgnoreCase)))
                {
                    if (!SecurityData.SecurityValidator.IsValid())
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (!data.SecurityData.SecurityExceptionDataList.Any(p => key.Equals(p.Key, StringComparison.InvariantCultureIgnoreCase)))
                {
                    if (!SecurityData.SecurityValidator.IsValid())
                    {
                        return false;
                    }
                }
            }
            //+
            return true;
        }
    }
}