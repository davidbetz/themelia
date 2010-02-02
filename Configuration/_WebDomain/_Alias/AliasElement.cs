#region Copyright
//+ Themelia Pro 2.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
#endregion
using System;
using System.Configuration;
//+
namespace Themelia.Web.Configuration
{
    [System.Diagnostics.DebuggerDisplay("{Key}, {Target}, {Extra}")]
    public class AliasElement : Themelia.Configuration.CommentableElement
    {
        //- @Key -//
        [ConfigurationProperty("key", IsRequired = true)]
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

        //- @Extra -//
        [ConfigurationProperty("extra")]
        public String Extra
        {
            get
            {
                return (String)this["extra"];
            }
            set
            {
                this["extra"] = value;
            }
        }

        //- @Target -//
        [ConfigurationProperty("target", IsRequired = true)]
        public String Target
        {
            get
            {
                return (String)this["target"];
            }
            set
            {
                this["target"] = value;
            }
        }

        //- @Enabled -//
        [ConfigurationProperty("enabled", DefaultValue = true)]
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