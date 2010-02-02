#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Linq;
//+
namespace Nalarium.Web.Processing.Data
{
    [System.Diagnostics.DebuggerDisplay("{Name}, {Path}, {AcceptMissingTrailingSlash}, BasedOn: {BasedOn}, IsBasedOn: {IsBasedOn}")]
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
            WebDomainData data = new WebDomainData();
            //data.Default = this.Default;
            data.DefaultType = this.DefaultType;
            data.ParameterDataList = this.ParameterDataList;
            data.DefaultParameter = this.DefaultParameter;
            data.AcceptMissingTrailingSlash = this.AcceptMissingTrailingSlash;
            data.Subdomain = this.Subdomain;
            data.IsSealed = this.IsSealed;
            data.CatchAllMode = this.CatchAllMode;
            data.CatchAllInitParameter = this.CatchAllInitParameter;
            data.InitProcessorDataList = this.InitProcessorDataList.Clone();
            data.PostRenderProcessorDataList = this.PostRenderProcessorDataList.Clone();
            data.SecurityData = this.SecurityData.Clone();
            data.ComponentDataList = this.ComponentDataList.Clone();
            data.CatchAllEndpoint = this.CatchAllEndpoint.Clone();
            data.EndpointDataList = this.EndpointDataList.Clone();
            data.HandlerFactoryDataList = this.HandlerFactoryDataList.Clone();
            data.SelectionProcessorDataList = this.SelectionProcessorDataList.Clone();
            data.OverrideProcessorDataList = this.OverrideProcessorDataList.Clone();
            data.StateProcessorDataList = this.StateProcessorDataList.Clone();
            //data.AccessRuleDataList = this.AccessRuleDataList.Clone();
            data.ProcessorFactoryDataList = this.ProcessorFactoryDataList.Clone();
            //+
            return data;
        }

        //- $SortPriority -//
        private static Int32 SortPriority(Nalarium.IHasPriority p1, Nalarium.IHasPriority p2)
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
            if (this.InitProcessorDataList.Count != this.InitProcessorDataList.OriginalCount)
            {
                this.InitProcessorDataList = new InitProcessorDataList(this.InitProcessorDataList.OrderBy(p => p.Priority).ToList());
            }
            if (this.ProcessorFactoryDataList.Count != this.ProcessorFactoryDataList.OriginalCount)
            {
                this.ProcessorFactoryDataList = new ProcessorFactoryDataList(this.ProcessorFactoryDataList.OrderBy(p => p.Priority).ToList());
            }
            if (this.HandlerFactoryDataList.Count != this.HandlerFactoryDataList.OriginalCount)
            {
                this.HandlerFactoryDataList = new EndpointFactoryDataList(this.HandlerFactoryDataList.OrderBy(p => p.Priority).ToList());
            }
            if (this.SelectionProcessorDataList.Count != this.SelectionProcessorDataList.OriginalCount)
            {
                this.SelectionProcessorDataList = new SelectionProcessorDataList(this.SelectionProcessorDataList.OrderBy(p => p.Priority).ToList());
            }
            if (this.OverrideProcessorDataList.Count != this.OverrideProcessorDataList.OriginalCount)
            {
                this.OverrideProcessorDataList = new OverrideProcessorDataList(this.OverrideProcessorDataList.OrderBy(p => p.Priority).ToList());
            }
            if (this.StateProcessorDataList.Count != this.StateProcessorDataList.OriginalCount)
            {
                this.StateProcessorDataList = new StateProcessorDataList(this.StateProcessorDataList.OrderBy(p => p.Priority).ToList());
            }
            if (this.PostRenderProcessorDataList.Count != this.PostRenderProcessorDataList.OriginalCount)
            {
                this.PostRenderProcessorDataList = new PostRenderProcessorDataList(this.PostRenderProcessorDataList.OrderBy(p => p.Priority).ToList());
            }
        }
    }
}