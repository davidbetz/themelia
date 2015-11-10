#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Globalization;

namespace Nalarium.Web.Processing
{
    internal static class ResetFlagReader
    {
        //- ~Read -//
        internal static ResetFlags Read(String data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return ResetFlags.None;
            }
            var flags = new ResetFlags();
            String[] partArray = data.ToLower(CultureInfo.CurrentCulture).Split(',');
            foreach (String part in partArray)
            {
                switch (part)
                {
                    case "endpoint":
                        flags = flags | ResetFlags.Endpoint;
                        break;
                    case "alias":
                        flags = flags | ResetFlags.Alias;
                        break;
                    case "redirect":
                        flags = flags | ResetFlags.Redirect;
                        break;
                    case "init":
                        flags = flags | ResetFlags.Init;
                        break;
                    case "selection":
                        flags = flags | ResetFlags.Selection;
                        break;
                    case "override":
                        flags = flags | ResetFlags.Override;
                        break;
                    case "state":
                        flags = flags | ResetFlags.State;
                        break;
                    case "catchall":
                        flags = flags | ResetFlags.CatchAll;
                        break;
                    case "error":
                        flags = flags | ResetFlags.Error;
                        break;
                    case "handlerfactory":
                        flags = flags | ResetFlags.HandlerFactory;
                        break;
                    case "processorfactory":
                        flags = flags | ResetFlags.ProcessorFactory;
                        break;
                    case "objectfactory":
                        flags = flags | ResetFlags.ObjectFactory;
                        break;
                    case "component":
                        flags = flags | ResetFlags.Component;
                        break;
                    case "accessrule":
                        flags = flags | ResetFlags.AccessRule;
                        break;
                    case "postrender":
                        flags = flags | ResetFlags.PostRender;
                        break;
                    case "factory":
                        flags = flags | ResetFlags.HandlerFactory | ResetFlags.ProcessorFactory | ResetFlags.ObjectFactory;
                        break;
                    case "processor":
                        flags = flags | ResetFlags.Init | ResetFlags.Selection | ResetFlags.Override | ResetFlags.Override | ResetFlags.CatchAll | ResetFlags.PostRender;
                        break;
                    default:
                        break;
                }
            }
            //+
            return flags;
        }
    }
}