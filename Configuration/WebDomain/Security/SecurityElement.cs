#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;
using System.Diagnostics;
using Nalarium.Configuration;
using Nalarium.Web.Security;
//+

namespace Nalarium.Web.Processing.Configuration
{
    [DebuggerDisplay("{DefaultAccessMode}, {ValidatorType}")]
    public class SecurityElement : CommentableElement
    {
        //- @DefaultAccessMode -//
        [ConfigurationProperty("defaultAccessMode", IsRequired = true)]
        public DefaultAccessMode? DefaultAccessMode
        {
            get
            {
                return (DefaultAccessMode?)this["defaultAccessMode"];
            }
            set
            {
                this["defaultAccessMode"] = value;
            }
        }

        //- @ValidatorType -//
        [ConfigurationProperty("validatorType", IsRequired = true)]
        public String ValidatorType
        {
            get
            {
                return (String)this["validatorType"];
            }
            set
            {
                this["validatorType"] = value;
            }
        }

        //- @LoginText -//
        [ConfigurationProperty("loginText", DefaultValue = "login")]
        public String LoginText
        {
            get
            {
                return (String)this["loginText"];
            }
            set
            {
                this["loginText"] = value;
            }
        }

        //- @LoginPage -//
        [ConfigurationProperty("loginPage", IsRequired = true)]
        public String LoginPage
        {
            get
            {
                return (String)this["loginPage"];
            }
            set
            {
                this["loginPage"] = value;
            }
        }

        //- @LogoutText -//
        [ConfigurationProperty("logoutText", DefaultValue = "logout")]
        public String LogoutText
        {
            get
            {
                return (String)this["logoutText"];
            }
            set
            {
                this["logoutText"] = value;
            }
        }

        //- @LogoutPage -//
        [ConfigurationProperty("logoutPage", IsRequired = false)]
        public String LogoutPage
        {
            get
            {
                return (String)this["logoutPage"];
            }
            set
            {
                this["logoutPage"] = value;
            }
        }

        //- @DefaultLoggedInTarget -//
        [ConfigurationProperty("defaultLoggedInTarget", DefaultValue = "/")]
        public String DefaultLoggedInTarget
        {
            get
            {
                return (String)this["defaultLoggedInTarget"];
            }
            set
            {
                this["defaultLoggedInTarget"] = value;
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

        //- @Exceptions -//
        [ConfigurationProperty("exceptions")]
        [ConfigurationCollection(typeof(SecurityExceptionElement), AddItemName = "add")]
        public SecurityExceptionCollection Exceptions
        {
            get
            {
                return (SecurityExceptionCollection)this["exceptions"];
            }
        }
    }
}