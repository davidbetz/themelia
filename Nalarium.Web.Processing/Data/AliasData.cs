#region Copyright
//+ Themelia Pro 2.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
#endregion
using System;
//+
namespace Themelia.Web.Processing.Data
{
    [System.Diagnostics.DebuggerDisplay("{Key}, {Target}, Extra: {Extra}")]
    public class AliasData
    {
        //- @Key -//
        public String Key { get; set; }

        //- @Target -//
        public String Target { get; set; }

        //- @Source -//
        public String Extra { get; set; }

        //- ~Extra2 -//
        internal String Extra2 { get; set; }

        //- ~Source -//
        internal String Source { get; set; }

        //+
        //- @Create -//
        public static AliasData Create(String key, String target)
        {
            return Create(key, target, String.Empty);
        }

        private static AliasData Create(String key, String target, String extra)
        {
            return new AliasData
            {
                Key = key,
                Target = target,
                Extra = extra
            };
        }
    }
}