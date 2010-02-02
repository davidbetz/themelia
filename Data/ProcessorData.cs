#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing.Data
{
    [System.Diagnostics.DebuggerDisplay("{ProcessorType}, {Priority}")]
    public class ProcessorData : Nalarium.IHasPriority
    {
        //- @ParameterArray -//
        public Object[] ParameterArray { get; set; }

        //- @ProcessorType -//
        public String ProcessorType { get; set; }

        //- @Priority -//
        public Int32 Priority { get; set; }

        //- @Enabled -//
        public Boolean Enabled { get; set; }

        //- ~Source -//
        internal String Source { get; set; }

        //+
        //- @Init -//
        public virtual void Init()
        {
        }

        //- @Create -//
        public static T Create<T>(String processorType) where T : ProcessorData, new()
        {
            T data = new T
            {
                ProcessorType = processorType
            };
            data.Init();
            //+
            return data;
        }
        public static T Create<T>(String processorType, Object[] parameterArray) where T : ProcessorData, new()
        {
            T data = new T
            {
                ProcessorType = processorType,
                ParameterArray = parameterArray
            };
            data.Init();
            //+
            return data;
        }
        public static T Create<T>(String processorType, Object[] parameterArray, Int32 priority, Boolean enabled) where T : ProcessorData, new()
        {
            T data = new T
            {
                ProcessorType = processorType,
                ParameterArray = parameterArray,
                Priority = priority,
                Enabled = enabled
            };
            data.Init();
            //+
            return data;
        }
    }
}