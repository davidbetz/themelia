#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

//+
//+
using Nalarium.Web.Globalization;

namespace Nalarium.Web.Processing
{
    public class NotFoundHttpHandler : ReusableHttpHandler
    {
        //- @ProcessRequest -//
        public override void Process()
        {
            HttpExceptionThrower.Throw404(ResourceAccessor.GetString("Redirect_NotFound"));
        }
    }
}