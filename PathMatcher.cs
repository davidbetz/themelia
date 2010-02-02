#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Text.RegularExpressions;
//+
namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Used to match specific text against the current URL via a specific match type.
    /// </summary>
    public static class PathMatcher
    {
        //- @Match -//
        /// <summary>
        /// Tests the specified text against the current url, path, or web domain path based on the specified match type.
        /// </summary>
        /// <param name="type">The match type.</param>
        /// <param name="text">The text to match.</param>
        /// <returns>true is there is a match, false if not</returns>
        public static Boolean Match(SelectorType type, String text)
        {
            if (text == "*") { return true; }
            //if (String.IsNullOrEmpty(type)) { return false; }
            if (String.IsNullOrEmpty(text)) { return false; }
            //+
            //type = type.ToLower(System.Globalization.CultureInfo.CurrentCulture);
            text = UrlCleaner.CleanWebPathHead(text).ToLower(System.Globalization.CultureInfo.CurrentCulture);
            text = text.Replace("~", Nalarium.Configuration.ConfigAccessor.ApplicationSettings("Domain", false));
            //+
            String reference = PathMatcher.GetReference(type).ToLower(System.Globalization.CultureInfo.CurrentCulture);
            if (type == SelectorType.Contains || type == SelectorType.PathContains)
            {
                return MatchContains(reference, text);
            }
            else if (type == SelectorType.StartsWith || type == SelectorType.PathStartsWith || type == SelectorType.WebDomainPathStartsWith)
            {
                return MatchStartsWith(reference, text);
            }
            else if (type == SelectorType.PathEquals || type == SelectorType.WebDomainPathEquals)
            {
                return MatchEquals(reference, text);
            }
            else if (type == SelectorType.EndsWith)
            {
                return MatchEndsWith(reference, text);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Obtains the path to be referenced for a particular match type.
        /// </summary>
        /// <param name="type">The match type</param>
        /// <returns>The url/path/web domain path reference</returns>
        public static String GetReference(SelectorType type)
        {
            String url = Http.RawUrl;
            String path = UrlCleaner.CleanWebPathHead(Http.RawPath);
            //+
            //if (String.IsNullOrEmpty(type)) { return String.Empty; }
            //+
            //type = type.ToLower(System.Globalization.CultureInfo.CurrentCulture);
            //+
            if (type == SelectorType.Contains || type == SelectorType.StartsWith || type == SelectorType.EndsWith)
            {
                return url;
            }
            else if (type == SelectorType.PathContains || type == SelectorType.PathStartsWith || type == SelectorType.PathEquals)
            {
                return path;
            }
            else if (type == SelectorType.WebDomainPathStartsWith || type == SelectorType.WebDomainPathEquals)
            {
                String webDomainUri = NalariumContext.Current.WebDomain.FullUrl;
                //+
                return UrlCleaner.CleanWebPathHead(url.Substring(webDomainUri.Length, url.Length - webDomainUri.Length));
            }
            else
            {
                return String.Empty;
            }
        }

        //- @Substitute -//
        public static String Substitute(SelectorType type, String matchText, String target)
        {
            Regex ex = new Regex(matchText, RegexOptions.IgnoreCase);
            String path = PathMatcher.GetReference(type);
            Match m = ex.Match(path);
            if (m.Groups.Count > 1)
            {
                for (Int32 i = 1; i < m.Groups.Count; i++)
                {
                    Group g = m.Groups[i];
                    target = target.Replace("$" + i.ToString(System.Globalization.CultureInfo.CurrentCulture), g.Value);
                }
            }
            //+
            return target;
        }

        //- @GetQueryStringVariableMap -//
        public static Map GetQueryStringVariableMap(SelectorType type, String matchText, String target, out String newTarget)
        {
            newTarget = target;
            Regex ex = new Regex(UrlCleaner.CleanWebPathHead(matchText), RegexOptions.IgnoreCase);
            String path = PathMatcher.GetReference(type);
            Match m = ex.Match(path);
            if (m.Groups.Count > 1)
            {
                for (Int32 i = 1; i < m.Groups.Count; i++)
                {
                    Group g = m.Groups[i];
                    newTarget = newTarget.Replace("$" + i.ToString(System.Globalization.CultureInfo.CurrentCulture), g.Value);
                }
            }
            String queryString = String.Empty;
            Map map = null;
            Int32 pageIndex = newTarget.IndexOf("?", StringComparison.OrdinalIgnoreCase);
            if (pageIndex > -1)
            {
                queryString = newTarget.Substring(pageIndex + 1, newTarget.Length - pageIndex - 1);
                map = new Map();
                map.AddQueryString(queryString);
            }
            //+
            return map;
        }

        //- $MatchContains -//
        private static Boolean MatchContains(String url, String text)
        {
            if (text.Contains("("))
            {
                return MatchRegex(url, text);
            }
            else
            {
                return url.Contains(text);
            }
        }

        //- $MatchEquals -//
        private static Boolean MatchEquals(String url, String text)
        {
            if (text.Contains("("))
            {
                return MatchRegex(url, text);
            }
            else
            {
                return url.Equals(text, StringComparison.OrdinalIgnoreCase);
            }
        }

        //- $MatchStartsWith -//
        private static Boolean MatchStartsWith(String url, String text)
        {
            if (text.Contains("("))
            {
                return MatchRegex(url, text);
            }
            else
            {
                return url.StartsWith(text, StringComparison.OrdinalIgnoreCase);
            }
        }

        //- $MatchEndsWith -//
        private static Boolean MatchEndsWith(String url, String text)
        {
            if (text.Contains("("))
            {
                return MatchRegex(url, text);
            }
            else
            {
                return url.EndsWith(text, StringComparison.OrdinalIgnoreCase);
            }
        }

        //- $MatchRegex -//
        private static Boolean MatchRegex(String url, String text)
        {
            return new Regex(text).IsMatch(url);
        }
    }
}