#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;

namespace Nalarium.Web.Processing
{
    public abstract class ProcessorFactory : IFactory
    {
        internal static Type _Type = typeof(ProcessorFactory);

        //+
        //- @CreateProcessor -//
        /// <summary>
        /// Creates an processor based on the specified text.
        /// </summary>
        /// <param name="text">The text used to create the processor.</param>
        /// <returns>The created processor.</returns>
        public abstract IProcessor CreateProcessor(String text);
    }
}