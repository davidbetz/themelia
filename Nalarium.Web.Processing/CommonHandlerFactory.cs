#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web;

namespace Nalarium.Web.Processing
{
    internal class CommonHandlerFactory : HandlerFactory
    {
        //- @GetHttpHandler -//
        public override IHttpHandler CreateHttpHandler(String text)
        {
            switch (text)
            {
                    //+ positive
                case "sequence":
                    return new PageEndpointHttpHandler(true);
                case "page":
                    return new PageEndpointHttpHandler(false);
                case "redirect":
                    return new RedirectHttpHandler();
                    //+ blocking (advanced)
                case "{blocked}":
                    return new BlockedHttpHandler();
                case "{notfound}":
                    return new NotFoundHttpHandler();
                case "{httpforbidden}":
                    return NonPublicHandlerFactory.CreateHttpForbiddenHandler();
                    //+ passthrough (advanced)
                case "{passthrough}":
                    return new PassThroughHttpHandler(false);
                case "{forcepassthrough}":
                    return new PassThroughHttpHandler(true);
                case "{default}":
                    return new PassThroughHttpHandler();
                case "{staticfile}":
                    return NonPublicHandlerFactory.CreateStaticFileHandler();
                    //+ config (advanced)
                case "{configviewer}":
                    return new ConfigViewerHttpHandler();
                case "{configeditor}":
                    return new ConfigEditorHttpHandler();
            }
            //+
            return null;
        }
    }
}