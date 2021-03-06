﻿#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.IO;
using System.Web.UI;
using Nalarium.Web.Processing.Configuration;

//+
//+

namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Used to alias a page
    /// </summary>
    internal class PageEndpointHttpHandler : SessionHttpHandler, IHasParameterMap, IHasPage
    {
        //- ~Info -//

        //+
        //- @IsDefaultRedirect -//
        public PageEndpointHttpHandler()
            : this(false)
        {
        }

        public PageEndpointHttpHandler(Boolean isSequence)
        {
            IsSequence = isSequence;
        }

        public PageEndpointHttpHandler(String target, String extra)
            : this(false, target, extra)
        {
        }

        public PageEndpointHttpHandler(Boolean isSequence, String target, String extra)
        {
            IsSequence = isSequence;
            Target = target;
            Extra = extra;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is actaully the default redirect.
        /// </summary>
        public Boolean IsDefaultRedirect { get; set; }

        //- @IsSequence -//
        /// <summary>
        /// States whether the current page alias is actually a sequence.
        /// </summary>
        public Boolean IsSequence
        {
            get
            {
                return HttpData.GetScopedItem<Boolean>(RouteActivator.Info.Scope, Info.IsSequence);
            }
            set
            {
                HttpData.SetScopedItem(RouteActivator.Info.Scope, Info.IsSequence, value);
            }
        }

        //- @Target -//
        /// <summary>
        /// Gets or sets the target initialization parameter.
        /// </summary>
        public String Target { get; set; }

        //- @Extra -//
        /// <summary>
        /// Gets or sets the extra initialization parameter.
        /// </summary>
        public String Extra { get; set; }

        #region IHasPage Members

        /// <summary>
        /// Gets the page object.
        /// </summary>
        public System.Web.UI.Page Page { get; set; }

        #endregion

        //- @DefaultParameter -//

        #region IHasParameterMap Members

        public String DefaultParameter
        {
            get
            {
                return "target";
            }
            set
            {
            }
        }

        //- @ParameterMap -//
        /// <summary>
        /// Gets or sets the parameter map.
        /// </summary>
        public Map ParameterMap { get; set; }

        #endregion

        //- @Page -//

        //+
        //- @ProcessRequest -//
        public override void Process()
        {
            SelectorType matchType;
            String matchText = String.Empty;
            //+
            var defaultPageTarget = HttpData.GetScopedItem<String>(DefaultPageInitProcessor.Info.Scope, DefaultPageInitProcessor.Info.DefaultParameter);
            if (!String.IsNullOrEmpty(defaultPageTarget))
            {
                //+ sequence target
                if (defaultPageTarget.StartsWith("{") && defaultPageTarget.EndsWith("}"))
                {
                    defaultPageTarget = GetLinkedSequenceTarget(defaultPageTarget);
                    if (!String.IsNullOrEmpty(defaultPageTarget))
                    {
                        IsSequence = true;
                    }
                }
                IsDefaultRedirect = true;
                try
                {
                    Page = PageParser.GetCompiledPageInstance("~/" + defaultPageTarget, null, Context) as System.Web.UI.Page;
                }
                catch (Exception ex)
                {
                    if (WebProcessingReportController.Reporter.Initialized)
                    {
                        var map = new Map();
                        if (IsSequence)
                        {
                            map.Add("Section", "Sequence Endpoint (" + Resource.General_DefaultRedirect + ")");
                        }
                        else
                        {
                            map.Add("Section", "Page Endpoint (" + Resource.General_DefaultRedirect + ")");
                        }
                        map.Add("Message", ex.Message);
                        map.Add("Default Path", defaultPageTarget);
                        map.Add("Exception Type", ex.GetType().FullName);
                        //+
                        WebProcessingReportController.Reporter.AddMap(map);
                    }
                    //+
                    throw;
                }
                //+ validate
                EnsureSequenceUsesSequencePageControl(defaultPageTarget);
                //+ go
                Page.ProcessRequest(Context);
            }
            else
            {
                String target = String.Empty;
                String extra = String.Empty;
                matchType = HttpData.GetScopedItem<SelectorType>(RouteActivator.Info.Scope, RouteActivator.Info.Selector);
                matchText = HttpData.GetScopedItem<String>(RouteActivator.Info.Scope, RouteActivator.Info.Text);
                if (!String.IsNullOrEmpty(Target))
                {
                    target = Target;
                    if (!String.IsNullOrEmpty(Extra))
                    {
                        extra = Extra;
                    }
                }
                else if (ParameterMap != null && ParameterMap.Count > 0)
                {
                    target = ParameterMap.Get("target", StringComparison.OrdinalIgnoreCase);
                    extra = ParameterMap.Get("extra", StringComparison.OrdinalIgnoreCase);
                }
                if (String.IsNullOrEmpty(extra))
                {
                    extra = HttpData.GetScopedItem<String>(RouteActivator.Info.Scope, Info.MatchText);
                }
                if (!String.IsNullOrEmpty(target))
                {
                    target = target.Trim();
                    //+ linked target
                    if (target.StartsWith("{") && target.EndsWith("}"))
                    {
                        //+ validate
                        if (!IsSequence)
                        {
                            throw new InvalidOperationException(String.Format(Resource.Page_EndpointRequiresFullPath, target));
                        }
                        target = GetLinkedSequenceTarget(target);
                        if (!String.IsNullOrEmpty(target))
                        {
                            IsSequence = true;
                        }
                    }
                        //+ validate
                    else if (IsSequence)
                    {
                        throw new InvalidOperationException(String.Format(Resource.Sequence_EndpointRequiresSequenceNameCode, target));
                    }
                    if (!String.IsNullOrEmpty(target))
                    {
                        //+ capture
                        if (matchText.Contains("(") && target.Contains("$"))
                        {
                            String newTarget;
                            Map map = PathMatcher.GetQueryStringVariableMap(matchType, matchText, target, out newTarget);
                            if (map != null)
                            {
                                HttpData.ImportScopedItemMap(HttpData.Info.Capture, map);
                            }
                            if (newTarget != target)
                            {
                                target = newTarget;
                            }
                        }
                        if (target.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                        {
                            target = target.Substring(2, target.Length - 2);
                        }
                        //+
                        target = UrlCleaner.CleanWebPathHead(target);
                        //+
                        if (target.Contains("://") || target.Contains(Http.Root))
                        {
                            throw new InvalidOperationException(Resource.Page_InvalidPath);
                        }
                        Int32 pageIndex = target.IndexOf("?", StringComparison.OrdinalIgnoreCase);
                        if (pageIndex > -1)
                        {
                            target = target.Substring(0, pageIndex);
                        }
                        HttpData.SetScopedItem(Http.Info.Alias, Http.Info.IsAliased, true);
                        HttpData.SetHeaderItem(HttpHeader.Response.RewriteUrl, Http.AbsoluteUrlOriginalCase);
                        try
                        {
                            Page = PageParser.GetCompiledPageInstance("~/" + target, null, Context) as System.Web.UI.Page;
                        }
                        catch (Exception ex)
                        {
                            if (WebProcessingReportController.Reporter.Initialized)
                            {
                                var map = new Map();
                                if (IsSequence)
                                {
                                    map.Add("Section", "Page Endpoint");
                                }
                                else
                                {
                                    map.Add("Section", "Sequence Endpoint");
                                }
                                map.Add("Message", ex.Message);
                                map.Add("Path", target);
                                map.Add("Exception Type", ex.GetType().FullName);
                                //+
                                WebProcessingReportController.Reporter.AddMap(map);
                            }
                            //+
                            throw;
                        }
                        //+ validate
                        EnsureSequenceUsesSequencePageControl(defaultPageTarget);
                        //+ go
                        Page.ProcessRequest(Context);
                    }
                }
            }
        }

        //- $GetLinkedSequenceTarget -// 
        private string GetLinkedSequenceTarget(String target)
        {
            String linkedKey = target.Substring(1, target.Length - 2);
            String path = ProcessingSection.GetConfigSection().Sequences.RootPath;
            String sequencePath = Path.Combine(path, linkedKey + ".aspx");
            if (File.Exists(Http.Server.MapPath(sequencePath)))
            {
                target = sequencePath;
            }
            else
            {
                if (WebProcessingReportController.Reporter.Initialized)
                {
                    var map = new Map();
                    map.Add("Section", "Sequence Endpoint");
                    map.Add("Type", "Sequence");
                    map.Add("Message", "Sequence " + sequencePath + " not found.");
                    map.Add("Key", linkedKey);
                    map.Add("Path", path);
                    //+
                    WebProcessingReportController.Reporter.AddMap(map);
                }
                target = String.Empty;
            }
            //+
            return target;
        }

        //- $EnsureSequenceUsesSequencePageControl -//
        private void EnsureSequenceUsesSequencePageControl(String path)
        {
            var sequencePage = Page as Sequence.Page;
            if (IsSequence)
            {
                if (sequencePage == null)
                {
                    throw new InvalidOperationException(String.Format(Resource.Page_SequencePageRequired, "~/" + path));
                }
            }
            else
            {
                if (sequencePage != null)
                {
                    throw new InvalidOperationException(String.Format(Resource.Page_SequencePageDetected, "~/" + path));
                }
            }
        }

        #region Nested type: Info

        internal static class Info
        {
            public const String MatchText = "MatchText";
            public const String MatchType = "MatchType";
            public const String IsSequence = "IsSequence";
        }

        #endregion
    }
}