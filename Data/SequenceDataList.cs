#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Nalarium.Web.Processing.Data
{
    public class SequenceDataList : List<SequenceData>
    {
        //- @[Indexer] -//
        public SequenceData this[String index]
        {
            get
            {
                SequenceData data = this.FirstOrDefault(p => p.Name == index);
                if (data != null)
                {
                    return data;
                }
                //+
                return null;
            }
        }

        //- @AllSequenceData -//
        public static SequenceDataList AllSequenceData { get; set; }
    }
}