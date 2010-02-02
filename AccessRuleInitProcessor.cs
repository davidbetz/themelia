#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
//+
//+
namespace Nalarium.Web.Processing
{
    internal class AccessRuleInitProcessor : SystemInitProcessor
    {
        public override InitProcessor Execute()
        {
            AccessRule.ConfigurationLoader.Load();
            //+
            return null;
        }
    }
}