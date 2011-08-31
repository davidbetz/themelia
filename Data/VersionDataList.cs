#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;

namespace Nalarium.Web.Processing.Data
{
    public class VersionDataList : List<VersionData>
    {
        //- @Ctor -//
        public VersionDataList()
        {
        }

        public VersionDataList(IEnumerable<VersionData> collection)
            : base(collection)
        {
        }

        //+
        //- @ExplicitVersion -//
        public String ExplicitVersion { get; set; }

        //- ~Clone -//
        public VersionDataList Clone()
        {
            var list = new VersionDataList
                       {
                           ExplicitVersion = ExplicitVersion
                       };
            foreach (VersionData data in this)
            {
                list.Add(new VersionData
                         {
                             Name = data.Name,
                             Weight = data.Weight
                         });
            }
            //+
            return list;
        }
    }
}