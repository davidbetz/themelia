#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing.Data
{
    [System.Diagnostics.DebuggerDisplay("{FactoryType}, {Priority}")]
    public class FactoryData : Nalarium.IHasPriority
    {
        //- @ParameterArray -//
        public Object[] ParameterArray { get; set; }

        //- @ParameterMap -//
        public Map ParameterMap { get; set; }

        //- @FactoryType -//
        public String FactoryType { get; set; }

        //- @Priority -//
        public Int32 Priority { get; set; }

        //- ~Source -//
        internal String Source { get; set; }

        //+
        //- @Create -//
        public static FactoryData Create(String processorType)
        {
            return new FactoryData
            {
                FactoryType = processorType
            };
        }
        public static FactoryData Create(String processorType, Object[] parameterArray)
        {
            return new FactoryData
            {
                FactoryType = processorType,
                ParameterArray = parameterArray
            };
        }
        public static FactoryData Create(String processorType, Map parameterMap)
        {
            return new FactoryData
            {
                FactoryType = processorType,
                ParameterMap = parameterMap
            };
        }
        public static FactoryData Create(String processorType, Object[] parameterArray, Map parameterMap)
        {
            return new FactoryData
            {
                FactoryType = processorType,
                ParameterArray = parameterArray,
                ParameterMap = parameterMap
            };
        }
        public static FactoryData Create(String processorType, Object[] parameterArray, Int32 priority)
        {
            return new FactoryData
            {
                FactoryType = processorType,
                ParameterArray = parameterArray,
                Priority = priority
            };
        }
    }
}