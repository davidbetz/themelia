#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Base class for init processors.
    /// </summary>
    public abstract class InitProcessor : IProcessor
    {
        internal static Type _Type = typeof(InitProcessor);

        //+
        //- ~IsChained -//
        internal Boolean IsChained { get; set; }

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
        /// Runs the pre-processor; which runs before any other custom processor
        /// </summary>
        public abstract InitProcessor Execute( );
    }
}