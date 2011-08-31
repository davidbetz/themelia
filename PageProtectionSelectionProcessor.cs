#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web;
using Nalarium.Web.Processing.Configuration;

namespace Nalarium.Web.Processing
{
    internal class PageProtectionSelectionProcessor : SelectionProcessor
    {
        //- @Execute -//
        public override IHttpHandler Execute(params Object[] parameterArray)
        {
            String first = Http.GetUrlPart(Position.First);
            if ((first == "shared_" || first == "page_" || first == "sequence_") && !ProcessingSection.GetConfigSection().WebDomain.EnableDirectPageAccess)
            {
                return NonPublicHandlerFactory.CreateHttpNotFoundHandler();
            }
            //+
            return null;
        }
    }
}