#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
using Info = Nalarium.Web.Processing.DefaultPageInitProcessor.Info;
//+
namespace Nalarium.Web.Processing
{
    internal class DefaultPageOverrideProcessor : OverrideProcessor
    {
        //- @Execute -//
        public override System.Web.IHttpHandler Execute(System.Web.IHttpHandler activeHttpHandler)
        {
            if (activeHttpHandler != null)
            {
                return activeHttpHandler;
            }
            if (String.IsNullOrEmpty(HttpData.GetScopedItem<String>(Info.Scope, Info.DefaultParameter)))
            {
                String defaultParameter = NalariumContext.Current.WebDomain.Configuration.DefaultParameter;
                if (!String.IsNullOrEmpty(defaultParameter))
                {
                    HttpData.SetScopedItem<String>(Info.Scope, Info.DefaultParameter, defaultParameter);
                }
            }
            if (!String.IsNullOrEmpty(HttpData.GetScopedItem<String>(Info.Scope, Info.DefaultParameter)))
            {
                return new PageEndpointHttpHandler();
            }
            //+
            return null;
        }
    }
}