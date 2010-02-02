#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Configuration;
//+
namespace Nalarium.Web.Processing.Configuration
{
    public class ProcessorCollection : Nalarium.Configuration.CommentableCollection<ProcessorElement>
    {
        //- #GetElementKey -//
        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ProcessorElement)element).ProcessorType;
        }
    }
}