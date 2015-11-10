#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using Nalarium.Data.Cached;

namespace Nalarium.Web.Processing
{
    public abstract class PageInitializer
    {
        //- ~PageName -//
        public PageInitializer()
        {
            ModelTypeMap = new Map<String, Type>();
            Model = new Map<String, Model>();
        }

        internal String PageName { get; set; }

        //- ~SegmentName -//
        internal String SegmentName { get; set; }

        //- ~DataScope -//
        internal String DataScope
        {
            get
            {
                return "__$Initializer$" + SegmentName;
            }
        }

        //- ~PageName -//
        internal Page Page { get; set; }

        //- @DataLookupList -//
        protected internal Map<String, Type> ModelTypeMap { get; set; }

        //- @DataLookupList -//
        protected internal Map<String, Model> Model { get; set; }

        //- ~PageName -//
        protected String PageTitle
        {
            get
            {
                if (Page == null)
                {
                    return null;
                }
                //+
                return Page.Title;
            }
            set
            {
                if (Page == null)
                {
                    return;
                }
                //+
                Page.Title = value;
            }
        }

        //- @ViewData -//
        protected Map ViewData
        {
            get
            {
                if (Page == null)
                {
                    return null;
                }
                //+
                return Page.ViewData;
            }
        }

        //+
        //- @Ctor -//

        //- #InitData -//
        protected internal virtual void InitData()
        {
        }

        //- @GetCache -//
        public T GetCache<T>(String name)
        {
            return CachedDataFactory.Get<T>(DataScope, name);
        }

        //- @RegisterData -//
        public Boolean RegisterData<T>(String name, Func<T> runner)
        {
            return CachedDataFactory.Register(DataScope, name, runner);
        }

        //- @SetPage -//
        internal void SetPage(Page page)
        {
            Page = page;
        }
    }
}