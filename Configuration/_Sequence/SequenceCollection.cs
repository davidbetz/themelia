#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Configuration;
using Nalarium.Configuration;

namespace Nalarium.Web.Processing.Configuration
{
    public class SequenceCollection : CommentableCollection<SequenceElement>
    {
        //- ~RootPath -//
        [ConfigurationProperty("rootPath", DefaultValue = "/Sequence_/")]
        public String RootPath
        {
            get
            {
                return (String)this["rootPath"];
            }
        }

        //+
        //- #GetElementKey -//
        protected override Object GetElementKey(ConfigurationElement element)
        {
            return (element as SequenceElement).Name;
        }
    }
}