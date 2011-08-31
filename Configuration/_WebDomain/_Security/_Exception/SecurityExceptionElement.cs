#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;
using System.Diagnostics;
using Nalarium.Configuration;

namespace Nalarium.Web.Processing.Configuration
{
    [DebuggerDisplay("{Key}")]
    public class SecurityExceptionElement : CommentableElement
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
    }
}