#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
using Nalarium.Web.Globalization;
//+
namespace Nalarium.Web.Processing
{
    internal class ConfigEditorHttpHandler : ReusableHttpHandler
    {
        //- @Process -//
        public override void Process()
        {
            ContentType = "text/plain";
            //+
            if (Http.Method == HttpVerbs.Get)
            {
                ProcessGet();
            }
            else if (Http.Method == HttpVerbs.Post)
            {
                ProcessPost();
            }
        }

        //- $ProcessGet -//
        private void ProcessGet()
        {
            Map map = HttpData.GetQueryItemMap();
            String type = map["type"];
            String name = map["name"];
            if (map.Count > 0 && !String.IsNullOrEmpty(type) && !String.IsNullOrEmpty(name))
            {
                switch (type)
                {
                    case "css":
                    case "js":
                        ShowResource(type, name);
                        break;
                }
            }
            else
            {
                ShowScreen();
            }
        }

        //- $ShowResource -//
        private void ShowResource(String type, String name)
        {
            if (type == "css")
            {
                switch (name)
                {
                    case "default":
                        break;
                }
            }
            else
            {
                switch (name)
                {
                    case "default":
                        break;
                }
            }
        }

        //- $ShowScreen -//
        private void ShowScreen()
        {
            ContentType = "text/html";
            //+
            Output.AppendLine(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd""> 
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"" lang=""en""> 
<head><title>" + ResourceAccessor.GetString("Editor_Title") + @"</title></head> 
<body>
<div id=""title""></div>
<div id=""separation""></div>
<div id=""content""></div>
</body>
</html>");
        }

        //- $ProcessPost -//
        private void ProcessPost()
        {
            String input = HttpData.GetInputHttpString();
            String[] partArray = input.Split('\x02');
            switch (partArray[0])
            {
                case "load":
                    break;
                case "save":
                    break;
            }
        }
    }
}
