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
    [DebuggerDisplay("EnableWebDomainMissingSlash: {EnableWebDomainMissingSlash}, EnableDirectPageAccess: {EnableDirectPageAccess}")]
    public class WebDomainCollection : CommentableCollection<WebDomainElement>
    {
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
        [ConfigurationProperty("faviconMode", DefaultValue = FaviconMode.Exclusion)]
        internal FaviconMode FaviconMode
        {
            get
            {
                return (FaviconMode)this["faviconMode"];
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