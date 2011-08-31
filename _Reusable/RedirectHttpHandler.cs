#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using Nalarium.Web.Globalization;

namespace Nalarium.Web.Processing
{
    public class RedirectHttpHandler : ReusableSessionHttpHandler, IHasParameterMap
    {
        //- @ParameterMap -//

        #region IHasParameterMap Members

        /// <summary>
        /// Gets or sets the parameter map.
        /// </summary>
        /// <value>The parameter map.</value>
        public Map ParameterMap { get; set; }

        //- @DefaultParameter -//
        public String DefaultParameter
        {
            get
            {
                return "destination";
            }
            set
            {
            }
        }

        #endregion

        //+
        //- @ProcessRequest -//
        public override void Process()
        {
            String destination = String.Empty;
            if (ParameterMap != null && ParameterMap.Count > 0)
            {
                destination = ParameterMap.Get("destination", StringComparison.OrdinalIgnoreCase);
            }
            if (String.IsNullOrEmpty(destination))
            {
                HttpExceptionThrower.Throw404(ResourceAccessor.GetString("Redirect_NotFound"));
            }
            //+
            Http.Redirect(destination);
        }
    }
}