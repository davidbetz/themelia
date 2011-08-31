#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Diagnostics;

namespace Nalarium.Web.Processing.Data
{
    [DebuggerDisplay("{ComponentType}, {Key}, IsInstalled: {IsInstalled}")]
    public class ComponentData
    {
        //- ~IsInstalled -//
        internal Boolean IsInstalled { get; set; }

        //- @ComponentType -//
        public String ComponentType { get; set; }

        //- @Key -//
        public String Key { get; set; }

        //- @ParameterDataList -//
        public ParameterDataList ParameterDataList { get; set; }
    }
}