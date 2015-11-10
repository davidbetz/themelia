#region Copyright
//+ Themelia Pro 2.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
#endregion
using System;
using System.Configuration;
//+
namespace Themelia.Web.Configuration
{
    public class RedirectCollection : Themelia.Configuration.CommentableCollection<RedirectElement>
    {
        //- #GetElementKey -//
        protected override Object GetElementKey(ConfigurationElement element)
        {
            return GuidCreator.GetNewGuid();
        }
    }
}