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
    [DebuggerDisplay("{ProcessorType}, {Priority}")]
    public class ProcessorElement : WithParameterArrayElement, IProcessorElement, IHasPriority
    {
        //- @ProcessorType -//

        //- @Enabled -//
        [ConfigurationProperty("enabled", DefaultValue = true, IsRequired = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }

        #region IHasPriority Members

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

        #endregion

        #region IProcessorElement Members

        [ConfigurationProperty("type", IsRequired = true)]
        public String ProcessorType
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

        #endregion
    }
}