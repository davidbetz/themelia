#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Nalarium.Web.Processing.Data;
//+

namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Provides information regarding the current web domain.
    /// </summary>
    [DebuggerDisplay("{Name}, {PathSegment}, {RelativePath}")]
    public class WebDomain
    {
        //- ~Info -//

        private static readonly Object _lock = new Object();

        //+
        //- ~Configuration -//
        /// <summary>
        /// Gets the current web domain information
        /// </summary>
        /// <value>The current web domain information</value>
        public WebDomainData Configuration { get; set; }

        //- @IsRoot -//
        /// <summary>
        /// Gets a value indicating whether this instance is root.
        /// </summary>
        /// <value><c>true</c> if this instance is root; otherwise, <c>false</c>.</value>
        public Boolean IsRoot
        {
            get
            {
                return Name == "root";
            }
        }

        //- @Name -//
        /// <summary>
        /// Gets the current web domain name.
        /// </summary>
        /// <value>The current web domain name.</value>
        public String Name
        {
            get
            {
                return Configuration.Name;
            }
        }

        //- @PathSegment -//
        /// <summary>
        /// Gets the current web domain path segment.
        /// </summary>
        public String PathSegment
        {
            get
            {
                return Configuration.Path;
            }
        }

        //- @FullUrl -//
        /// <summary>
        /// Gets the full web domain path.
        /// </summary>
        public String FullUrl
        {
            get
            {
                return Http.Root + "/" + UrlCleaner.CleanWebPath(PathSegment);
            }
        }

        //- @PathPartArray -//
        /// <summary>
        /// Gets the current web domain path part array.
        /// </summary>
        public String[] PathPartArray
        {
            get
            {
                return RelativePath.ToLower(CultureInfo.CurrentCulture).Split('/').Where(p => !String.IsNullOrEmpty(p)).ToArray();
            }
        }

        //- @RelativePathOriginalCase -//
        /// <summary>
        /// Gets the current URL relative to the web domain root with the original casing.
        /// </summary>
        /// <returns>Relative URL for the current web domain.</returns>
        public String RelativePathOriginalCase
        {
            get
            {
                String absoluteUrl = Http.AbsoluteUrlOriginalCase;
                String webDomainUrl = String.Empty;
                if (String.IsNullOrEmpty(GetCleanWebDomain(NalariumContext.Current.WebDomain.Configuration.Name)))
                {
                    webDomainUrl = Http.Root + "/";
                }
                else
                {
                    webDomainUrl = Http.Root + "/" + NalariumContext.Current.WebDomain.Configuration.Path + "/";
                }
                Int32 offset = (Configuration.FoundWithoutTrailingSlash ? 1 : 0);
                if (absoluteUrl.Length >= webDomainUrl.Length - offset)
                {
                    return absoluteUrl.Substring(webDomainUrl.Length - offset, absoluteUrl.Length - webDomainUrl.Length + offset);
                }
                //+
                return absoluteUrl;
            }
        }

        //- @RelativePath -//
        /// <summary>
        /// Gets the current URL relative to the web domain root.
        /// </summary>
        /// <returns>Relative URL for the current web domain.</returns>
        public String RelativePath
        {
            get
            {
                return RelativePathOriginalCase.ToLower();
            }
        }

        //- $GetCleanWebDomain -//
        private static String GetCleanWebDomain(String webDomainName)
        {
            WebDomainData data = GetWebDomainData(webDomainName);
            if (data != null)
            {
                String name = data.Name;
                if (name == "root")
                {
                    return String.Empty;
                }
                //+
                return name;
            }
            //+
            return null;
        }

        //- $GetWebDomainData -//
        private static WebDomainData GetWebDomainData(String webDomainName)
        {
            lock (_lock)
            {
                if (String.IsNullOrEmpty(webDomainName))
                {
                    webDomainName = "root";
                }
                //+
                return WebDomainDataList.AllWebDomainData.SingleOrDefault(p => p.Name == webDomainName);
            }
        }

        #region Nested type: Info

        internal static class Info
        {
            public const String ActiveData = "ActiveData";
            public const String WebDomain = "WebDomain";
        }

        #endregion
    }
}