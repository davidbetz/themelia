#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing.Data
{
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class SequenceData
    {
        //- @Key -//
        public String Name { get; set; }

        //- @Target -//
        public ViewDataList ViewList { get; set; }

        //- @Target -//
        public VersionDataList VersionList { get; set; }

        //- ~CreateBlank -//
        internal static SequenceData CreateBlank()
        {
            SequenceData data =new SequenceData();
            data.ViewList = new ViewDataList();
            data.VersionList = new VersionDataList();
            data.Name = String.Empty;
            //+
            return data;
        }
    }
}