#region Copyright
//+ Themelia Suite 2.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
#endregion
using System;
//+
namespace Themelia.Web.Processing
{
    /// <summary>
    /// Used to block access to any requests in a web domain not directly handled by Themelia
    /// </summary>
    public class HttpForbiddenCatchAllProcessor : CatchAllProcessorBase
    {
        //- @GetHandler -//
        /// <summary>
        /// Runs the catchall processor; this only runs if the HTTP handler selection process did not successfully find an appropriate handler.
        /// </summary>
        /// <param name="context">The HttpContext object.</param>
        /// <param name="requestType">Type of the request.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="path">The actual path.</param>
        /// <param name="parameterArray">The optional parameter array.</param>
        /// <returns>Instance of HTTP handler to use</returns>
        public override System.Web.IHttpHandler GetHandler(System.Web.HttpContext context, String requestType, String virtualPath, String path, params Object[] parameterArray)
        {
            return NonPublicHandlerFactory.CreateHttpForbiddenHandler();
        }
    }
}