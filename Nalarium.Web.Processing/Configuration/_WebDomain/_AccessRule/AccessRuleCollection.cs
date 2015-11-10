#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Configuration;
//+
namespace Nalarium.Web.Processing.Configuration
{
    public class AccessRuleCollection : Nalarium.Configuration.CommentableCollection<AccessRuleElement>
    {
        //- @DefaultAccessMode -//
        [ConfigurationProperty("defaultMode", DefaultValue = Nalarium.Web.Security.DefaultAccessMode.Allow)]
        public Nalarium.Web.Security.DefaultAccessMode DefaultAccessMode
        {
            get
            {
                return (Nalarium.Web.Security.DefaultAccessMode)this["defaultMode"];
            }
            set
            {
                this["defaultMode"] = value;
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
        [ConfigurationProperty("actionType")]
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

        //+
        //- #GetElementKey -//
        protected override Object GetElementKey(ConfigurationElement element)
        {
            AccessRuleElement AccessRuleElement = (AccessRuleElement)element;
            //+
            return AccessRuleElement.AccessType + ":" + AccessRuleElement.Text;
        }
    }
}