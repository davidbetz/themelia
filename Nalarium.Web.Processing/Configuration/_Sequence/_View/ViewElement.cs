#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;
using System.Diagnostics;
using Nalarium.Configuration.AppConfig;

namespace Nalarium.Web.Processing.Configuration
{
    [DebuggerDisplay("{Name}")]
    public class ViewElement : CommentableElement
    {
        //- @Name -//
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        //- @ViewUsed -//
        [ConfigurationProperty("viewUsed")]
        public String ViewUsed
        {
            get
            {
                return (String)this["viewUsed"];
            }
            set
            {
                this["viewUsed"] = value;
            }
        }
    }
}