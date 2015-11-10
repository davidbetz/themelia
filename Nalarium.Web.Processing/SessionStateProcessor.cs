#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;

namespace Nalarium.Web.Processing
{
    public class SessionStateProcessor : StateProcessor
    {
        //- ~Info -//

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
                var map = HttpData.GetScopedItem<StringObjectMap>(RouteActivator.Info.Scope, Info.Items);
                if (map == null)
                {
                    map = new StringObjectMap();
                    HttpData.SetScopedItem(RouteActivator.Info.Scope, Info.Items, map);
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
        public override StateProcessor Execute()
        {
            HttpData.ImportSessionMap(Data);
            return null;
        }

        #region Nested type: Info

        internal static class Info
        {
            public const String Items = "Items";
        }

        #endregion
    }
}