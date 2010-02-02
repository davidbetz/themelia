#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing
{
    public abstract class Factory<T> : IFactory where T : new()
    {
        //- Create -//
        /// <summary>
        /// Creates an instance based on the incoming text.
        /// </summary>
        /// <param name="text">The text on which to base creation.</param>
        /// <returns>Instance of object</returns>
        public abstract T Create(String text);
    }
}