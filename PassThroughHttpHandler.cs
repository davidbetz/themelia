#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing
{
    public class PassThroughHttpHandler : ReusableSessionHttpHandler
    {
        //- @ForceUse -//
        /// <summary>
        /// When true, passthrough is used.  Can only be set to true.  Setting to false has no effect.
        /// </summary>
        public static Boolean ForceUse
        {
            get
            {
                return HttpData.GetScopedItem<Boolean>(RouteActivator.Info.Scope, RouteActivator.Info.PassThroughForceUse);
            }
            set
            {
                //++ only allow setting to true
                if (value)
                {
                    HttpData.SetScopedItem<Boolean>(RouteActivator.Info.Scope, RouteActivator.Info.PassThroughForceUse, true);
                }
            }
        }

        //+
        //- @Ctor -//
        public PassThroughHttpHandler()
        {
        }
        public PassThroughHttpHandler(Boolean force)
        {
            ForceUse = force;
        }

        //+
        //- @ProcessRequest -//
        public override void Process()
        {
            //+ blank
        }
    }
}