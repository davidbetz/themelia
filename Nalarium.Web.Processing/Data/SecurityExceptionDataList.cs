#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Nalarium.Web.Processing.Data
{
    public class SecurityExceptionDataList : List<SecurityExceptionData>
    {
        //- @[Indexer] -//
        public SecurityExceptionData this[String index]
        {
            get
            {
                SecurityExceptionData data = this.FirstOrDefault(p => p.Key == index);
                if (data != null)
                {
                    return data;
                }
                //+
                return null;
            }
        }

        //- ~Clone -//
        internal SecurityExceptionDataList Clone()
        {
            var list = new SecurityExceptionDataList();
            foreach (SecurityExceptionData data in this)
            {
                list.Add(new SecurityExceptionData
                         {
                             Key = data.Key
                         });
            }
            //+
            return list;
        }
    }
}