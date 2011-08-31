#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Reflection;
using System.Web;
using Nalarium.Activation;
using Nalarium.Web.Processing.Data;
//+

namespace Nalarium.Web.Processing
{
    public class DefaultPageSelectionProcessor : SelectionProcessor
    {
        private static Type _aspNetMvcHttpHandlerType;

        //+
        //- @OnSelectionProcessorExecute -//
        public override IHttpHandler Execute(params Object[] parameterArray)
        {
            WebDomainData data = NalariumContext.Current.WebDomain.Configuration;
            DefaultType type = data.DefaultType;
            switch (type)
            {
                case DefaultType.Page:
                case DefaultType.Sequence:
                    return new PageEndpointHttpHandler(type == DefaultType.Sequence);
                    //+
                case DefaultType.Handler:
                    var parameter = HttpData.GetScopedItem<String>(DefaultPageInitProcessor.Info.Scope, DefaultPageInitProcessor.Info.DefaultParameter);
                    EndpointData endpointData;
                    Map parameterMap = data.ParameterDataList.GetMapForCategory(String.Empty);
                    if (!String.IsNullOrEmpty(data.CustomParameter))
                    {
                        parameterMap.Add(String.Empty, parameter);
                        endpointData = new EndpointData
                                       {
                                           Type = data.CustomParameter,
                                           ParameterMap = parameterMap
                                       };
                    }
                    else
                    {
                        endpointData = new EndpointData
                                       {
                                           Type = parameter,
                                           ParameterMap = data.ParameterDataList.GetMapForCategory(String.Empty)
                                       };
                    }
                    String typeName = endpointData.Type;
                    if (!typeName.Contains(","))
                    {
                        typeName = "{" + typeName + "}";
                    }
                    return new HttpHandlerSelector().AttemptHttpHandlerCreate(endpointData, typeName);
                    //+
                case DefaultType.Mvc:
                    return CreateMvcHandler();
            }
            //+
            return null;
        }

        //- $CreateMvcHandler -//
        private IHttpHandler CreateMvcHandler()
        {
            if (_aspNetMvcHttpHandlerType == null)
            {
                try
                {
                    Assembly assembly = AssemblyLoader.Load("Nalarium.Web.Mvc");
                    _aspNetMvcHttpHandlerType = assembly.GetType("Nalarium.Web.Mvc.AspNetMvcHttpHandler");
                }
                catch (Exception ex)
                {
                    if (WebProcessingReportController.Reporter.Initialized)
                    {
                        var map = new Map();
                        map.Add("Section", "Mvc");
                        map.Add("Message", ex.Message);
                        map.Add("Exception Type", ex.GetType().FullName);
                        //+
                        WebProcessingReportController.Reporter.AddMap(map);
                    }
                }
            }
            //+
            return ObjectCreator.CreateAs<IHttpHandler>(_aspNetMvcHttpHandlerType);
        }
    }
}