#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Diagnostics;

namespace Nalarium.Web.Processing.Data
{
    [DebuggerDisplay("{Category}, {Name}, {Value}")]
    public class ParameterData
    {
        //- @Category -//
        public String Category { get; set; }

        //- @Name -//
        public String Name { get; set; }

        //- @Value -//
        public String Value { get; set; }
    }
}