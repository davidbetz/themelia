#region Copyright
//+ Themelia Pro 2.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
#endregion
using System;
using System.Collections.Generic;
//+
namespace Themelia.Web.Processing.Data
{
    public class AliasDataList : List<AliasData>
    {
        //- @Ctor -//
        public AliasDataList()
        {
        }
        public AliasDataList(IEnumerable<AliasData> collection)
            : base(collection)
        {
        }

        //+
        //- @OriginalCount -//
        public Int32 OriginalCount { get; set; }

        //- ~Clone -//
        internal AliasDataList Clone()
        {
            AliasDataList list = new AliasDataList();
            foreach (AliasData data in this)
            {
                list.Add(new AliasData
                {
                    Key = data.Key,
                    Extra = data.Extra,
                    Target = data.Target,
                    Source = data.Source
                });
            }
            //+
            return list;
        }
    }
}