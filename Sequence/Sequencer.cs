#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Linq;
//+
using Nalarium.Web.Processing.Data;
//+
namespace Nalarium.Web.Processing.Sequence
{
    /// <summary>
    /// Holds all sequence views and versions.
    /// </summary>
    [System.Web.UI.ParseChildren(false)]
    [System.Web.UI.ControlBuilder(typeof(ViewVersionControlBuilder))]
    public class Sequencer : System.Web.UI.Control
    {
        //- @Info -//
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

        //+
        private static Object _lock = new Object();
        private static Map<String, SequenceType> ValidatedSequenceMap = new Map<String, SequenceType>();

        //+ field
        //protected ViewData _activeViewData;
        protected String _activeVersionName;
        protected String _activeViewName;
        protected String _sequenceName;
        protected Boolean _ensureViewConsistency = true;
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
        public System.Collections.Generic.List<VersionData> VersionDataList { get; set; }

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
        public Sequencer()
        {
            //ViewDataList = new System.Collections.Generic.List<ViewData>();
            VersionDataList = new System.Collections.Generic.List<VersionData>();
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
            VersionData version = obj as VersionData;
            if (version != null)
            {
                VersionDataList.Add(version);
            }
        }

        //+
        //- #OnInit -//
        protected override void OnInit(EventArgs e)
        {
            Page page = Page as Page;
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
                Controls.Add(new ViewAnalytics() { Sequencer = this });
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

        //protected virtual void ProcessCommand()
        //{
        //    //+ process message
        //    if (!String.IsNullOrEmpty(StateTracker.PostedMessage))
        //    {
        //        Boolean isNext = false;
        //        switch (StateTracker.PostedMessage.ToLower())
        //        {
        //            case Info.FirstMessage:
        //                _activeViewData = ViewDataList.FirstOrDefault();
        //                break;
        //            case Info.LastMessage:
        //                _activeViewData = ViewDataList.LastOrDefault();
        //                break;
        //            case Info.NextMessage:
        //                foreach (ViewData vd in ViewDataList)
        //                {
        //                    if (isNext)
        //                    {
        //                        _activeViewData = vd;
        //                        break;
        //                    }
        //                    if (vd.Name == _activeViewName)
        //                    {
        //                        isNext = true;
        //                    }
        //                }
        //                break;
        //            case Info.PreviousMessage:
        //                for (int i = ViewDataList.Count; i >= 0; i--)
        //                {
        //                    if (isNext)
        //                    {
        //                        _activeViewData = ViewDataList[i];
        //                        break;
        //                    }
        //                    if (ViewDataList[i].Name == _activeViewName)
        //                    {
        //                        isNext = true;
        //                    }
        //                }
        //                break;
        //            case Info.JumpMessage:
        //                if (!String.IsNullOrEmpty(StateTracker.GetPostedMessageParameter(Position.First)))
        //                {
        //                    ViewData tmp = ViewDataList.SingleOrDefault(p => p.Name == StateTracker.GetPostedMessageParameter(Position.First));
        //                    if (tmp != null)
        //                    {
        //                        _activeViewData = tmp;
        //                    }
        //                }
        //                break;
        //        }
        //    }
        //}

        ////- @MoveToPreviousView -//
        ///// <summary>
        ///// Sets the currently active view to the one just prior to the current one.
        ///// </summary>
        //public void MoveToPreviousView()
        //{
        //    if (String.IsNullOrEmpty(_activeViewName))
        //    {
        //        return;
        //    }
        //    Boolean isNext = false;
        //    for (int i = ViewDataList.Count; i >= 0; i--)
        //    {
        //        if (isNext)
        //        {
        //            _activeViewData = ViewDataList[i];
        //            break;
        //        }
        //        if (ViewDataList[i].Name == _activeViewName)
        //        {
        //            isNext = true;
        //        }
        //    }
        //    ShowControl(_activeViewData);
        //}

        ////- @MoveToNextView -//
        ///// <summary>
        ///// Sets the currently active view to the one just after to the current one.
        ///// </summary>
        //public void MoveToNextView()
        //{
        //    if (String.IsNullOrEmpty(_activeViewName))
        //    {
        //        return;
        //    }
        //    Boolean isNext = false;
        //    foreach (ViewData vd in ViewDataList)
        //    {
        //        if (isNext)
        //        {
        //            _activeViewData = vd;
        //            break;
        //        }
        //        if (vd.Name == _activeViewName)
        //        {
        //            isNext = true;
        //        }
        //    }
        //    ShowControl(_activeViewData);
        //}

        ////- @MoveToFirstView -//
        ///// <summary>
        ///// Sets the currently active view to the first view available.
        ///// </summary>
        //public void MoveToFirstView()
        //{
        //    ShowControl(ViewDataList[0]);
        //}

        ////- @MoveToLastView -//
        ///// <summary>
        ///// Sets the currently active view to the last view available.
        ///// </summary>
        //public void MoveToLastView()
        //{
        //    ShowControl(ViewDataList[ViewDataList.Count]);
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
            System.Collections.Generic.List<System.Web.UI.Control> toRemove = new System.Collections.Generic.List<System.Web.UI.Control>();
            foreach (System.Web.UI.Control c in Controls)
            {
                if (c is View)
                {
                    toRemove.Add(c);
                }
            }
            while (toRemove.Count > 0)
            {
                System.Web.UI.Control c = toRemove[0];
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

        ////- $EnsureViewConsistency -//
        //private void EnsureViewConsistency()
        //{
        //    lock (_lock)
        //    {
        //        if (!ValidatedSequenceMap.ContainsKey(SequenceName))
        //        {
        //            SequenceType type = SequenceType.Normal;
        //            Boolean first = true;
        //            foreach (VersionData versionData in VersionDataList)
        //            {
        //                foreach (ViewData viewData in ViewDataList)
        //                {
        //                    if (!String.IsNullOrEmpty(viewData.ViewUsed) && !viewData.Name.Equals(viewData.ViewUsed))
        //                    {
        //                        //+ when ViewUsed is set, the control won't be used.  so, don't validate it.
        //                        continue;
        //                    }
        //                    View view = LoadView(viewData.Name, viewData.ViewUsed, versionData.Name);
        //                    if (view is Mvp.IIsMvp)
        //                    {
        //                        if (first)
        //                        {
        //                            type = SequenceType.Mvp;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (first)
        //                        {
        //                            type = SequenceType.Normal;
        //                        }
        //                    }
        //                    first = false;
        //                }
        //            }
        //            //+
        //            ValidatedSequenceMap[SequenceName] = type;
        //        }
        //    }
        //}

        //- $LoadView -//
        protected virtual View LoadView(String viewName, String version)
        {
            String path;
            if (String.IsNullOrEmpty(version))
            {
                path = String.Format(System.Globalization.CultureInfo.InvariantCulture, @"{0}/{1}.ascx", SequenceName, viewName);
            }
            else
            {
                path = String.Format(System.Globalization.CultureInfo.InvariantCulture, @"{0}/{1}/{2}.ascx", SequenceName, viewName, ActiveVersionName);
                if (!System.IO.File.Exists(Http.Server.MapPath(System.IO.Path.Combine(Nalarium.Web.Processing.Configuration.ProcessingSection.GetConfigSection().Sequences.RootPath, path))))
                {
                    path = String.Format(System.Globalization.CultureInfo.InvariantCulture, @"{0}/{1}.ascx", SequenceName, viewName);
                }
            }
            View control = null;
            try
            {
                control = this.Page.LoadControl(path) as View;
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
                    this.Page.RegisterRequiresControlState(control);
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                if (Nalarium.Web.Processing.WebProcessingReportController.Reporter.Initialized)
                {
                    Map map = new Map();
                    map.Add("Section", "Sequence");
                    map.Add("Message", ex.Message);
                    map.Add("Path", path);
                    map.Add("Exception Type", ex.GetType().FullName);
                    //+
                    Nalarium.Web.Processing.WebProcessingReportController.Reporter.AddMap(map);
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
    }
}