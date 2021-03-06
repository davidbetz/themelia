#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright � Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Nalarium.Web.Controls;
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
                var sequencer = ControlFinder.FindControlRecursivelyByType<Sequencer>(this, true);
                if (sequencer == null)
                {
                    throw new InvalidOperationException(Resource.Sequence_SequencerNotFound);
                }
                //+
                return sequencer;
            }
        }

        //- @SelectInitView -//

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

        /// <summary>
        /// Used to choose the initial sequence view.
        /// </summary>
        /// <returns>Name of initial view.</returns>
        public abstract String SelectInitView();

        //- @SelectVersion -//
        /// <summary>
        /// Used to choose the view version.
        /// </summary>
        /// <returns>Name of version or null if random.</returns>
        public virtual String SelectVersion()
        {
            return null;
        }

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
            //+ analytics
            if (EnableGoogleAnalytics && !String.IsNullOrEmpty(GoogleAnalyticsTrackingCode) && Sequencer != null)
            {
                if (ControlFinder.FindControlRecursivelyByType<GoogleAnalytics>(Page) == null)
                {
                    Sequencer.Controls.Add(new GoogleAnalytics(GoogleAnalyticsTrackingCode));
                }
            }
            //+
            base.OnInitComplete(e);
        }

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
                Title = title;
            }
            //+
            base.OnPreRenderComplete(e);
        }
    }
}