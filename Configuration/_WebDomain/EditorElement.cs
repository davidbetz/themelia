#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System.Configuration;
//+
//+
namespace Nalarium.Web.Processing.Configuration
{
    public class EditorElement : Nalarium.Configuration.CommentableElement
    {
        //- @Users -//
        [ConfigurationProperty("users")]
        [ConfigurationCollection(typeof(UserCollection), AddItemName = "add")]
        public UserCollection Users
        {
            get
            {
                return (UserCollection)this["users"];
            }
        }
    }
}