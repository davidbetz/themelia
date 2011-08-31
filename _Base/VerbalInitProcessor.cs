#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using Nalarium.Reflection;
//+

namespace Nalarium.Web.Processing
{
    /// <summary>
    /// Base class for verb processors.
    /// </summary>
    public abstract class VerbalInitProcessor : InitProcessor
    {
        internal new static Type _Type = typeof(VerbalInitProcessor);

        //+
        //- @Execute -//
        public override sealed InitProcessor Execute()
        {
            List<MethodAttributeInformation<RunForVerbsAttribute>> methodAttributeInformationArray = AttributeReader.FindMethodsWithAttribute<RunForVerbsAttribute>(this);
            if (methodAttributeInformationArray == null)
            {
                return null;
            }
            foreach (var mai in methodAttributeInformationArray)
            {
                HttpVerbs httpVerbs = mai.Attribute.HttpVerbs;
                if ((Http.Method == HttpVerbs.Get && (httpVerbs & HttpVerbs.Get) == HttpVerbs.Get) ||
                    (Http.Method == HttpVerbs.Post && (httpVerbs & HttpVerbs.Post) == HttpVerbs.Post) ||
                    (Http.Method == HttpVerbs.Post && (httpVerbs & HttpVerbs.Head) == HttpVerbs.Head) ||
                    (Http.Method == HttpVerbs.Post && (httpVerbs & HttpVerbs.Delete) == HttpVerbs.Delete) ||
                    (Http.Method == HttpVerbs.Post && (httpVerbs & HttpVerbs.Put) == HttpVerbs.Put))
                {
                    Object result = mai.MethodInfo.Invoke(this, null);
                    var resultInitProcessor = result as InitProcessor;
                    if (resultInitProcessor == null)
                    {
                        return resultInitProcessor;
                    }
                }
            }
            //+
            return null;
        }
    }
}