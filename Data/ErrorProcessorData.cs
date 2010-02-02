#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
using System.Collections.Generic;
//+
namespace Nalarium.Web.Processing.Data
{
    public class ErrorProcessorData : ProcessorData, Nalarium.IHasPriority
    {
        //- @AcceptableTypeList -//
        public List<Type> AcceptableTypeList { get; set; }

        //+
        //- @Init -//
        public override void Init()
        {
            AcceptableTypeList = new List<Type>();
            //+
            if (this.ParameterArray == null)
            {
                return;
            }
            foreach (Object obj in this.ParameterArray)
            {
                String name = obj as String ?? String.Empty;
                if (String.IsNullOrEmpty(name))
                {
                    continue;
                }
                Type type = Nalarium.Activation.TypeFactoryActivator.Create(name);
                if (type != null)
                {
                    AcceptableTypeList.Add(type);
                }
            }
        }
    }
}