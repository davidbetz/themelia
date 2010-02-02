#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing.Data
{
    [System.Diagnostics.DebuggerDisplay("{Name}, {AccessType}, {Text}, ParameterType: {ParameterType}, {Disabled}")]
    public class AccessRuleData
    {
        //- @Name -//
        public String Name { get; set; }

        //- @AccessType -//
        public AccessType AccessType { get; set; }

        //- @Text -//
        public String Text { get; set; }

        //- @Parameter -//
        public String Parameter { get; set; }

        //- @ParameterType -//
        public ActionType ParameterType { get; set; }

        //- @Disabled -//
        public Boolean Disabled { get; set; }
    }
}