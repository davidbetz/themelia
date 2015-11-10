#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web.UI;
using Nalarium.Web.Controls;

namespace Nalarium.Web.Processing.Sequence
{
    internal class ViewAnalytics : Control
    {
        //- @Sequencer -//
        internal Sequencer Sequencer { get; set; }

        //+
        protected override void OnPreRender(EventArgs e)
        {
            var analyticsControl = ControlFinder.FindControlRecursivelyByType<GoogleAnalytics>(Page);
            if (analyticsControl == null)
            {
                return;
            }
            if (Sequencer == null)
            {
                return;
            }
            String path = String.Empty;
            String view = Sequencer.ActiveViewName;
            String version = Sequencer.ActiveVersionName;
            path = UrlCleaner.CleanWebPathTail(Http.AbsolutePath) + "/" + UrlCleaner.CleanWebPath(view);
            if (!String.IsNullOrEmpty(version))
            {
                path += "/" + UrlCleaner.CleanWebPath(version);
            }
            if (String.IsNullOrEmpty(path))
            {
                return;
            }
            analyticsControl.Path = path;
            //+
            base.OnPreRender(e);
        }
    }
}