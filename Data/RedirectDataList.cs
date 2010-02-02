#region Copyright
//+ Themelia Pro 2.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
#endregion
using System;
using System.Collections.Generic;
//+
namespace Themelia.Web.Processing.Data
{
    public class RedirectDataList : List<RedirectData>
    {
        //- @Ctor -//
        public RedirectDataList()
        {
        }
        public RedirectDataList(IEnumerable<RedirectData> collection)
            : base(collection)
        {
        }

        //+
        //- @OriginalCount -//
        public Int32 OriginalCount { get; set; }

        //- ~Clone -//
        internal RedirectDataList Clone()
        {
            RedirectDataList list = new RedirectDataList();
            foreach (RedirectData data in this)
            {
                list.Add(new RedirectData
                {
                    Key = data.Key,
                    Destination = data.Destination,
                    Source = data.Source
                });
            }
            //+
            return list;
        }
    }
}