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
    [DebuggerDisplay("{ComponentType}, {Key}, Priority: {Priority}")]
    public class ComponentElement : CommentableElement
    {
        //- @Key -//
        [ConfigurationProperty("key", IsKey = true)]
        public String Key
        {
            get
            {
                return (String)this["key"];
            }
        }

        //- @ComponentType -//
        [ConfigurationProperty("type")]
        public String ComponentType
        {
            get
            {
                return (String)this["type"];
            }
        }

        //- @Parameters -//
        [ConfigurationProperty("parameters")]
        [ConfigurationCollection(typeof(ComponentParameterCollection), AddItemName = "add")]
        public ComponentParameterCollection Parameters
        {
            get
            {
                return (ComponentParameterCollection)this["parameters"];
            }
        }

        //- @Priority -//
        [ConfigurationProperty("priority", DefaultValue = 5, IsRequired = false)]
        public Int32 Priority
        {
            get
            {
                return (Int32)this["priority"];
            }
        }
    }
}