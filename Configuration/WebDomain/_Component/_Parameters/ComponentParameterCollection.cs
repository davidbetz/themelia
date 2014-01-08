#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;

//+
//+

namespace Nalarium.Web.Processing.Configuration
{
    public class ComponentParameterCollection : Nalarium.Configuration.ParameterCollection
    {
        //- @Reset -//
        [ConfigurationProperty("reset")]
        public Boolean ResetCollection
        {
            get
            {
                return (Boolean)this["reset"];
            }
            set
            {
                this["reset"] = value;
            }
        }
    }
}