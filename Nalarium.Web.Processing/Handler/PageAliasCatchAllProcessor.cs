#region Copyright
//+ Themelia Pro 2.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
#endregion
using System;
//+
namespace Themelia.Web.Processing
{
    /// <summary>
    /// Passes all requests in a particular web domain to the the page aliaser
    /// </summary>
    public class PageAliasCatchAllProcessor : CatchAllProcessorBase
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
            HttpData.SetScopedItem<String>(PageAliasHttpHandler.Info.Scope, RouteActivator.Info.MatchType, "contains");
            HttpData.SetScopedItem<String>(PageAliasHttpHandler.Info.Scope, RouteActivator.Info.MatchText, "/");
            HttpData.SetScopedItem<String>(PageAliasHttpHandler.Info.Scope, RouteActivator.Info.ReferenceKey, "/");
            //+
            String target = String.Empty;
            String extra = String.Empty;
            if (parameterArray.Length > 0)
            {
                target = parameterArray[0] as String;
                if (parameterArray.Length > 1)
                {
                    extra = parameterArray[1] as String;
                }
                else
                {
                    String[] partArray = target.Split(',');
                    if (partArray.Length > 1)
                    {
                        target = partArray[0].Trim();
                        extra = partArray[1].Trim();
                    }
                }
            }
            //+
            return new PageAliasHttpHandler(target, extra);
        }
    }
}