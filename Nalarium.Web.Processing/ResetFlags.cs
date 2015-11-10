#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;

namespace Nalarium.Web.Processing
{
    [Flags]
    internal enum ResetFlags
    {
        None = 0x0,
        Endpoint = 0x01,
        Alias = 0x02,
        Redirect = 0x04,
        Init = 0x8,
        Selection = 0x10,
        Override = 0x20,
        State = 0x40,
        PostRender = 0x80,
        CatchAll = 0x100,
        Error = 0x200,
        HandlerFactory = 0x400,
        ProcessorFactory = 0x800,
        ObjectFactory = 0x1000,
        Component = 0x2000,
        AccessRule = 0x4000,
        Security = 0x8000
    }
}