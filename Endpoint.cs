#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Linq;
//+
namespace Nalarium.Web.Processing
{
    public class Endpoint
    {
        //- @SetViaSelectionProcessor -//
        public EndpointSetMode SetMode { get; internal set; }

        //- @RelativePathOriginalCase -//
        public String RelativePathOriginalCase
        {
            get
            {
                String text = UrlCleaner.CleanWebPathHead(Text);
                String webDomainRelativeUrl = UrlCleaner.CleanWebPathHead(NalariumContext.Current.WebDomain.RelativePathOriginalCase);
                if (webDomainRelativeUrl.Equals(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    return String.Empty;
                }
                //+
                if (String.IsNullOrEmpty(Text))
                {
                    return webDomainRelativeUrl;
                }
                else
                {
                    return UrlCleaner.CleanWebPath(webDomainRelativeUrl.Substring(text.Length + 1, webDomainRelativeUrl.Length - text.Length - 1));
                }
            }
        }

        //- @RelativePath -//
        public String RelativePath
        {
            get
            {
                return RelativePathOriginalCase.ToLower();
            }
        }

        //- @RelativePath -//
        public String Path
        {
            get
            {
                String webDomainRelativeUrl = "/" + UrlCleaner.CleanWebPath(NalariumContext.Current.WebDomain.RelativePathOriginalCase) + "/";
                String text = UrlCleaner.CleanWebPath(Text);
                if (webDomainRelativeUrl.Contains("/" + text + "/"))
                {
                    return text;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        //- @ParameterArray -//
        public String[] ParameterArray
        {
            get
            {
                return RelativePathOriginalCase.Split('/').Where(p => !String.IsNullOrEmpty(p)).ToArray();
            }
        }

        //- @OriginalText -//
        public String OriginalText { get; internal set; }

        //- @OriginalSelector -//
        public SelectorType OriginalSelector { get; internal set; }

        //- @Text -//
        public String Text { get; internal set; }

        //- @Selector -//
        public SelectorType Selector { get; internal set; }

        //- @MatchWithoutTrailingSlash -//
        public Boolean MatchWithoutTrailingSlash { get; internal set; }

        //- @ParameterMap -//
        public Map ParameterMap { get; internal set; }

        //- @ParameterValue -//
        public String ParameterValue { get; internal set; }
    }
}