#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;
using System.Diagnostics;
using Nalarium.Configuration;
//+

namespace Nalarium.Web.Processing.Configuration
{
    [DebuggerDisplay("{Name}, Path: {Path}, Subdomain: {Subdomain}, EnableMissingSlash: {EnableMissingSlash}, BasedOn: {BasedOn}")]
    public class WebDomainElement : CommentableElement
    {
        //- @CatchAllMode -//
        [ConfigurationProperty("catchAllMode", DefaultValue = "Custom")]
        public CatchAllMode CatchAllMode
        {
            get
            {
                return (CatchAllMode)this["catchAllMode"];
            }
        }

        //- @CatchAllInitParameter -//
        [ConfigurationProperty("catchAllInitParameter")]
        public String CatchAllInitParameter
        {
            get
            {
                return (String)this["catchAllInitParameter"];
            }
        }

        //- @Default -//
        [ConfigurationProperty("default")]
        public String Default
        {
            get
            {
                return (String)this["default"];
            }
        }

        //- @DefaultParameter -//
        [ConfigurationProperty("defaultParameter")]
        public String DefaultParameter
        {
            get
            {
                return (String)this["defaultParameter"];
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

        //- @Name -//
        [ConfigurationProperty("name")]
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

        //- @Subdomain -//
        [ConfigurationProperty("subdomain")]
        public String Subdomain
        {
            get
            {
                return (String)this["subdomain"];
            }
            set
            {
                this["subdomain"] = value;
            }
        }

        //- @Path -//
        [ConfigurationProperty("path")]
        public String Path
        {
            get
            {
                return (String)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }

        //- @BasedOn -//
        [ConfigurationProperty("basedOn")]
        public String BasedOn
        {
            get
            {
                return (String)this["basedOn"];
            }
        }

        //- @IsSealed -//
        [ConfigurationProperty("isSealed")]
        public Boolean IsSealed
        {
            get
            {
                return (Boolean)this["isSealed"];
            }
        }

        //- ~ValidateAbstract -//

        //- @IsAbstract -//
        public Boolean IsAbstract
        {
            get
            {
                return Name.StartsWith("{", StringComparison.InvariantCultureIgnoreCase) && Name.EndsWith("}", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        //- @Parameters -//
        [ConfigurationProperty("parameters")]
        [ConfigurationCollection(typeof(ParameterCollection), AddItemName = "add")]
        public ParameterCollection Parameters
        {
            get
            {
                return (ParameterCollection)this["parameters"];
            }
        }

        //- @Components -//
        [ConfigurationProperty("components")]
        [ConfigurationCollection(typeof(ComponentCollection), AddItemName = "add")]
        public ComponentCollection Components
        {
            get
            {
                return (ComponentCollection)this["components"];
            }
        }

        //- @ResetSeries -//
        [ConfigurationProperty("reset")]
        public String ResetSeries
        {
            get
            {
                return (String)this["reset"];
            }
        }

        //- @Factories -//
        [ConfigurationProperty("factories")]
        [ConfigurationCollection(typeof(FactoryElement), AddItemName = "add")]
        public FactoryCollection Factories
        {
            get
            {
                return (FactoryCollection)this["factories"];
            }
        }

        //- @Processors -//
        [ConfigurationProperty("processors")]
        [ConfigurationCollection(typeof(ProcessorElement), AddItemName = "add")]
        public ProcessorCollection Processors
        {
            get
            {
                return (ProcessorCollection)this["processors"];
            }
        }

        //- @ApplicationProcessors -//
        [ConfigurationProperty("applicationProcessors")]
        [ConfigurationCollection(typeof(ProcessorElement), AddItemName = "add")]
        public ProcessorCollection ApplicationProcessors
        {
            get
            {
                return (ProcessorCollection)this["applicationProcessors"];
            }
        }

        //- @Endpoints -//
        [ConfigurationProperty("endpoints")]
        [ConfigurationCollection(typeof(EndpointElement), AddItemName = "add")]
        public EndpointCollection Endpoints
        {
            get
            {
                return (EndpointCollection)this["endpoints"];
            }
        }

        //- @Security -//
        [ConfigurationProperty("security")]
        public SecurityElement Security
        {
            get
            {
                return (SecurityElement)this["security"];
            }
        }

        internal void ValidateAbstract()
        {
            if (IsAbstract)
            {
                if (!String.IsNullOrEmpty(Path) || !String.IsNullOrEmpty(Subdomain))
                {
                    throw new InvalidOperationException(String.Format(Resource.WebDomain_InvalidAbstractWebDomain, Name));
                }
                if (IsSealed)
                {
                    throw new InvalidOperationException(String.Format(Resource.WebDomain_AbstractWebDomainMayNotBeSealed, Name));
                }
            }
        }
    }
}