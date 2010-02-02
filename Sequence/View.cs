#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing.Sequence
{
    /// <summary>
    /// Base class to create a sequence view.
    /// </summary>
    public class View : System.Web.UI.UserControl
    {
        private Boolean _isDataBound;

        //+
        ////- ~Presenter -//
        //internal Nalarium.Mvp.Presenter PresenterPlain { get; set; }

        //- @AutoDataBind -//
        /// <summary>
        /// When true, automatically occurs when.  By default, data binding only occurs when IsPostBack is false.databinding occurs even when IsPostBack is true.
        /// </summary>
        public Boolean AutoDataBind { get; set; }

        //- @PageTitle -//
        /// <summary>
        /// Represents the current page title.  The actual page title is set in the PreRender step.
        /// </summary>
        public String PageTitle { get; set; }

        //- @Sequencer -//
        /// <summary>
        /// Represents the sequencer which holds the sequence views.
        /// </summary>
        public Sequencer Sequencer { get; set; }

        ////- ~ViewUsed -//
        ///// <summary>
        ///// Name of the view actually used
        ///// </summary>
        //internal String ViewUsed { get; set; }

        public StrongBag ViewData { get; set; }

        //+
        //- @SequenceView -//
        public View()
        {
            _isDataBound = false;
        }

        //+
        //- #SaveControlState -//
        protected override object SaveControlState()
        {
            return _isDataBound;
        }

        //- #LoadControlState -//
        protected override void LoadControlState(Object savedState)
        {
            _isDataBound = (Boolean)savedState;
        }

        //- #OnInit -//
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(OnViewLoad);
            //+
            base.OnInit(e);
        }

        //- #OnViewLoad -//
        protected void OnViewLoad(Object sender, EventArgs e)
        {
            if (!_isDataBound && AutoDataBind)
            {
                _isDataBound = true;
                this.DataBind();
            }
        }

        ////- @MovePrevious -//
        ///// <summary>
        ///// Sets the currently active view to the one just prior to the current one.
        ///// </summary>
        //public void MoveToPreviousView()
        //{
        //    if (Sequencer != null)
        //    {
        //        Sequencer.MoveToPreviousView();
        //    }
        //}

        ////- @MoveNext -//
        ///// <summary>
        ///// Sets the currently active view to the one just after to the current one.
        ///// </summary>
        //public void MoveToNextView()
        //{
        //    if (Sequencer != null)
        //    {
        //        Sequencer.MoveToNextView();
        //    }
        //}

        ////- @MoveToFirstView -//
        ///// <summary>
        ///// Sets the currently active view to the first view available.
        ///// </summary>
        //public void MoveToFirstView()
        //{
        //    if (Sequencer != null)
        //    {
        //        Sequencer.MoveToFirstView();
        //    }
        //}

        ////- @MoveToLastView -//
        ///// <summary>
        ///// Sets the currently active view to the last view available.
        ///// </summary>
        //public void MoveToLastView()
        //{
        //    if (Sequencer != null)
        //    {
        //        Sequencer.MoveToLastView();
        //    }
        //}

        //- @SetView -//
        /// <summary>
        /// Sets the currently active view to a specific view.
        /// </summary>
        public void SetView(String viewName)
        {
            if (Sequencer != null)
            {
                Sequencer.SetView(viewName);
            }
        }
    }
}