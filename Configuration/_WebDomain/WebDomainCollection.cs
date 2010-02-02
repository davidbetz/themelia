#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Configuration;
//+
namespace Nalarium.Web.Processing.Configuration
{
    [System.Diagnostics.DebuggerDisplay("EnableWebDomainMissingSlash: {EnableWebDomainMissingSlash}, EnableDirectPageAccess: {EnableDirectPageAccess}")]
    public class WebDomainCollection : Nalarium.Configuration.CommentableCollection<WebDomainElement>
    {
        ////- ~EnableWebDomainMissingSlash -//
        //[ConfigurationProperty("enableWebDomainMissingSlash")]
        //internal Boolean EnableWebDomainMissingSlash
        //{
        //    get
        //    {
        //        return (Boolean)this["enableWebDomainMissingSlash"];
        //    }
        //}

        //- @AccessRuleGroup -//
        [ConfigurationProperty("accessRuleGroup")]
        public String AccessRuleGroup
        {
            get
            {
                return (String)this["accessRuleGroup"];
            }
        }

        //- ~RequireSlash -//
        [ConfigurationProperty("requireSlash")]
        internal Boolean RequireSlash
        {
            get
            {
                return (Boolean)this["requireSlash"];
            }
        }

        //- ~FaviconMode -//
        [ConfigurationProperty("faviconMode", DefaultValue = Nalarium.Web.Processing.FaviconMode.Exclusion)]
        internal Nalarium.Web.Processing.FaviconMode FaviconMode
        {
            get
            {
                return (Nalarium.Web.Processing.FaviconMode)this["faviconMode"];
            }
        }

        //- ~AllowDirectPageAccess -//
        [ConfigurationProperty("enableDirectPageAccess")]
        internal Boolean EnableDirectPageAccess
        {
            get
            {
                return (Boolean)this["enableDirectPageAccess"];
            }
        }

        //+
        //- #GetElementKey -//
        protected override Object GetElementKey(ConfigurationElement element)
        {
            return GuidCreator.GetNewGuid();
        }
    }
}