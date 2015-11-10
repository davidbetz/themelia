#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Nalarium.Web.Processing.Data;
//+

namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Provides information regarding the current web domain.
    /// </summary>
    [DebuggerDisplay("{Name}, {Path}, {RelativeUrl}")]
    public static class WebDomainOld
    {
        //- ~Info -//

        //+
        //- ~CurrentData -//
        /// <summary>
        /// Gets the current web domain information
        /// </summary>
        /// <value>The current web domain information</value>
        public static WebDomainData Current
        {
            get
            {
                return HttpData.GetScopedItem<WebDomainData>(RouteActivator.Info.Scope, Info.ActiveData);
            }
            set
            {
                if (value != null)
                {
                    HttpData.SetScopedItem(RouteActivator.Info.Scope, Info.ActiveData, value);
                }
            }
        }

        //- @IsRoot -//
        /// <summary>
        /// Gets a value indicating whether this instance is root.
        /// </summary>
        /// <value><c>true</c> if this instance is root; otherwise, <c>false</c>.</value>
        public static Boolean IsRoot
        {
            get
            {
                return Name == "root";
            }
        }

        //- @GetCleanWebDomain -//

        //- @Name -//
        /// <summary>
        /// Gets the current web domain name.
        /// </summary>
        /// <value>The current web domain name.</value>
        public static String Name
        {
            get
            {
                return Current.Name;
            }
        }

        //- @PathWithLeadingSlash -//
        /// <summary>
        /// Gets the current web domain path with conditional leading slash 
        /// </summary>
        /// <value>The current web domain raw path with leading slash if the current web domain is not root, otherwise a blank string</value>
        public static String PathWithLeadingSlash
        {
            get
            {
                if (String.IsNullOrEmpty(Current.Path))
                {
                    return String.Empty;
                }
                //+
                return "/" + Current.Path;
            }
        }

        //- @Path -//
        /// <summary>
        /// Gets the current web domain path.
        /// </summary>
        /// <value>The current web domain raw path.</value>
        public static String Path
        {
            get
            {
                return Current.Path;
            }
        }

        //- @PathPartArray -//
        /// <summary>
        /// Gets the current web domain path part array.
        /// </summary>
        public static String[] PathPartArray
        {
            get
            {
                return RelativeUrl.ToLower(CultureInfo.CurrentCulture).Split('/').Where(p => !String.IsNullOrEmpty(p)).ToArray();
            }
        }

        //+
        //- @Uri -//
        /// <summary>
        /// Gets the current web domain URI.
        /// </summary>
        /// <returns>Uri instance representing the current web domain.</returns>
        public static Uri Uri
        {
            get
            {
                return GetUri(Path);
            }
        }

        //- @RelativeUrl -//
        /// <summary>
        /// Gets the current URL relative to the web domain root.
        /// </summary>
        /// <returns>Relative URL for the current web domain.</returns>
        public static String RelativeUrl
        {
            get
            {
                String absoluteUrl = Http.AbsoluteUrl;
                String webDomainUrl = Url;
                Int32 offset = (NalariumContext.Current.WebDomain.Configuration.FoundWithoutTrailingSlash ? 1 : 0);
                if (absoluteUrl.Length >= webDomainUrl.Length - offset)
                {
                    return absoluteUrl.Substring(webDomainUrl.Length - offset, absoluteUrl.Length - webDomainUrl.Length + offset);
                }
                //+
                return absoluteUrl;
            }
        }

        //- @Url -//
        /// <summary>
        /// Gets the current web domain URL.
        /// </summary>
        /// <returns>Url for the current web domain.</returns>
        public static String Url
        {
            get
            {
                return GetUrl();
            }
        }

        /// <summary>
        /// Returns the name of the current web domain.
        /// </summary>
        /// <returns>The name of the web domain; blank if "root"</returns>
        public static String GetCleanWebDomain()
        {
            String name = NalariumContext.Current.WebDomain.Configuration.Name;
            if (name == "root")
            {
                return String.Empty;
            }
            //+
            return name;
        }

        /// <summary>
        /// Returns the name of the current web domain.
        /// </summary>
        /// <param name="webDomainName">The web domain name.</param>
        /// <returns>The name of the web domain; blank if "root"</returns>
        public static String GetCleanWebDomain(String webDomainName)
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

        //- @GetUri -//
        /// <summary>
        /// Gets the current web domain URI.
        /// </summary>
        /// <returns>Uri instance representing the current web domain.</returns>
        public static Uri GetUri()
        {
            return new Uri(GetUrl(NalariumContext.Current.WebDomain.Configuration.Path));
        }

        /// <summary>
        /// Gets the web domain URI for a particular web domain
        /// </summary>
        /// <param name="webDomainName">The web domain name.</param>
        /// <returns>Uri instance representing a particular web domain.</returns>
        public static Uri GetUri(String webDomainName)
        {
            WebDomainData data = GetWebDomainData(webDomainName);
            if (data != null)
            {
                return new Uri(GetUrl(data.Path));
            }
            //+
            return null;
        }

        //- @GetUrl -//
        /// <summary>
        /// Gets the current web domain URL.
        /// </summary>
        /// <returns>Url for the current web domain.</returns>
        public static String GetUrl()
        {
            String url;
            if (String.IsNullOrEmpty(GetCleanWebDomain(NalariumContext.Current.WebDomain.Configuration.Name)))
            {
                url = Http.Root + "/";
            }
            else
            {
                url = Http.Root + "/" + NalariumContext.Current.WebDomain.Configuration.Path + "/";
            }
            //+
            return url;
        }

        /// <summary>
        /// Gets the current web domain URL.
        /// </summary>
        /// <param name="webDomainName">The web domain name.</param>
        /// <returns>Url for a particular web domain.</returns>
        public static String GetUrl(String webDomainName)
        {
            WebDomainData data = GetWebDomainData(webDomainName);
            if (data != null)
            {
                String url;
                if (String.IsNullOrEmpty(GetCleanWebDomain(data.Path)))
                {
                    url = Http.Root + "/";
                }
                else
                {
                    url = Http.Root + "/" + data.Path + "/";
                }
                //+
                return url;
            }
            //+
            return String.Empty;
        }

        //- @GetWebDomainData -//
        /// <summary>
        /// Returns the WebDomainData object for a particular web domain. 
        /// </summary>
        /// <param name="webDomainName">The web domain name used to lookup the web domain.</param>
        /// <returns>A WebDomainData object or null if the web domain name is invalid.</returns>
        public static WebDomainData GetWebDomainData(String webDomainName)
        {
            var readerWriterLock = new ReaderWriterLock();
            readerWriterLock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                if (String.IsNullOrEmpty(webDomainName))
                {
                    webDomainName = "root";
                }
                //+
                return WebDomainDataList.AllWebDomainData.SingleOrDefault(p => p.Name == webDomainName);
            }
            finally
            {
                readerWriterLock.ReleaseReaderLock();
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