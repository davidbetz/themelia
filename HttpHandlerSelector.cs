#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Collections.Generic;
using System.Web;
//+
using Nalarium.Activation;
using Nalarium.Web.Processing.Data;
//+
namespace Nalarium.Web.Processing
{
    internal class HttpHandlerSelector
    {
        //- ~MatchHttpHandler -//
        internal EndpointData MatchHttpHandler(EndpointData endpoint, Boolean withoutSlash, out IHttpHandler handler)
        {
            String text = endpoint.Text;
            if (withoutSlash)
            {
                text = endpoint.TextWithoutSlash;
            }
            if (PathMatcher.Match(endpoint.Selector, text))
            {
                if (endpoint.SubEndpointDataList != null && endpoint.SubEndpointDataList.Count > 0)
                {
                    EndpointData newEndpoint = ProcessPotentialSubEndpoint(endpoint);
                    if (newEndpoint != null)
                    {
                        endpoint = newEndpoint;
                    }
                }
                handler = AttemptHttpHandlerCreate(endpoint);
            }
            else
            {
                handler = new DummyHttpHandler();
            }
            //+
            return endpoint;
        }

        //- $ProcessPotentialSubEndpoint -//
        private EndpointData ProcessPotentialSubEndpoint(EndpointData endpoint)
        {
            EndpointDataList list = endpoint.SubEndpointDataList;
            foreach (EndpointData data in list)
            {
                switch (data.Selector)
                {
                    case SelectorType.UserAgent:
                        if (Http.UserAgent.Contains(data.Text) || (data.Text.Equals("{blank}") && Http.UserAgent.Length == 0))
                        {
                            return data;
                        }
                        break;
                    case SelectorType.Referrer:
                        if (Http.Referrer.Contains(data.Text) || (data.Text.Equals("{blank}") && Http.Referrer.Length == 0))
                        {
                            return data;
                        }
                        break;
                    case SelectorType.IPAddress:
                        if (IPAddressMatcher.Match(data.Text) || (data.Text.Equals("{blank}") && Http.IPAddress.Length == 0))
                        {
                            return data;
                        }
                        break;
                }
            }
            //+
            return null;
        }

        //- ~AttemptHttpHandlerCreate-//
        internal IHttpHandler AttemptHttpHandlerCreate(EndpointData endpoint)
        {
            IHttpHandler hh = CreateHttpHandler(endpoint.Type);
            if (hh == null)
            {
                if (WebProcessingReportController.Reporter.Initialized)
                {
                    Map map = new Map();
                    map.Add("Section", "Handler Creation");
                    map.Add("Message", "Could not create handler.  Either the type name is incorrect or the required handler factory was not installed correctly.");
                    map.Add("Handler Name", endpoint.Type);
                    map.Add("Handler Match Type", endpoint.Selector.ToString());
                    map.Add("Handler Match Text", endpoint.Text);
                    //+
                    WebProcessingReportController.Reporter.AddMap(map);
                }
            }
            else
            {
                //+ initialize
                IHasParameterMap iHasParameterMap;
                if ((iHasParameterMap = hh as IHasParameterMap) != null)
                {
                    if ((endpoint.ParameterMap == null || endpoint.ParameterMap.Count == 0) && !String.IsNullOrEmpty(iHasParameterMap.DefaultParameter) && !String.IsNullOrEmpty(endpoint.ParameterValue))
                    {
                        if (iHasParameterMap.ParameterMap == null)
                        {
                            iHasParameterMap.ParameterMap = new Map();
                        }
                        iHasParameterMap.ParameterMap[iHasParameterMap.DefaultParameter] = endpoint.ParameterValue;
                    }
                    else
                    {
                        iHasParameterMap.ParameterMap = endpoint.ParameterMap;
                        if (iHasParameterMap.ParameterMap == null)
                        {
                            iHasParameterMap.ParameterMap = new Map();
                        }
                    }
                }
            }
            //+
            return hh;
        }

        //- ~CreateHttpHandler -//
        internal static IHttpHandler CreateHttpHandler(String text)
        {
            IHttpHandler handler = null;
            if (text.Contains(","))
            {
                return ObjectCreator.CreateAs<IHttpHandler>(text);
            }
            //+
            if (RouteCache.HandlerFactoryCache != null)
            {
                String lowerCaseText = text.ToLower(System.Globalization.CultureInfo.CurrentCulture);
                List<IFactory> handlerFactoryList = RouteCache.HandlerFactoryCache.GetValueList();
                foreach (IFactory factory in handlerFactoryList)
                {
                    handler = ((HandlerFactory)factory).CreateHttpHandler(lowerCaseText);
                    if (handler != null)
                    {
                        break;
                    }
                }
            }
            //+
            return handler;
        }
    }
}