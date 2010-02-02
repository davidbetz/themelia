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
    internal class FileAliasInitProcessor : InitProcessor
    {
        //- @OnInitProcessorExecute -//
        public override InitProcessor Execute()
        {
            String target = String.Empty;
            String contentType = String.Empty;
            WebDomainData activeData = NalariumContext.Current.WebDomain.Configuration;
            SelectorType matchType = 0;
            List<EndpointData> handlerList = activeData.EndpointDataList.Where(p => p.Type.Equals("file", StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (EndpointData handler in handlerList)
            {
                if (PathMatcher.Match(handler.Selector, handler.Text))
                {
                    if (!String.IsNullOrEmpty(handler.ParameterValue))
                    {
                        Int32 commaIndex = handler.ParameterValue.IndexOf(":");
                        if (commaIndex > -1)
                        {
                            target = handler.ParameterValue.Substring(0, commaIndex).Trim();
                            contentType = handler.ParameterValue.Substring(commaIndex + 1, handler.ParameterValue.Length - commaIndex - 1).Trim();
                        }
                        else
                        {
                            target = handler.ParameterValue;
                        }
                    }
                    if (handler.ParameterMap != null && handler.ParameterMap.Count > 0)
                    {
                        target = handler.ParameterMap.Get("target", StringComparison.OrdinalIgnoreCase);
                        contentType = handler.ParameterMap.Get("contentType", StringComparison.OrdinalIgnoreCase);
                    }
                }
                else if (handler.Text.EndsWith("/", StringComparison.OrdinalIgnoreCase) && activeData.AcceptMissingTrailingSlash)
                {
                    String matchTextWithoutTrailingSlash = handler.Text.Substring(0, handler.Text.Length - 1);
                    if (PathMatcher.Match(handler.Selector, matchTextWithoutTrailingSlash))
                    {
                        if (handler.ParameterMap != null && handler.ParameterMap.Count > 0)
                        {
                            target = handler.ParameterMap.Get("target", StringComparison.OrdinalIgnoreCase);
                            contentType = handler.ParameterMap.Get("contentType", StringComparison.OrdinalIgnoreCase);
                        }
                    }
                }
                if (!String.IsNullOrEmpty(target))
                {
                    matchType = handler.Selector;
                }
            }
            if (!String.IsNullOrEmpty(target))
            {
                if (target.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                {
                    target = target.Substring(2, target.Length - 2);
                }
                //+ file
                if (matchType == SelectorType.WebDomainPathEquals ||
                    matchType == SelectorType.EndsWith ||
                    matchType == SelectorType.PathEquals ||
                    System.IO.File.Exists(target))
                {
                    if (!String.IsNullOrEmpty(contentType))
                    {
                        Context.Response.ContentType = contentType;
                    }
                    Context.Response.WriteFile(Context.Server.MapPath(target));
                    Context.Response.End();
                }
                //+
                FlowControl.StopAfterInitProcessing();
            }
            //+
            return null;
        }
    }
}