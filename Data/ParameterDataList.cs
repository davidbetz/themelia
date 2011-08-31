#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Nalarium.Web.Processing.Data
{
    public class ParameterDataList : List<ParameterData>
    {
        //- @[Indexer] -//
        public ParameterData this[String index]
        {
            get
            {
                ParameterData data = this.FirstOrDefault(p => p.Name == index);
                if (data != null)
                {
                    return data;
                }
                //+
                return null;
            }
        }

        //+
        //- @GetMap -//
        public Map GetMapForCategory(String category)
        {
            var map = new Map();
            foreach (ParameterData data in this.Where(p => p.Category.Equals(category, StringComparison.InvariantCultureIgnoreCase)))
            {
                map.Add(data.Name, data.Value);
            }
            //+
            return map;
        }

        //- @GetMap -//
        public Map GetMap()
        {
            var map = new Map();
            foreach (ParameterData data in this)
            {
                String name = data.Name;
                if (!String.IsNullOrEmpty(data.Category))
                {
                    name = ScopeTranscriber.Construct(data.Category, data.Name);
                }
                map.Add(name, data.Value);
            }
            //+
            return map;
        }

        //- ~Clone -//
        internal ParameterDataList Clone()
        {
            var list = new ParameterDataList();
            foreach (ParameterData data in this)
            {
                list.Add(new ParameterData
                         {
                             Category = data.Category,
                             Name = data.Name,
                             Value = data.Value
                         });
            }
            //+
            return list;
        }
    }
}