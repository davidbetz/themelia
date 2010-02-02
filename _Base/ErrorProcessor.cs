#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Base class for error processors.
    /// </summary>
    public abstract class ErrorProcessor : IProcessor
    {
        internal static Type _Type = typeof(ErrorProcessor);

        //+
        //- #Context -//
        protected System.Web.HttpContext Context { get; set; }

        //- #ParameterArray -//
        protected Object[] ParameterArray { get; set; }

        //- #Error -//
        protected Exception Error { get; set; }

        //+
        //- ~Initialize -//
        internal void Initialize(System.Web.HttpContext context, Exception error, params Object[] parameterArray)
        {
            this.Context = context;
            this.ParameterArray = parameterArray;
            this.Error = error;
        }

        //- @Execute -//
        /// <summary>
        /// Called when an unhandled ASP.NET exception is thrown (reminder: there is always a web domain, the default is "root"); use this method to provide custom error handling logic.
        /// </summary>
        public abstract void Execute( );
    }
}