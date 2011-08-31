#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web;

namespace Nalarium.Web.Processing
{
    public abstract class HandlerFactory : IFactory
    {
        internal static Type _Type = typeof(HandlerFactory);

        //+
        //- @CreateHttpHandler -//
        /// <summary>
        /// Creates an HTTP handler based on the specified alias; it is highly recommended that all custom HTTP handlers be registered in a an a handler factory to add extra performance and ease of use.
        /// </summary>
        /// <param name="text">The handler alias.</param>
        /// <returns>Instance of HTTP handler</returns>
        public abstract IHttpHandler CreateHttpHandler(String text);
    }
}