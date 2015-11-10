#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using Nalarium.Activation;

namespace Nalarium.Web.Processing.Data
{
    public class ErrorProcessorData : ProcessorData, IHasPriority
    {
        //- @AcceptableTypeList -//
        public List<Type> AcceptableTypeList { get; set; }

        //+
        //- @Init -//
        public override void Init()
        {
            AcceptableTypeList = new List<Type>();
            //+
            if (ParameterArray == null)
            {
                return;
            }
            foreach (Object obj in ParameterArray)
            {
                String name = obj as String ?? String.Empty;
                if (String.IsNullOrEmpty(name))
                {
                    continue;
                }
                Type type = TypeFactoryActivator.Create(name);
                if (type != null)
                {
                    AcceptableTypeList.Add(type);
                }
            }
        }
    }
}