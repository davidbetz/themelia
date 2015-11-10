#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Configuration;
//+
namespace Nalarium.Web.Processing.Configuration
{
    [System.Diagnostics.DebuggerDisplay("{Name}, {AccessType}, {Text}, {Enabled}")]
    public class AccessRuleElement : Nalarium.Configuration.CommentableElement
    {
        //- @Name -//
        [ConfigurationProperty("name", IsRequired = false)]
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

        //- @AccessType -//
        [ConfigurationProperty("type", IsRequired = true)]
        public Nalarium.Web.Processing.AccessType AccessType
        {
            get
            {
                return (Nalarium.Web.Processing.AccessType)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }

        //- @Text -//
        [ConfigurationProperty("text", IsRequired = true)]
        public String Text
        {
            get
            {
                return (String)this["text"];
            }
            set
            {
                this["text"] = value;
            }
        }

        //- @Parameter -//
        [ConfigurationProperty("parameter", DefaultValue = " ")]
        public String Parameter
        {
            get
            {
                return (String)this["parameter"];
            }
            set
            {
                this["parameter"] = value;
            }
        }

        //- @ActionType -//
        [ConfigurationProperty("actionType", DefaultValue = Nalarium.Web.Processing.ActionType.Write)]
        public Nalarium.Web.Processing.ActionType ActionType
        {
            get
            {
                return (Nalarium.Web.Processing.ActionType)this["actionType"];
            }
            set
            {
                this["actionType"] = value;
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

        //- @Priority -//
        [ConfigurationProperty("priority", DefaultValue = 5, IsRequired = false)]
        public Int32 Priority
        {
            get
            {
                return (Int32)this["priority"];
            }
            set
            {
                this["priority"] = value;
            }
        }
    }
}