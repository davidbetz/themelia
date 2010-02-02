#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Collections.Generic;
//+
namespace Nalarium.Web.Processing.Data
{
    public class AccessRuleDataList : List<AccessRuleData>
    {
        //- @DefaultAccessMode -//
        public Nalarium.Web.Security.DefaultAccessMode DefaultAccessMode { get; internal set; }

        //- @Parameter -//
        public String Parameter { get; set; }

        //- @ParameterType -//
        public ActionType ParameterType { get; set; }

        //- ~Clone -//
        internal AccessRuleDataList Clone()
        {
            AccessRuleDataList list = new AccessRuleDataList();
            foreach (AccessRuleData data in this)
            {
                list.Add(new AccessRuleData
                {
                    AccessType = data.AccessType,
                    Parameter = data.Parameter,
                    Name = data.Name,
                    ParameterType = data.ParameterType,
                    Text = data.Text,
                    Disabled = data.Disabled
                });
            }
            //+
            return list;
        }
    }
}