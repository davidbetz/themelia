#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;

namespace Nalarium.Web.Processing.Data
{
    public class EndpointDataList : List<EndpointData>
    {
        //- @Ctor -//
        public EndpointDataList()
        {
        }

        public EndpointDataList(IEnumerable<EndpointData> collection)
            : base(collection)
        {
        }

        //+
        //- @OriginalCount -//
        public Int32 OriginalCount { get; set; }

        //- ~Clone -//
        internal EndpointDataList Clone()
        {
            var list = new EndpointDataList();
            foreach (EndpointData data in this)
            {
                list.Add(data.Clone());
            }
            //+
            return list;
        }
    }
}