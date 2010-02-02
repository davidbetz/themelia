#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
namespace Nalarium.Web.Processing
{
    public enum CatchAllMode
    {
        Custom = 0,
        PassThrough = 1,
        PassToDefault = 2,
        Forbidden = 3,
        Blocked = 4,
        Redirect = 5,
        RedirectToRoot = 6,
        Page = 7,
        NotFound = 8
    }
}