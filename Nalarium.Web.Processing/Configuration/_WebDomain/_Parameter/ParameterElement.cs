﻿#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;
using System.Diagnostics;
using Nalarium.Configuration.AppConfig;

namespace Nalarium.Web.Processing.Configuration
{
    [DebuggerDisplay("{Category}")]
    public class ParameterElement : Nalarium.Configuration.AppConfig.Parameter.ParameterElement
    {
        //- @Category -//
        [ConfigurationProperty("category")]
        public String Category
        {
            get
            {
                return (String)this["category"];
            }
        }
    }
}