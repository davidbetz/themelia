#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing
{
    public class InvalidFactoryException : Exception
    {
        //- @Ctor -//
        public InvalidFactoryException() { }
        public InvalidFactoryException(String message) : base(message) { }
        public InvalidFactoryException(String message, Exception inner) : base(message, inner) { }

        //- #Ctor -//
        protected InvalidFactoryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}