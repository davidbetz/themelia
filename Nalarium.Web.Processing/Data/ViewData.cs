#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Diagnostics;

namespace Nalarium.Web.Processing.Data
{
    [DebuggerDisplay("{Name}")]
    public class ViewData
    {
        //- @Key -//
        public String Name { get; set; }

        //- @ViewUsed -//
        public String ViewUsed { get; set; }
    }
}