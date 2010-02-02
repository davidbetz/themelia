#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing.Data
{
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class VersionData
    {
        //- @Key -//
        public String Name { get; set; }

        //- @Weight -//
        public Int32 Weight { get; set; }
    }
}