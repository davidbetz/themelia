#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Nalarium.Activation;
using Nalarium.Reflection;

namespace Nalarium.Web.Processing
{
    public abstract class Page : System.Web.UI.Page
    {
        private readonly Map<String, PageInitMetadata> _initializerCache = new Map<String, PageInitMetadata>();
        private readonly Object _lock = new Object();

        public Page()
        {
            var pageEndpointHttpHandler = (HttpContext.Current.CurrentHandler as PageEndpointHttpHandler);
            if (pageEndpointHttpHandler == null)
            {
                return;
            }
            if (pageEndpointHttpHandler.ParameterMap == null)
            {
                return;
            }
            String target = pageEndpointHttpHandler.ParameterMap["target"];
            if (String.IsNullOrEmpty(target))
            {
                return;
            }
            Int32 pageTextIndex = target.IndexOf("Page_/", StringComparison.InvariantCultureIgnoreCase);
            if (pageTextIndex == -1)
            {
                return;
            }
            Int32 slash = target.IndexOf("/", pageTextIndex + 6);
            if (slash == -1)
            {
                return;
            }
            String segmentName = target.Substring(pageTextIndex + 6, slash - pageTextIndex - 6);
            String pageName = target.Substring(slash + 1, target.Length - slash - 6);
            //+
            Type pageInitializerType;
            lock (_lock)
            {
                if (_initializerCache.ContainsKey(segmentName))
                {
                    PageInitMetadata metadata = _initializerCache[segmentName];
                    PageInitializer = metadata.PageInitializer;
                    pageInitializerType = metadata.Type;
                }
                else
                {
                    List<Type> pageInitializerTypeList = ScannedTypeCache.GetTypeData("pageInitializer");
                    if (pageInitializerTypeList == null)
                    {
                        return;
                    }
                    pageInitializerType = pageInitializerTypeList.SingleOrDefault(p => p.Name.EndsWith(segmentName + "Initializer"));
                    if (pageInitializerType == null)
                    {
                        return;
                    }
                    PageInitializer = ObjectCreator.Create(pageInitializerType) as PageInitializer;
                    _initializerCache.Add(segmentName, new PageInitMetadata
                                                       {
                                                           PageInitializer = PageInitializer,
                                                           Type = pageInitializerType
                                                       });
                    if (PageInitializer == null)
                    {
                        return;
                    }
                    PageInitializer.SegmentName = segmentName;
                    PageInitializer.PageName = pageName;
                    PageInitializer.SetPage(this);
                    PageInitializer.InitData();
                }
            }
            MethodInfo pageMethodInfo = pageInitializerType.GetMethod(pageName);
            var runForVerbsAttribute = AttributeReader.ReadMethodAttribute<RunForVerbsAttribute>(pageMethodInfo);
            if (runForVerbsAttribute == null)
            {
                return;
            }
            ViewData = new Map();
            if (Http.Method == HttpVerbs.Get && (runForVerbsAttribute.HttpVerbs & HttpVerbs.Get) == HttpVerbs.Get ||
                Http.Method == HttpVerbs.Post && (runForVerbsAttribute.HttpVerbs & HttpVerbs.Post) == HttpVerbs.Post)
            {
                pageMethodInfo.Invoke(PageInitializer, null);
            }
        }

        //+
        protected PageInitializer PageInitializer { get; set; }

        //- @ViewData -//
        protected internal Map ViewData { get; set; }

        //- ~IsSimplePage -//
        internal Boolean IsSimplePage { get; set; }

        #region Nested type: PageInitMetadata

        internal class PageInitMetadata
        {
            //- @PageInitializer -//
            public PageInitializer PageInitializer { get; set; }

            //- @Type -//
            public Type Type { get; set; }
        }

        #endregion

        //+
        //- @Ctor -//
    }
}