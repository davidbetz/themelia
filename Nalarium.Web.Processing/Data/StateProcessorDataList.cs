#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;

namespace Nalarium.Web.Processing.Data
{
    public class StateProcessorDataList : List<ProcessorData>
    {
        //- @Ctor -//
        public StateProcessorDataList()
        {
        }

        public StateProcessorDataList(IEnumerable<ProcessorData> collection)
            : base(collection)
        {
        }

        //+
        //- @OriginalCount -//
        public Int32 OriginalCount { get; set; }

        //- ~Clone -//
        internal StateProcessorDataList Clone()
        {
            var list = new StateProcessorDataList();
            foreach (ProcessorData data in this)
            {
                list.Add(new ProcessorData
                         {
                             Priority = data.Priority,
                             ParameterArray = data.ParameterArray,
                             ProcessorType = data.ProcessorType,
                             Source = data.Source
                         });
            }
            //+
            return list;
        }
    }
}