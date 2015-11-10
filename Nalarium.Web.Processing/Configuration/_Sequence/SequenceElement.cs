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
    [DebuggerDisplay("{Name}")]
    public class SequenceElement : CommentableElement
    {
        //- @Name -//
        [ConfigurationProperty("name", IsRequired = true)]
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

        //- @Views -//
        [ConfigurationProperty("views")]
        [ConfigurationCollection(typeof(ViewElement), AddItemName = "add")]
        public ViewCollection Views
        {
            get
            {
                return (ViewCollection)this["views"];
            }
        }

        //- @Versions -//
        [ConfigurationProperty("versions")]
        [ConfigurationCollection(typeof(VersionElement), AddItemName = "add")]
        public VersionCollection Versions
        {
            get
            {
                return (VersionCollection)this["versions"];
            }
        }

        //- @ExplicitVersion -//
        [ConfigurationProperty("explicitVersion")]
        public String ExplicitVersion
        {
            get
            {
                return (String)this["explicitVersion"];
            }
            set
            {
                this["explicitVersion"] = value;
            }
        }
    }
}