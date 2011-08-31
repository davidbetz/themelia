#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Runtime.Serialization;

namespace Nalarium.Web.Processing
{
    public class InvalidProcessorException : Exception
    {
        //- @Ctor -//
        public InvalidProcessorException()
        {
        }

        public InvalidProcessorException(String message)
            : base(message)
        {
        }

        public InvalidProcessorException(String message, Exception inner)
            : base(message, inner)
        {
        }

        //- #Ctor -//
        protected InvalidProcessorException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}