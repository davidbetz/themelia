#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using Nalarium.Web.Processing.Data;
//+
namespace Nalarium.Web.Processing
{
    public class NalariumContext
    {
        //- ~Info -//
        internal static class Info
        {
            public const String Scope = "__$Nalarium$Processing$Context";
            //+
            public const String Current = "Current";
        }

        //+
        //- @Current -//
        public static NalariumContext Current
        {
            get
            {
                return HttpData.GetScopedItem<NalariumContext>(Info.Scope, Info.Current);
            }
            internal set
            {
                HttpData.SetScopedItem<NalariumContext>(Info.Scope, Info.Current, value);
            }
        }

        //- @WebDomain -//
        /// <summary>
        /// Contains information about the current web domain.
        /// </summary>
        public WebDomain WebDomain { get; set; }

        //- @Endpoint -//
        /// <summary>
        /// Contains information about the current endpoint.
        /// </summary>
        public Endpoint Endpoint { get; internal set; }

        //+
        //- @WebDomainList -//
        public static WebDomain GetWebDomain(String name)
        {
            return new WebDomain
            {
                Configuration = WebDomainDataList.AllWebDomainData[name]
            };
        }
    }
}