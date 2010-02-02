#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
namespace Nalarium.Web.Processing
{
    internal static class PassThroughHandlerCreator
    {
        //- @GetHandler -//
        public static System.Web.IHttpHandler GetHandler()
        {
            return new PassThroughHttpHandler();
        }
    }
}