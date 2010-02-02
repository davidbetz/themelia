#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Configuration;
//+
namespace Nalarium.Web.Processing.Configuration
{
    [System.Diagnostics.DebuggerDisplay("{Enabled}")]
    public class ProfilingElement : Nalarium.Configuration.CommentableElement
    {
        //- @enabled -//
        [ConfigurationProperty("enabled", DefaultValue = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
        //- @IncludeExclusions -//
        [ConfigurationProperty("includeExclusions", DefaultValue = false)]
        public Boolean IncludeExclusions
        {
            get
            {
                return (Boolean)this["includeExclusions"];
            }
            set
            {
                this["includeExclusions"] = value;
            }
        }
    }
}