#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;

namespace Nalarium.Web.Processing.Data
{
    public class EndpointFactoryDataList : List<FactoryData>
    {
        //- @Ctor -//
        public EndpointFactoryDataList()
        {
        }

        public EndpointFactoryDataList(IEnumerable<FactoryData> collection)
            : base(collection)
        {
        }

        //+
        //- @OriginalCount -//
        public Int32 OriginalCount { get; set; }

        //- ~Clone -//
        internal EndpointFactoryDataList Clone()
        {
            var list = new EndpointFactoryDataList();
            foreach (FactoryData data in this)
            {
                list.Add(new FactoryData
                         {
                             Priority = data.Priority,
                             FactoryType = data.FactoryType,
                             ParameterArray = data.ParameterArray,
                             Source = data.Source
                         });
            }
            //+
            return list;
        }
    }
}