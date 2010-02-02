#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing.Sequence
{
    public class ViewVersionControlBuilder : System.Web.UI.ControlBuilder
    {
        //- @GetChildControlType -//
        public override Type GetChildControlType(String tagName, System.Collections.IDictionary attribs)
        {
            if (tagName.Equals("View", StringComparison.InvariantCultureIgnoreCase))
            {
                return typeof(Nalarium.Web.Processing.Data.ViewData);
            }
            else
            if (tagName.Equals("Version", StringComparison.InvariantCultureIgnoreCase))
            {
                return typeof(Nalarium.Web.Processing.Data.VersionData);
            }
            //+
            return base.GetChildControlType(tagName, attribs);
        }

        //- @AppendLiteralString -//
        public override void AppendLiteralString(string s)
        {
        }
    }
}
    