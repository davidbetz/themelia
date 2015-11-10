#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2007-2009

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
//+

namespace Nalarium.Web.Processing
{
    internal class ScannedTypeCacheInitProcessor : SystemInitProcessor
    {
        private static readonly Type _httpHandlerType = typeof(IHttpHandler);

        //- @Execute -//
        public override InitProcessor Execute()
        {
            if (!ScannedTypeCache.IsTypeListLoaded("httpHandler"))
            {
                //+ load
                List<Type> list = LoadAllTypeData();
                SetupHttpHandlerTypeList(_httpHandlerType, list);
            }
            //+
            return null;
        }

        //- $SetupHttpHandlerTypeList -//
        private void SetupHttpHandlerTypeList(Type type, List<Type> list)
        {
            if (type == null)
            {
                return;
            }
            Func<Type, Boolean> isOfBaseType = p =>
                                               p != null && p.IsPublic && !p.IsAbstract && type.IsAssignableFrom(p)
                                               && !p.Namespace.StartsWith("System.")
                                               && !p.Namespace.StartsWith("Nalarium.");
            ScannedTypeCache.SetTypeData("httpHandler", list.Where(isOfBaseType).ToList());
        }

        //- $LoadAllTypeData -//
        private List<Type> LoadAllTypeData()
        {
            var list = new List<Type>();
            ICollection collection = BuildManager.GetReferencedAssemblies();
            foreach (Assembly assembly in collection)
            {
                Type[] typeArray;
                try
                {
                    typeArray = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    typeArray = ex.Types;
                }
                list.AddRange(typeArray);
            }
            //+
            return list;
        }
    }
}