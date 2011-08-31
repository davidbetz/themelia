#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Web;

namespace Nalarium.Web.Processing
{
    public static class FlowControl
    {
        //- ~Info -//

        //- @IsPassThrough -//
        public static Boolean IsPassThrough
        {
            get
            {
                return ActiveHandler is PassThroughHttpHandler;
            }
        }

        //- ~ActiveHandler -//
        internal static IHttpHandler ActiveHandler
        {
            get
            {
                return HttpData.GetScopedItem<IHttpHandler>(RouteActivator.Info.Scope, Info.ActiveHandler);
            }
            set
            {
                HttpData.SetScopedItem(RouteActivator.Info.Scope, Info.ActiveHandler, value);
            }
        }

        //- ~OverrideProcessingSkipped -//
        internal static Boolean OverrideProcessingSkipped
        {
            get
            {
                return HttpData.GetScopedItem<Boolean>(RouteActivator.Info.Scope, Info.OverrideProcessingSkipped);
            }

            set
            {
                HttpData.SetScopedItem(RouteActivator.Info.Scope, Info.OverrideProcessingSkipped, value);
            }
        }

        //- ~StoppingAfterInitProcessing -//
        /// <summary>
        /// Used for service and file endpoints.
        /// </summary>
        internal static Boolean StoppingAfterInitProcessing
        {
            get
            {
                return HttpData.GetScopedItem<Boolean>(RouteActivator.Info.Scope, Info.StoppingAfterInitProcessing);
            }

            set
            {
                HttpData.SetScopedItem(RouteActivator.Info.Scope, Info.StoppingAfterInitProcessing, value);
            }
        }

        //- ~IsExclusion -//
        internal static Boolean IsHalted
        {
            get
            {
                return HttpData.GetScopedItem<Boolean>(RouteActivator.Info.Scope, Info.IsHalted);
            }

            set
            {
                if (true)
                {
                    HttpData.SetScopedItem(RouteActivator.Info.Scope, Info.IsHalted, value);
                }
            }
        }

        /// <summary>
        /// Tells the processing mechanism to stop after init-processing.
        /// </summary>
        public static void StopAfterInitProcessing()
        {
            StoppingAfterInitProcessing = true;
        }

        //- @SkipOverrideProcessing -//
        /// <summary>
        /// Tells the processing mechanism to skip post processing; this is a one-way operation.
        /// </summary>
        public static void SkipOverrideProcessing()
        {
            OverrideProcessingSkipped = true;
        }

        //- ~SetAsExclusion -//
        internal static void EndProcessing()
        {
            IsHalted = true;
        }

        #region Nested type: Info

        internal static class Info
        {
            internal const String ActiveHandler = "__$ActiveHandler";
            internal const String OverrideProcessingSkipped = "__$OverrideProcessingSkipped";
            internal const String StoppingAfterInitProcessing = "__$StoppingAfterInitProcessing";
            internal const String IsHalted = "__$IsProcessingEnded";
        }

        #endregion
    }
}