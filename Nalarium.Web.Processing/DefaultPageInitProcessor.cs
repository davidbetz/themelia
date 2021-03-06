﻿#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Globalization;
using System.IO;
using Nalarium.Web.Processing.Data;
//+

namespace Nalarium.Web.Processing
{
    public class DefaultPageInitProcessor : ContextInitProcessor
    {
        //- @Info -//

        //- @OnPreHttpHandlerExecute -//
        public override InitProcessor Execute()
        {
            WebDomainData data = NalariumContext.Current.WebDomain.Configuration;
            String defaultParameter = data.DefaultParameter;
            DefaultType defaultType = data.DefaultType;
            //+
            //++
            //+ Some times you don't see the default document
            //+ being accessed, so check is default.htm exists
            //+ manually.
            //++
            String rootUrl = UrlCleaner.RemoveEndingQuestionMark(Http.RawUrl).Replace("default.html", "default.htm");
            Boolean rootRedirectActivated = ((rootUrl == "/" && (File.Exists(Context.Server.MapPath(rootUrl + "default.htm")))) || rootUrl.ToLower(CultureInfo.CurrentCulture).EndsWith("/default.htm", StringComparison.OrdinalIgnoreCase));
            Boolean webDomainRedirectActivated = String.IsNullOrEmpty(NalariumContext.Current.WebDomain.RelativePath);
            if (defaultType == DefaultType.Mvc || ((!String.IsNullOrEmpty(defaultParameter)) && (rootRedirectActivated || webDomainRedirectActivated)))
            {
                HttpData.SetScopedItem(Info.Scope, Info.DefaultType, defaultType);
                HttpData.SetScopedItem(Info.Scope, Info.DefaultParameter, defaultParameter);
            }
            //+
            return null;
        }

        #region Nested type: Info

        public static class Info
        {
            public const String Scope = "__$Nalarium$Processing";
            //+
            public const String DefaultType = "DefaultType";
            public const String DefaultParameter = "DefaultParameter";
        }

        #endregion
    }
}