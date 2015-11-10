#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;

namespace Nalarium.Web.Processing.Data
{
    public class ErrorProcessorDataList : List<ErrorProcessorData>
    {
        //- @Ctor -//
        public ErrorProcessorDataList()
        {
        }

        public ErrorProcessorDataList(IEnumerable<ErrorProcessorData> collection)
            : base(collection)
        {
        }

        //+
        //- @OriginalCount -//
        public Int32 OriginalCount { get; set; }

        //- ~Clone -//
        internal ErrorProcessorDataList Clone()
        {
            var list = new ErrorProcessorDataList();
            foreach (ErrorProcessorData data in this)
            {
                list.Add(new ErrorProcessorData
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