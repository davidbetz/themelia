#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;
using ConfigurationSection = Nalarium.Configuration.ConfigurationSection;

namespace Nalarium.Web.Processing.Configuration
{
    /// <summary>
    /// Provides access to the configuration section.
    /// </summary>
    public class ProcessingSection : ConfigurationSection
    {
        //- ~EnableConfigViewer -//
        [ConfigurationProperty("enableConfigViewer")]
        internal Boolean EnableConfigViewer
        {
            get
            {
                return (Boolean)this["enableConfigViewer"];
            }
        }

        //- ~EnableConfigEditor -//
        [ConfigurationProperty("enableConfigEditor")]
        internal Boolean EnableConfigEditor
        {
            get
            {
                return (Boolean)this["enableConfigEditor"];
            }
        }

        //- @DisableProcessing -//
        [ConfigurationProperty("disableProcessing", DefaultValue = false)]
        public Boolean DisableProcessing
        {
            get
            {
                return (Boolean)this["disableProcessing"];
            }
        }

        //- @Editor -//
        [ConfigurationProperty("editor")]
        public EditorElement Editor
        {
            get
            {
                return (EditorElement)this["editor"];
            }
        }

        //- @Profiling -//
        [ConfigurationProperty("profiling")]
        public ProfilingElement Profiling
        {
            get
            {
                return (ProfilingElement)this["profiling"];
            }
        }

        //- @WebDomain -//
        [ConfigurationProperty("webDomains")]
        [ConfigurationCollection(typeof(WebDomainElement), AddItemName = "add")]
        public WebDomainCollection WebDomain
        {
            get
            {
                return (WebDomainCollection)this["webDomains"];
            }
        }

        //- @Sequences -//
        [ConfigurationProperty("sequences")]
        [ConfigurationCollection(typeof(SequenceElement), AddItemName = "add")]
        public SequenceCollection Sequences
        {
            get
            {
                return (SequenceCollection)this["sequences"];
            }
        }

        //+
        //- @GetConfigSection -//
        /// <summary>
        /// Gets the config section.
        /// </summary>
        /// <returns>Configuration section</returns>
        public static ProcessingSection GetConfigSection()
        {
            return GetConfigSection<ProcessingSection>("nalarium/web.processing");
        }
    }
}