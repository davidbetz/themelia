#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;

namespace Nalarium.Web.Processing.Data
{
    public class ViewDataList : List<ViewData>
    {
        //- @Ctor -//
        public ViewDataList()
        {
        }

        public ViewDataList(IEnumerable<ViewData> collection)
            : base(collection)
        {
        }

        //+
        //- @OriginalCount -//
        public Int32 OriginalCount { get; set; }

        //- ~Clone -//
        internal ViewDataList Clone()
        {
            var list = new ViewDataList();
            foreach (ViewData data in this)
            {
                list.Add(new ViewData
                         {
                             Name = data.Name
                         });
            }
            //+
            return list;
        }
    }
}