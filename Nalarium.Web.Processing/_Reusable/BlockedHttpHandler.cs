#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;

namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Provides either a blank screen or a denial message.
    /// </summary>
    public class BlockedHttpHandler : ReusableSessionHttpHandler, IHasParameterMap
    {
        //- @DefaultParameter -//

        #region IHasParameterMap Members

        public String DefaultParameter
        {
            get
            {
                return "text";
            }
            set
            {
            }
        }

        //- @ParameterMap -//
        public Map ParameterMap { get; set; }

        #endregion

        //+
        //- @ProcessRequest -//
        public override void Process()
        {
            String text = ParameterMap.Get("text", StringComparison.OrdinalIgnoreCase);
            if (!String.IsNullOrEmpty(text))
            {
                Response.Write(text);
            }
            //+
            Response.End();
        }
    }
}