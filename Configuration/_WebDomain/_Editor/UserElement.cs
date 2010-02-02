#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Configuration;
//+
//+
namespace Nalarium.Web.Processing.Configuration
{
    [System.Diagnostics.DebuggerDisplay("{Type}, {Name}, Disabled: {Disabled}")]
    public class UserElement : Nalarium.Configuration.CommentableElement
    {
        //- @Type -//
        [ConfigurationProperty("type")]
        public Nalarium.Web.Processing.UserType Type
        {
            get
            {
                return (Nalarium.Web.Processing.UserType)this["type"];
            }
        }

        //- @Name -//
        [ConfigurationProperty("name")]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
        }

        //- @PasswordHash -//
        [ConfigurationProperty("passwordHash")]
        public String PasswordHash
        {
            get
            {
                return (String)this["passwordHash"];
            }
        }

        //- @Disabled -//
        [ConfigurationProperty("disabled", DefaultValue = false)]
        public Boolean Disabled
        {
            get
            {
                return (Boolean)this["disabled"];
            }
            set
            {
                this["disabled"] = value;
            }
        }
    }
}