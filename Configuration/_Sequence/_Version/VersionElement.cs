#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Configuration;
//+
namespace Nalarium.Web.Processing.Configuration
{
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class VersionElement : Nalarium.Configuration.CommentableElement
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

        //- @Weight -//
        [ConfigurationProperty("weight")]
        public Int32 Weight
        {
            get
            {
                return (Int32)this["weight"];
            }
            set
            {
                this["weight"] = value;
            }
        }
    }
}