﻿#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web;

namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Base class for selection processors.
    /// </summary>
    public abstract class SelectionProcessor : IProcessor
    {
        internal static Type _Type = typeof(SelectionProcessor);

        //+
        //- #Context -//
        protected HttpContext Context { get; set; }

        //- #ParameterArray -//
        protected Object[] ParameterArray { get; set; }

        //+
        //- ~Initialize -//
        internal void Initialize(HttpContext context, params Object[] parameterArray)
        {
            Context = context;
            ParameterArray = parameterArray;
        }

        //- @Execute -//
        /// <summary>
        /// Runs the mid-processor; which runs after injection processing and just before the HTTP selection process.
        /// </summary>
        /// <param name="context">The HttpContext object.</param>
        /// <param name="parameterArray">The optional parameter array.</param>
        /// <returns>The new HTTP handler to use.  null means to use the existing handler</returns>
        public abstract IHttpHandler Execute(params Object[] parameterArray);
    }
}