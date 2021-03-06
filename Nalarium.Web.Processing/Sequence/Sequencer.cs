#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright � Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using Nalarium.Web.Processing.Configuration;
using Nalarium.Web.Processing.Data;
//+

namespace Nalarium.Web.Processing.Sequence
{
    /// <summary>
    /// Holds all sequence views and versions.
    /// </summary>
    [ParseChildren(false)]
    [ControlBuilder(typeof(ViewVersionControlBuilder))]
    public class Sequencer : Control
    {
        //- @Info -//

        //+
        private static readonly Object _lock = new Object();
        private static readonly Map<String, SequenceType> ValidatedSequenceMap = new Map<String, SequenceType>();

        //+ field
        //protected ViewData _activeViewData;
        protected String _activeVersionName;
        protected String _activeViewName;
        protected Boolean _ensureViewConsistency = true;
        protected String _sequenceName;

        public Sequencer()
        {
            //ViewDataList = new System.Collections.Generic.List<ViewData>();
            VersionDataList = new List<VersionData>();
            //+
            _activeViewName = StateTracker.Get(StateEntryType.Value, "__$Sequence$ViewDataName");
            ActiveVersionName = StateTracker.Get(StateEntryType.Value, "__$Sequence$VersionDataName");
            //+
            //if (!String.IsNullOrEmpty(_activeViewName))
            //{
            //    _activeViewData = new ViewData
            //    {
            //        Name = _activeViewName
            //    };
            //}
        }

        //private Map<String, System.Web.UI.Control> _actionMap = new Map<string, System.Web.UI.Control>();
        //private Map<String, System.Web.UI.Control> _dataMap = new Map<string, System.Web.UI.Control>();

        //+
        ////- #DataMap -//
        //internal Map<String, System.Web.UI.Control> DataControlRegistry
        //{
        //    get { return _dataMap; }
        //    set { _dataMap = value; }
        //}

        ////- #ActionList -//
        //internal Map<String, System.Web.UI.Control> ActionControlRegistry
        //{
        //    get { return _actionMap; }
        //    set { _actionMap = value; }
        //}

        //- @ActiveView -//
        /// <summary>
        /// Current version of a view being displayed.
        /// </summary>
        public String ActiveViewName
        {
            get
            {
                return _activeViewName;
            }
        }

        //- ~SequenceType -//
        internal SequenceType SequenceType
        {
            get
            {
                lock (_lock)
                {
                    return ValidatedSequenceMap[SequenceName];
                }
            }
        }

        ////- @ViewDataList -//
        ///// <summary>
        ///// View of registered views.
        ///// </summary>
        //public System.Collections.Generic.List<ViewData> ViewDataList { get; set; }

        //- @VersionDataList -//
        /// <summary>
        /// View of registered view version
        /// </summary>
        public List<VersionData> VersionDataList { get; set; }

        //- ~CurrentView -//
        internal View CurrentView { get; set; }

        //- @LoadedViaConfiguration -//
        /// <summary>
        /// True is sequence views and versions were loaded via configuration instead of declarative code.
        /// </summary>
        public Boolean LoadedViaConfiguration { get; set; }

        //- ~EnableGoogleAnalytics -//
        internal Boolean EnableGoogleAnalytics { get; set; }

        //- ~GoogleAnalyticsTrackingCode -//
        internal String GoogleAnalyticsTrackingCode { get; set; }

        //- @DisableViewAnalytics -//
        /// <summary>
        /// True if per-view Google analytics is disabled.  Per-sequence will remain be enabled.
        /// </summary>
        public Boolean DisableViewAnalytics { get; set; }

        //- @ExplicitVersion -//
        /// <summary>
        /// The name of the version which to always choose.
        /// </summary>
        public String ExplicitVersion { get; set; }

        //- @StateContainerID -//
        /// <summary>
        /// Name of ASP.NET object which should house Nalarium state.  This allows control over where the state should be.
        /// </summary>
        public String StateContainerID { get; set; }

        //- @ActiveVersion -//
        /// <summary>
        /// Current version of a view's version being displayed.  Blank if no specific version set.
        /// </summary>
        public String ActiveVersionName { get; set; }

        //- @SequenceName -//
        /// <summary>
        /// The name of the sequence.
        /// </summary>
        public String SequenceName
        {
            get
            {
                if (String.IsNullOrEmpty(_sequenceName))
                {
                    throw new InvalidOperationException(String.Format(Resource.General_Required, "SequenceName"));
                }
                //+
                return _sequenceName;
            }

            set
            {
                _sequenceName = value;
            }
        }

        //+
        //- @Ctor -//

        //+
        //- #AddParsedSubObject -//
        protected override void AddParsedSubObject(Object obj)
        {
            //ViewData view = obj as ViewData;
            //if (view != null)
            //{
            //    ViewDataList.Add(view);
            //    return;
            //}
            var version = obj as VersionData;
            if (version != null)
            {
                VersionDataList.Add(version);
            }
        }

        //+
        //- #OnInit -//
        protected override void OnInit(EventArgs e)
        {
            var page = Page as Page;
            //+ load all
            if (VersionDataList.Count == 0)
            {
                //ViewDataList = page.SequenceData.ViewList.Clone();
                VersionDataList = page.SequenceData.VersionList.Clone();
                //+
                LoadedViaConfiguration = true;
            }
            //+ validate
            //ValidateViewData();
            //+ init view selection
            if (!Page.IsPostBack)
            {
                if (page != null)
                {
                    //_activeViewData = ViewDataList.SingleOrDefault(p => p.Name == page.SelectInitView());
                    _activeViewName = page.SelectInitView();
                }
            }
            //else
            //{
            //    ProcessCommand();
            //}
            if (String.IsNullOrEmpty(_activeViewName))
            {
            }
            ValidateActiveView();
            ////+ find control used
            //if (String.IsNullOrEmpty(_activeViewData.ViewUsed))
            //{
            //    ViewData vd = ViewDataList.SingleOrDefault(p => p.Name == _activeViewData.Name);
            //    if (vd != null)
            //    {
            //        _activeViewData.ViewUsed = vd.ViewUsed;
            //    }
            //}
            //+ version
            if (VersionDataList.Count > 0)
            {
                _activeVersionName = (Page as Page).SetVersion();
                if (!String.IsNullOrEmpty(_activeVersionName))
                {
                    ActiveVersionName = _activeVersionName;
                    StateTracker.Set(StateEntryType.Value, "__$Sequence$VersionDataName", _activeVersionName);
                }
            }
            //+
            //if (_ensureViewConsistency)
            //{
            //    EnsureViewConsistency();
            //}
            //+ copy
            if (page.EnableGoogleAnalytics)
            {
                EnableGoogleAnalytics = page.EnableGoogleAnalytics;
            }
            //+ add control
            ShowControl();
            //+
            if (!DisableViewAnalytics)
            {
                Controls.Add(new ViewAnalytics
                             {
                                 Sequencer = this
                             });
            }
            //+
            base.OnInit(e);
        }

        protected virtual void ValidateActiveView()
        {
        }

        //protected virtual void ValidateViewData()
        //{
        //    if (ViewDataList.Count == 0)
        //    {
        //        throw new InvalidOperationException(Resource.Sequence_NoViewsRegistered);
        //    }
        //}

        //- @SetView -//
        public void SetView(String viewName)
        {
            ShowControl(viewName);
        }

        //- $ShowControl -//
        private void ShowControl(String viewName)
        {
            _activeViewName = viewName;
            //+
            ShowControl();
        }

        //- $ShowControl -//
        private void ShowControl()
        {
            //+ remove all
            var toRemove = new List<Control>();
            foreach (Control c in Controls)
            {
                if (c is View)
                {
                    toRemove.Add(c);
                }
            }
            while (toRemove.Count > 0)
            {
                Control c = toRemove[0];
                Controls.Remove(c);
                toRemove.Remove(c);
            }
            //+ add
            if (String.IsNullOrEmpty(_activeViewName))
            {
                return;
            }
            //if (viewData == null)
            //{
            //    return;
            //}
            LoadView(_activeViewName, ActiveVersionName);
            //+ update state
            StateTracker.Set(StateEntryType.Value, "__$Sequence$ViewDataName", _activeViewName);
        }

        //- $LoadView -//
        protected virtual View LoadView(String viewName, String version)
        {
            String path;
            if (String.IsNullOrEmpty(version))
            {
                path = String.Format(CultureInfo.InvariantCulture, @"{0}/{1}.ascx", SequenceName, viewName);
            }
            else
            {
                path = String.Format(CultureInfo.InvariantCulture, @"{0}/{1}/{2}.ascx", SequenceName, viewName, ActiveVersionName);
                if (!File.Exists(Http.Server.MapPath(Path.Combine(ProcessingSection.GetConfigSection().Sequences.RootPath, path))))
                {
                    path = String.Format(CultureInfo.InvariantCulture, @"{0}/{1}.ascx", SequenceName, viewName);
                }
            }
            View control = null;
            try
            {
                control = Page.LoadControl(path) as View;
                if (control == null)
                {
                    throw new InvalidOperationException(Resource.Sequence_InvalidView);
                }
                control.ID = viewName;
                control.Sequencer = this;
                //control.ViewUsed = String.IsNullOrEmpty(viewUsed) ? name : viewUsed;
                control.Visible = true;
                try
                {
                    Page.RegisterRequiresControlState(control);
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                if (WebProcessingReportController.Reporter.Initialized)
                {
                    var map = new Map();
                    map.Add("Section", "Sequence");
                    map.Add("Message", ex.Message);
                    map.Add("Path", path);
                    map.Add("Exception Type", ex.GetType().FullName);
                    //+
                    WebProcessingReportController.Reporter.AddMap(map);
                }
                //+
                throw;
            }
            //+
            CurrentView = control;
            Controls.Add(CurrentView);
            //+
            return CurrentView;
        }

        #region Nested type: Info

        public static class Info
        {
            public const String Scope = "__$Nalarium$Sequence";
            //+
            public const String ActiveVersion = "ActiveVersion";
            public const String PageTitle = "PageTitle";
            //+
            public const String PublicScope = "sequence";
            //+
            //public const String FirstMessage = PublicScope + "::first";
            //public const String LastMessage = PublicScope + "::last";
            //public const String NextMessage = PublicScope + "::next";
            //public const String PreviousMessage = PublicScope + "::previous";
            //public const String JumpMessage = PublicScope + "::jump";
        }

        #endregion
    }
}