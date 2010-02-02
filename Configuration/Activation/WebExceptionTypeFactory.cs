#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using Nalarium.Activation;
//+
namespace Nalarium.Web.Activation
{
    internal class WebExceptionTypeFactory : Nalarium.Activation.TypeFactory
    {
        //- @CreateObject -//
        public override Type CreateType(String text)
        {
            Type ex = null;
            switch (text)
            {
                case "http":
                    return TypeCache.InlineRegister(typeof(System.Web.HttpException));
                case "httpunhandled":
                    return TypeCache.InlineRegister(typeof(System.Web.HttpUnhandledException));
                case "httpcompile":
                    return TypeCache.InlineRegister(typeof(System.Web.HttpCompileException));
                case "httpparse":
                    return TypeCache.InlineRegister(typeof(System.Web.HttpParseException));
                case "httprequestvalidation":
                    return TypeCache.InlineRegister(typeof(System.Web.HttpRequestValidationException));
                case "databasenotenabledfornotification":
                    return TypeCache.InlineRegister(typeof(System.Web.Caching.DatabaseNotEnabledForNotificationException));
                case "tablenotenabledfornotification":
                    return TypeCache.InlineRegister(typeof(System.Web.Caching.TableNotEnabledForNotificationException));
                case "sqlexecution":
                    return TypeCache.InlineRegister(typeof(System.Web.Management.SqlExecutionException));
                case "membershipcreateuser":
                    return TypeCache.InlineRegister(typeof(System.Web.Security.MembershipCreateUserException));
                case "membershippassword":
                    return TypeCache.InlineRegister(typeof(System.Web.Security.MembershipPasswordException));
                case "viewstate":
                    return TypeCache.InlineRegister(typeof(System.Web.UI.ViewStateException));
            }
            //+
            return ex;
        }
    }
}