#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
//+
namespace Nalarium.Web.Processing.Data
{
    public class ComponentDataList : List<ComponentData>
    {
        //- @[Indexer] -//
        public ComponentData this[String index]
        {
            get
            {
                ComponentData data = this.FirstOrDefault(p => p.Key == index);
                if (data != null)
                {
                    return data;
                }
                //+
                return null;
            }
        }

        //+
        //- ~Clone -//
        internal ComponentDataList Clone()
        {
            ComponentDataList list = new ComponentDataList();
            foreach (ComponentData data in this)
            {
                list.Add(new ComponentData
                {
                    ComponentType = data.ComponentType,
                    Key = data.Key,
                    ParameterDataList = data.ParameterDataList.Clone()
                });
            }
            //+
            return list;
        }
    }
}