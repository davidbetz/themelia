#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Base class for override processors.
    /// </summary>
    public abstract class OverrideProcessor : IProcessor
    {
        internal static Type _Type = typeof(OverrideProcessor);

        //+
        //- #Context -//
        protected System.Web.HttpContext Context { get; set; }

        //- #ParameterArray -//
        protected Object[] ParameterArray { get; set; }

        //+
        //- ~Initialize -//
        internal void Initialize(System.Web.HttpContext context, params Object[] parameterArray)
        {
            this.Context = context;
            this.ParameterArray = parameterArray;
        }

        //- @Execute -//
        /// <summary>
        /// Runs the post-processor; which runs after catch-all processors and gives a final opportunity to change the active HTTP handler
        /// </summary>
        /// <param name="context">The HttpContext object.</param>
        /// <param name="currentHttpHandler">The current HTTP handler.</param>
        /// <param name="parameterArray">The optional parameter array.</param>
        /// <returns>The new HTTP handler to use.  null means to use the existing handler</returns>
        public abstract System.Web.IHttpHandler Execute(System.Web.IHttpHandler currentHttpHandler);
    }
}