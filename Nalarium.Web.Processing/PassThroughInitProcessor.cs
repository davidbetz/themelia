#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Globalization;

namespace Nalarium.Web.Processing
{
    public class PassThroughInitProcessor : InitProcessor
    {
        //- @OnInitProcessorExecute -//
        public override InitProcessor Execute()
        {
            if (ParameterArray != null && ParameterArray.Length > 0)
            {
                Boolean forcePassThrough = false;
                foreach (Object item in ParameterArray)
                {
                    var value = item as String;
                    if (!String.IsNullOrEmpty(value))
                    {
                        String[] partArray = value.Split(',');
                        SelectorType type = SelectorType.Contains;
                        String criteria = String.Empty;
                        if (partArray.Length == 2)
                        {
                            try
                            {
                                type = (SelectorType)Enum.Parse(typeof(SelectorType), partArray[0].Trim());
                            }
                            catch
                            {
                                if (WebProcessingReportController.Reporter.Initialized)
                                {
                                    var map = new Map();
                                    map.Add("Section", "PassThrough");
                                    map.Add("Message", "Invalid selector type");
                                    map.Add("Selector Type", partArray[0].Trim());
                                    //+
                                    WebProcessingReportController.Reporter.AddMap(map);
                                }
                                continue;
                            }
                            criteria = partArray[1].Trim().ToLower(CultureInfo.CurrentCulture);
                        }
                        else if (partArray.Length == 1)
                        {
                            criteria = partArray[0].Trim().ToLower(CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            return null;
                        }
                        //+
                        forcePassThrough = PathMatcher.Match(type, criteria) ? true : forcePassThrough;
                    }
                }
                //+
                if (forcePassThrough)
                {
                    PassThroughHttpHandler.ForceUse = true;
                }
            }
            //+
            return null;
        }
    }
}