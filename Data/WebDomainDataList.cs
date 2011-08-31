#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Nalarium.Web.Processing.Data
{
    public class WebDomainDataList : List<WebDomainData>
    {
        //- @[Indexer] -//
        public WebDomainData this[String index]
        {
            get
            {
                WebDomainData data = this.FirstOrDefault(p => p.Name == index);
                if (data != null)
                {
                    return data;
                }
                //+
                return null;
            }
        }

        //- @AllWebDomainData -//
        public static WebDomainDataList AllWebDomainData { get; set; }
    }
}