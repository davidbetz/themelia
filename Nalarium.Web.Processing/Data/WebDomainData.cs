#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Diagnostics;
using System.Linq;

namespace Nalarium.Web.Processing.Data
{
    [DebuggerDisplay("{Name}, {Path}, {AcceptMissingTrailingSlash}, BasedOn: {BasedOn}, IsBasedOn: {IsBasedOn}")]
    public class WebDomainData
    {
        //- @Name -//
        public String Name { get; set; }

        //- @Subdomain -//
        public String Subdomain { get; internal set; }

        //- @Path -//
        public String Path { get; internal set; }

        //- @CustomParameter -//
        public String CustomParameter { get; internal set; }

        //- @DefaultType -//
        public DefaultType DefaultType { get; internal set; }

        //- @DefaultParameter -//
        public String DefaultParameter { get; internal set; }

        //- @AccessRuleGroup -//
        public String AccessRuleGroup { get; set; }

        ////- @DefaultPage -//
        //public String Default { get; internal set; }

        ////- @DefaultEndpoint -//
        //public String DefaultEndpoint { get; internal set; }

        //- @BasedOn -//
        public String BasedOn { get; internal set; }

        //- @IsBasedOn -//
        public Boolean IsBasedOn { get; internal set; }

        //- @IsAbstract -//
        public Boolean IsAbstract
        {
            get
            {
                return Name.StartsWith("{", StringComparison.InvariantCultureIgnoreCase) && Name.EndsWith("}", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        //- @IsSealed -//
        public Boolean IsSealed { get; internal set; }

        //- @ParameterDataList -//
        public ParameterDataList ParameterDataList { get; internal set; }

        //- @HasOverriddenComponent -//
        public Boolean HasOverriddenComponent { get; internal set; }

        //- @AcceptMissingTrailingSlash -//
        public Boolean AcceptMissingTrailingSlash { get; internal set; }

        //- ~FoundWithoutTrailingSlash -//
        internal Boolean FoundWithoutTrailingSlash { get; set; }

        //- @CatchAllMode -//
        public CatchAllMode CatchAllMode { get; internal set; }

        //- @CatchAllInitParameter -//
        public String CatchAllInitParameter { get; internal set; }

        //- ~ComponentDataList -//
        public ComponentDataList ComponentDataList { get; set; }

        //- ~ErrorProcessorDataList -//
        public ErrorProcessorDataList ErrorProcessorDataList { get; set; }

        //- ~ProcessorDataList -//
        public ProcessorDataList ProcessorDataList { get; set; }

        //- ~FactoryDataList -//
        public FactoryDataList FactoryDataList { get; set; }

        //- ~ProcessorFactoryDataList -//
        public ProcessorFactoryDataList ProcessorFactoryDataList { get; set; }

        //- ~ObjectFactoryDataList -//
        public ObjectFactoryDataList ObjectFactoryDataList { get; set; }

        //- ~HandlerFactoryDataList -//
        public EndpointFactoryDataList HandlerFactoryDataList { get; set; }

        //- ~EndpointDataList -//
        public EndpointDataList EndpointDataList { get; set; }

        //- ~CatchAllEndpoint -//
        public EndpointData CatchAllEndpoint { get; set; }

        //- ~InitProcessorList -//
        public InitProcessorDataList InitProcessorDataList { get; set; }

        //- ~SelectionProcessorList -//
        public SelectionProcessorDataList SelectionProcessorDataList { get; set; }

        //- ~OverrideProcessorList -//
        public OverrideProcessorDataList OverrideProcessorDataList { get; set; }

        //- ~StateProcessorList -//
        public StateProcessorDataList StateProcessorDataList { get; set; }

        //- ~PostRenderProcessorDataList -//
        public PostRenderProcessorDataList PostRenderProcessorDataList { get; set; }

        ////- ~AccessRuleList -//
        //public AccessRuleDataList AccessRuleDataList { get; set; }

        ////- ~AliasDataList -//
        //public AliasDataList AliasDataList { get; set; }

        ////- ~RedirectDataList -//
        //public RedirectDataList RedirectDataList { get; set; }

        //- ~SecurityData -//
        public SecurityData SecurityData { get; set; }

        //- @Clone -//
        public WebDomainData Clone()
        {
            var data = new WebDomainData();
            //data.Default = this.Default;
            data.DefaultType = DefaultType;
            data.ParameterDataList = ParameterDataList;
            data.DefaultParameter = DefaultParameter;
            data.AcceptMissingTrailingSlash = AcceptMissingTrailingSlash;
            data.Subdomain = Subdomain;
            data.IsSealed = IsSealed;
            data.CatchAllMode = CatchAllMode;
            data.CatchAllInitParameter = CatchAllInitParameter;
            data.InitProcessorDataList = InitProcessorDataList.Clone();
            data.PostRenderProcessorDataList = PostRenderProcessorDataList.Clone();
            data.SecurityData = SecurityData.Clone();
            data.ComponentDataList = ComponentDataList.Clone();
            data.CatchAllEndpoint = CatchAllEndpoint.Clone();
            data.EndpointDataList = EndpointDataList.Clone();
            data.HandlerFactoryDataList = HandlerFactoryDataList.Clone();
            data.SelectionProcessorDataList = SelectionProcessorDataList.Clone();
            data.OverrideProcessorDataList = OverrideProcessorDataList.Clone();
            data.StateProcessorDataList = StateProcessorDataList.Clone();
            //data.AccessRuleDataList = this.AccessRuleDataList.Clone();
            data.ProcessorFactoryDataList = ProcessorFactoryDataList.Clone();
            //+
            return data;
        }

        //- $SortPriority -//
        private static Int32 SortPriority(IHasPriority p1, IHasPriority p2)
        {
            if (p2.Priority > p1.Priority)
            {
                return -1;
            }
            else if (p1.Priority > p2.Priority)
            {
                return 1;
            }
            //+
            return 0;
        }

        //- @SortAllDynamic -//
        public void SortAllDynamic()
        {
            if (InitProcessorDataList.Count != InitProcessorDataList.OriginalCount)
            {
                InitProcessorDataList = new InitProcessorDataList(InitProcessorDataList.OrderBy(p => p.Priority).ToList());
            }
            if (ProcessorFactoryDataList.Count != ProcessorFactoryDataList.OriginalCount)
            {
                ProcessorFactoryDataList = new ProcessorFactoryDataList(ProcessorFactoryDataList.OrderBy(p => p.Priority).ToList());
            }
            if (HandlerFactoryDataList.Count != HandlerFactoryDataList.OriginalCount)
            {
                HandlerFactoryDataList = new EndpointFactoryDataList(HandlerFactoryDataList.OrderBy(p => p.Priority).ToList());
            }
            if (SelectionProcessorDataList.Count != SelectionProcessorDataList.OriginalCount)
            {
                SelectionProcessorDataList = new SelectionProcessorDataList(SelectionProcessorDataList.OrderBy(p => p.Priority).ToList());
            }
            if (OverrideProcessorDataList.Count != OverrideProcessorDataList.OriginalCount)
            {
                OverrideProcessorDataList = new OverrideProcessorDataList(OverrideProcessorDataList.OrderBy(p => p.Priority).ToList());
            }
            if (StateProcessorDataList.Count != StateProcessorDataList.OriginalCount)
            {
                StateProcessorDataList = new StateProcessorDataList(StateProcessorDataList.OrderBy(p => p.Priority).ToList());
            }
            if (PostRenderProcessorDataList.Count != PostRenderProcessorDataList.OriginalCount)
            {
                PostRenderProcessorDataList = new PostRenderProcessorDataList(PostRenderProcessorDataList.OrderBy(p => p.Priority).ToList());
            }
        }
    }
}