#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web;

namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Base class for state processors.
    /// </summary>
    public abstract class StateProcessor : IProcessor
    {
        internal static Type _Type = typeof(StateProcessor);

        //+
        //- ~IsChained -//
        internal Boolean IsChained { get; set; }

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
        /// Runs the post state processor
        /// </summary>
        public abstract StateProcessor Execute();
    }
}