#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
//+
using Nalarium.Web.Processing.Data;
//+
namespace Nalarium.Web.Processing
{
    internal class ServiceEndpointInitProcessor : InitProcessor
    {
        //- @OnInitProcessorExecute -//
        public override InitProcessor Execute()
        {
            String target = String.Empty;
            SelectorType matchType;
            String matchText = String.Empty;
            WebDomainData activeData = NalariumContext.Current.WebDomain.Configuration;
            List<EndpointData> handlerList = activeData.EndpointDataList.Where(p => p.Type.Equals("service", StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (EndpointData handler in handlerList)
            {
                if (PathMatcher.Match(handler.Selector, handler.Text))
                {
                    if (!String.IsNullOrEmpty(handler.ParameterValue))
                    {
                        target = handler.ParameterValue;
                        matchText = handler.Text;
                        matchType = handler.Selector;
                        //+ substitution
                        if (matchText.Contains("(") && target.Contains("$"))
                        {
                            target = PathMatcher.Substitute(matchType, UrlCleaner.CleanWebPathHead(matchText), target);
                        }
                        break;
                    }
                }
            }
            //+
            if (!String.IsNullOrEmpty(target))
            {
                if (target.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                {
                    target = target.Substring(2, target.Length - 2);
                }
                //+
                target = UrlCleaner.CleanWebPathHead(target);
                String extraData = String.Empty;
                Int32 serviceIndex = target.IndexOf("?", StringComparison.OrdinalIgnoreCase);
                if (serviceIndex > -1)
                {
                    extraData = target.Substring(serviceIndex + 1, target.Length - serviceIndex - 1);
                    target = target.Substring(0, serviceIndex + 1);
                }
                //+
                if (target.Contains("://") || target.Contains(Http.Root))
                {
                    throw new InvalidOperationException(Resource.Path_PrefixForbidden);
                }
                HttpData.SetScopedItem<Boolean>(Http.Info.Alias, Http.Info.IsAliased, true);
                Context.RewritePath("~/" + target, extraData, Http.QueryString.ToString(), false);
                //+
                FlowControl.StopAfterInitProcessing();
            }
            //+
            return null;
        }
    }
}