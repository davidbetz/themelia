#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.IO;
using System.Text.RegularExpressions;
using Nalarium.Web.Processing.Configuration;
//+

namespace Nalarium.Web.Processing
{
    internal class ConfigViewerHttpHandler : ReusableHttpHandler
    {
        //- @Process -//
        public override void Process()
        {
            ProcessingSection cs = ProcessingSection.GetConfigSection();
            ContentType = "text/plain";
            //+
            String xml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cs.SectionInformation.ConfigSource));
            var re = new Regex("<!--(?:[^-]|-(?!->))*-->");
            xml = re.Replace(xml, String.Empty);
            re = new Regex("(\\s*)\n(\\s*)\n", RegexOptions.Singleline);
            xml = re.Replace(xml, "\n");
            //+
            Response.Write(xml);
        }
    }
}