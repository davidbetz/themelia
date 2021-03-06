﻿#region Copyright
//+ Themelia Pro 2.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
#endregion
using System;
//+
namespace Themelia.Web.Processing
{
    /// <summary>
    /// Passes all requests in a particular web domain to the the HTTP redirector
    /// </summary>
    public class RedirectCatchAllProcessor : CatchAllProcessorBase
    {
        //- @GetHandler -//
        /// <summary>
        /// Runs the catchall processor; this only runs if the HTTP handler selection process did not successfully find an appropriate handler. If null is returned, the PassThrougHttpHandler is used.
        /// </summary>
        /// <param name="context">The HttpContext object.</param>
        /// <param name="requestType">Type of the request.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="path">The actual path.</param>
        /// <param name="parameterArray">The optional parameter array.</param>
        /// <returns>Instance of HTTP handler to use</returns>
        public override System.Web.IHttpHandler GetHandler(System.Web.HttpContext context, String requestType, String virtualPath, String path, params Object[] parameterArray)
        {
            String text = String.Empty;
            if (!Collection.IsNullOrEmpty(parameterArray))
            {
                text = parameterArray[0] as String;
            }
            //+
            RedirectHttpHandler redirectHttpHandler = new RedirectHttpHandler();
            if (!String.IsNullOrEmpty(text))
            {
                redirectHttpHandler.ParameterMap.Add("Destination", text);
            }
            //+
            return redirectHttpHandler;
        }
    }
}