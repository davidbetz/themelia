#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;
using System.Diagnostics;
using Nalarium.Configuration;

namespace Nalarium.Web.Processing.Configuration
{
    [DebuggerDisplay("{Selector}, {Type}, {Text}")]
    public class EndpointElement : WithParameterArrayElement
    {
        //- ~RequireSlash -//
        [ConfigurationProperty("requireSlash")]
        internal Boolean RequireSlash
        {
            get
            {
                return (Boolean)this["requireSlash"];
            }
        }

        //- @Selector -//
        [ConfigurationProperty("selector", DefaultValue = SelectorType.WebDomainPathStartsWith)]
        public SelectorType Selector
        {
            get
            {
                return (SelectorType)this["selector"];
            }
            set
            {
                this["selector"] = value;
            }
        }

        //- @Type -//
        [ConfigurationProperty("type", IsRequired = true)]
        public String Type
        {
            get
            {
                return (String)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }

        //- @AccessRuleGroup -//
        [ConfigurationProperty("accessRuleGroup")]
        public String AccessRuleGroup
        {
            get
            {
                return (String)this["accessRuleGroup"];
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
        [ConfigurationProperty("parameter")]
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

        //- @Endpoints -//
        [ConfigurationProperty("subEndpoints")]
        [ConfigurationCollection(typeof(EndpointElement), AddItemName = "add")]
        public EndpointCollection SubEndpoints
        {
            get
            {
                return (EndpointCollection)this["subEndpoints"];
            }
        }
    }
}