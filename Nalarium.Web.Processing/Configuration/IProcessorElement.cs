#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.ComponentModel;

namespace Nalarium.Web.Processing.Configuration
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IProcessorElement
    {
        //- ProcessorType -//
        String ProcessorType { get; set; }
    }
}