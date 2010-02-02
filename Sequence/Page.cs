#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
//+
using Nalarium.Web.Processing.Data;
//+
namespace Nalarium.Web.Processing.Sequence
{
    /// <summary>
    /// This is the base class for all sequence pages.
    /// </summary>
    public abstract class Page : System.Web.UI.Page
    {
        //- @Sequencer -//
        /// <summary>
        /// Represents the sequencer which holds the sequence views.
        /// </summary>
        public Sequencer Sequencer
        {
            get
            {
                Sequencer sequencer = ControlFinder.FindControlRecursivelyByType<Sequencer>(this, true);
                if (sequencer == null)
                {
                    throw new InvalidOperationException(Resource.Sequence_SequencerNotFound);
                }
                //+
                return sequencer;
            }
        }

        //- @SelectInitView -//
        /// <summary>
        /// Used to choose the initial sequence view.
        /// </summary>
        /// <returns>Name of initial view.</returns>
        abstract public String SelectInitView();

        //- @SelectVersion -//
        /// <summary>
        /// Used to choose the view version.
        /// </summary>
        /// <returns>Name of version or null if random.</returns>
        public virtual String SelectVersion()
        {
            return null;
        }

        //- @SequenceData -//
        /// <summary>
        /// Sequence data pulled from configuration.
        /// </summary>
        public SequenceData SequenceData
        {
            get
            {
                return SequenceDataList.AllSequenceData[Sequencer.SequenceName] ?? SequenceData.CreateBlank();
            }
        }

        //- @EnableGoogleAnalytics -//
        /// <summary>
        /// True to enable Google Analytics.  Requires that GoogleAnalyticsTrackingCode contain the analytics tracking code.
        /// </summary>
        public Boolean EnableGoogleAnalytics { get; set; }

        //- @GoogleAnalyticsTrackingCode -//
        /// <summary>
        /// Analytics tracking code including the prefix (i.e. UA-).
        /// </summary>
        public String GoogleAnalyticsTrackingCode { get; set; }

        //- @SelectVersion -//
        /// <summary>
        /// Used to choose the view version.
        /// </summary>
        /// <returns>Name of version or null or blank if random.</returns>
        public virtual String SelectVersionComplete()
        {
            return String.Empty;
        }

        //+
        //- #OnInitComplete -//
        protected override void OnInitComplete(EventArgs e)
        {
            //if (CoreModule.IsMvpAvailable)
            //{
            //    SetupMvp();
            //}
            //+ analytics
            if (EnableGoogleAnalytics && !String.IsNullOrEmpty(GoogleAnalyticsTrackingCode) && Sequencer != null)
            {
                if (ControlFinder.FindControlRecursivelyByType<Nalarium.Web.Controls.GoogleAnalytics>(Page) == null)
                {
                    Sequencer.Controls.Add(new Nalarium.Web.Controls.GoogleAnalytics(GoogleAnalyticsTrackingCode));
                }
            }
            ////+ state
            //if (ControlFinder.FindControlRecursivelyByType<Nalarium.Web.Controls.StateArea>(this) == null)
            //{
            //    System.Web.UI.Control stateContainer = null;
            //    if (!String.IsNullOrEmpty(Sequencer.StateContainerID))
            //    {
            //        stateContainer = ControlFinder.FindControlRecursively(this, Sequencer.StateContainerID);
            //        if (stateContainer != null)
            //        {
            //            stateContainer.Controls.Add(new Nalarium.Web.Controls.StateArea());
            //        }
            //    }
            //    if (stateContainer == null)
            //    {
            //        Sequencer.Controls.Add(new Nalarium.Web.Controls.StateArea());
            //    }
            //}
            //+
            base.OnInitComplete(e);
        }

        ////- $SetupMvp -//
        //private void SetupMvp()
        //{
        //    if (Sequencer == null ||
        //       Sequencer.CurrentView == null)
        //    {
        //        return;
        //    }
        //    if (!(Sequencer.CurrentView is Nalarium.Mvp.IView) || !(Sequencer.CurrentView is Mvp.IIsMvp))
        //    {
        //        Sequencer.CurrentView.PresenterPlain = new Nalarium.Mvp.DummyPresenter();
        //        return;
        //    }
        //    if (String.IsNullOrEmpty(Sequencer.SequenceName) ||
        //       String.IsNullOrEmpty(Sequencer.ActiveViewName))
        //    {
        //        return;
        //    }
        //    View view = Sequencer.CurrentView;
        //    Type baseType = view.GetType();
        //    Boolean done = false;
        //    while (!done)
        //    {
        //        baseType = baseType.BaseType;
        //        if (baseType == null || baseType.FullName.StartsWith("Nalarium.Web.Sequence.Mvp.View`1", StringComparison.InvariantCulture))
        //        {
        //            done = true;
        //        }
        //    }
        //    Type presenterType = null;
        //    if (baseType != null)
        //    {
        //        Type[] typeArray = baseType.GetGenericArguments();
        //        if (typeArray != null)
        //        {
        //            presenterType = typeArray[0];
        //        }
        //    }
        //    view.PresenterPlain = Nalarium.Activation.ObjectCreator.Create(presenterType) as Nalarium.Mvp.Presenter;
        //    view.PresenterPlain.SetView(Sequencer.CurrentView as Nalarium.Mvp.IView, Sequencer.ActiveVersionName);
        //}

        //- ~SetVersion -//
        internal String SetVersion()
        {
            VersionData versionData;
            String versionName = String.Empty;
            //+ explicit
            if (!String.IsNullOrEmpty(Sequencer.ExplicitVersion))
            {
                versionData = Sequencer.VersionDataList.FirstOrDefault(p => p.Name.Equals(Sequencer.ExplicitVersion, StringComparison.CurrentCultureIgnoreCase));
                if (versionData != null)
                {
                    versionName = Sequencer.ExplicitVersion;
                }
            }
            //+ manual
            String version = SelectVersion();
            versionData = Sequencer.VersionDataList.FirstOrDefault(p => p.Name.Equals(version, StringComparison.CurrentCultureIgnoreCase));
            if (versionData != null)
            {
                versionName = version;
            }
            //+ fallback
            versionData = Sequencer.VersionDataList.FirstOrDefault(p => p.Name.Equals(Sequencer.ActiveVersionName, StringComparison.CurrentCultureIgnoreCase));
            if (versionData != null)
            {
                versionName = versionData.Name;
            }
            else if (Sequencer.VersionDataList.Count > 0)
            {
                //++ when version is invalid, set random
                versionName = ObtainRandomVersion(Sequencer.VersionDataList);
            }
            //+
            if (!String.IsNullOrEmpty(versionName))
            {
                SelectVersionComplete();
            }
            //+
            return versionName;
        }

        //- $ObtainRandomVersion -//
        private String ObtainRandomVersion(List<VersionData> versionDataList)
        {
            if (Collection.IsNullOrEmpty(versionDataList))
            {
                return String.Empty;
            }
            Int32 count = versionDataList.Count;
            Int32 version = new Random((Int32)DateTime.Now.Ticks).Next(count);
            //+
            return versionDataList[version].Name;
        }

        //- #OnPreRenderComplete -//
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (Sequencer.CurrentView == null)
            {
                return;
            }
            String title = Sequencer.CurrentView.PageTitle;
            if (!String.IsNullOrEmpty(title))
            {
                this.Title = title;
            }
            //+
            base.OnPreRenderComplete(e);
        }
    }
}