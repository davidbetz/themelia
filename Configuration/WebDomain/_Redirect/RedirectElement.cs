#region Copyright
//+ Themelia Pro 2.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
#endregion
using System;
using System.Configuration;
//+
namespace Themelia.Web.Configuration
{
    [System.Diagnostics.DebuggerDisplay("{Key}, {Destination}")]
    public class RedirectElement : Themelia.Configuration.CommentableElement
    {
        //- @Key -//
        [ConfigurationProperty("key", IsRequired = false)]
        public String Key
        {
            get
            {
                return (String)this["key"];
            }
            set
            {
                this["key"] = value;
            }
        }

        //- @Destination
        [ConfigurationProperty("destination", IsRequired = true)]
        public String Destination
        {
            get
            {
                return (String)this["destination"];
            }
            set
            {
                this["destination"] = value;
            }
        }

        //- @Enabled -//
        [ConfigurationProperty("enabled", DefaultValue = true, IsRequired = false)]
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
    }
}