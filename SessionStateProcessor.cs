#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing
{
    public class SessionStateProcessor : StateProcessor
    {
        //- ~Info -//
        internal static class Info
        {
            public const String Items = "Items";
        }

        //+
        //- @Data -//
        /// <summary>
        /// Gets the data bucket; used for setting information that will be inserted into session when session is initialized.
        /// </summary>
        /// <value>The data.</value>
        public static StringObjectMap Data
        {
            get
            {
                StringObjectMap map = HttpData.GetScopedItem<StringObjectMap>(RouteActivator.Info.Scope, Info.Items);
                if (map == null)
                {
                    map = new StringObjectMap();
                    HttpData.SetScopedItem<StringObjectMap>(RouteActivator.Info.Scope, Info.Items, map);
                }
                //+
                return map;
            }
        }

        //- @Count -//
        public static Int32 EntryCount
        {
            get
            {
                return Data.Count;
            }
        }

        //- @Execute -//
        public override StateProcessor Execute( )
        {
            Nalarium.Web.HttpData.ImportSessionMap<Object>(Data);
            return null;
        }
    }
}