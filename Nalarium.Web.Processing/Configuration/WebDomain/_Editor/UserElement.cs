#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;
using System.Diagnostics;
using Nalarium.Configuration;

//+
//+

namespace Nalarium.Web.Processing.Configuration
{
    [DebuggerDisplay("{Type}, {Name}, Disabled: {Disabled}")]
    public class UserElement : CommentableElement
    {
        //- @Type -//
        [ConfigurationProperty("type")]
        public UserType Type
        {
            get
            {
                return (UserType)this["type"];
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