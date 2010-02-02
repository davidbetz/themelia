#region Copyright
//+ Themelia Pro 2.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
#endregion
using System;
//+
namespace Themelia.Web.Processing.Data
{
    [System.Diagnostics.DebuggerDisplay("{Key}, {Destination}")]
    public class RedirectData
    {
        //- @Key -//
        public String Key { get; set; }

        //- @Destination -//
        public String Destination { get; set; }

        //- ~Source -//
        internal String Source { get; set; }

        //+
        //- @Create -//
        private static RedirectData Create(String key, String destination)
        {
            return new RedirectData
            {
                Key = key,
                Destination = destination
            };
        }
    }
}