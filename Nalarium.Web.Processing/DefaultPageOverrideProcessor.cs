#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web;
//+

namespace Nalarium.Web.Processing
{
    internal class DefaultPageOverrideProcessor : OverrideProcessor
    {
        //- @Execute -//
        public override IHttpHandler Execute(IHttpHandler activeHttpHandler)
        {
            if (activeHttpHandler != null)
            {
                return activeHttpHandler;
            }
            if (String.IsNullOrEmpty(HttpData.GetScopedItem<String>(DefaultPageInitProcessor.Info.Scope, DefaultPageInitProcessor.Info.DefaultParameter)))
            {
                String defaultParameter = NalariumContext.Current.WebDomain.Configuration.DefaultParameter;
                if (!String.IsNullOrEmpty(defaultParameter))
                {
                    HttpData.SetScopedItem(DefaultPageInitProcessor.Info.Scope, DefaultPageInitProcessor.Info.DefaultParameter, defaultParameter);
                }
            }
            if (!String.IsNullOrEmpty(HttpData.GetScopedItem<String>(DefaultPageInitProcessor.Info.Scope, DefaultPageInitProcessor.Info.DefaultParameter)))
            {
                return new PageEndpointHttpHandler();
            }
            //+
            return null;
        }
    }
}