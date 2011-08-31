#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections;
using System.Web.UI;
using Nalarium.Web.Processing.Data;

namespace Nalarium.Web.Processing.Sequence
{
    public class ViewVersionControlBuilder : ControlBuilder
    {
        //- @GetChildControlType -//
        public override Type GetChildControlType(String tagName, IDictionary attribs)
        {
            if (tagName.Equals("View", StringComparison.InvariantCultureIgnoreCase))
            {
                return typeof(ViewData);
            }
            else if (tagName.Equals("Version", StringComparison.InvariantCultureIgnoreCase))
            {
                return typeof(VersionData);
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