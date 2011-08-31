#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Diagnostics;
using Nalarium.Web.Security;
//+

namespace Nalarium.Web.Processing.Data
{
    [DebuggerDisplay("{Key}")]
    public class SecurityData
    {
        //- @SecurityValidator -//
        internal static ISecurityValidator SecurityValidator;

        //+
        //- @DefaultAccessMode -//
        public DefaultAccessMode DefaultAccessMode { get; set; }

        //- @ValidatorType -//
        public String ValidatorType { get; set; }

        //- @LoginText -//
        public String LoginText { get; set; }

        //- @LoginText -//
        public String LogoutText { get; set; }

        //- @LoginPage -//
        public String LoginPage { get; set; }

        //- @LogoutPage -//
        public String LogoutPage { get; set; }

        //- @DefaultLoggedInTarget -//
        public String DefaultLoggedInTarget { get; set; }

        //- @Disabled -//
        public Boolean Disabled { get; set; }

        //- @SecurityExceptionDataList -//
        public SecurityExceptionDataList SecurityExceptionDataList { get; set; }

        //- ~Clone -//
        internal SecurityData Clone()
        {
            return new SecurityData
                   {
                       DefaultAccessMode = DefaultAccessMode,
                       Disabled = Disabled,
                       LoginText = LoginText,
                       LogoutText = LogoutText,
                       LoginPage = LoginPage,
                       LogoutPage = LogoutPage,
                       ValidatorType = ValidatorType,
                       DefaultLoggedInTarget = DefaultLoggedInTarget,
                       SecurityExceptionDataList = SecurityExceptionDataList != null ? SecurityExceptionDataList.Clone() : null
                   };
        }
    }
}