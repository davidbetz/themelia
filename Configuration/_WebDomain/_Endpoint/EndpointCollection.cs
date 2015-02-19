#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;
using Nalarium.Configuration.AppConfig;


namespace Nalarium.Web.Processing.Configuration
{
    public class EndpointCollection : CommentableCollection<EndpointElement>
    {
        //- #GetElementKey -//
        protected override Object GetElementKey(ConfigurationElement element)
        {
            var endpointElement = (EndpointElement)element;
            //+
            return endpointElement.Selector + ":" + endpointElement.Type + ":" + endpointElement.Text;
        }
    }
}