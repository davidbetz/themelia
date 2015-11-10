#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web;
using System.Web.Caching;
using System.Web.Management;
using System.Web.UI;
using Nalarium.Activation;

namespace Nalarium.Web.Activation
{
    internal class WebExceptionTypeFactory : TypeFactory
    {
        //- @CreateObject -//
        public override Type CreateType(String text)
        {
            Type ex = null;
            switch (text)
            {
                case "http":
                    return TypeCache.InlineRegister(typeof(HttpException));
                case "httpunhandled":
                    return TypeCache.InlineRegister(typeof(HttpUnhandledException));
                case "httpcompile":
                    return TypeCache.InlineRegister(typeof(HttpCompileException));
                case "httpparse":
                    return TypeCache.InlineRegister(typeof(HttpParseException));
                case "httprequestvalidation":
                    return TypeCache.InlineRegister(typeof(HttpRequestValidationException));
                case "databasenotenabledfornotification":
                    return TypeCache.InlineRegister(typeof(DatabaseNotEnabledForNotificationException));
                case "tablenotenabledfornotification":
                    return TypeCache.InlineRegister(typeof(TableNotEnabledForNotificationException));
                case "sqlexecution":
                    return TypeCache.InlineRegister(typeof(SqlExecutionException));
                case "viewstate":
                    return TypeCache.InlineRegister(typeof(ViewStateException));
            }
            //+
            return ex;
        }
    }
}