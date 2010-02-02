#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
using Nalarium.Web.Security;
//+
namespace Nalarium.Web.Processing.Data
{
    [System.Diagnostics.DebuggerDisplay("{Key}")]
    public class SecurityData
    {
        //- @SecurityValidator -//
        internal static Nalarium.Web.Security.ISecurityValidator SecurityValidator;

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
                DefaultAccessMode = this.DefaultAccessMode,
                Disabled = this.Disabled,
                LoginText = this.LoginText,
                LogoutText = this.LogoutText,
                LoginPage = this.LoginPage,
                LogoutPage = this.LogoutPage,
                ValidatorType = this.ValidatorType,
                DefaultLoggedInTarget = this.DefaultLoggedInTarget,
                SecurityExceptionDataList = this.SecurityExceptionDataList != null ? this.SecurityExceptionDataList.Clone() : null
            };
        }
    }
}