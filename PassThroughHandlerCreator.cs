#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System.Web;

namespace Nalarium.Web.Processing
{
    internal static class PassThroughHandlerCreator
    {
        //- @GetHandler -//
        public static IHttpHandler GetHandler()
        {
            return new PassThroughHttpHandler();
        }
    }
}