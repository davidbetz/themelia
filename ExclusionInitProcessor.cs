#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

//+
namespace Nalarium.Web.Processing
{
    internal class ExclusionInitProcessor : InitProcessor
    {
        //- @OnInitProcessorExecute -//
        public override InitProcessor Execute()
        {
            switch (Http.GetUrlPart(Position.First))
            {
                case "content_":
                case "partition_":
                case "resource_":
                case "service_":
                    FlowControl.EndProcessing();
                    break;
            }
            //+
            return null;
        }
    }
}