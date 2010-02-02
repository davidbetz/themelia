#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
using Nalarium.Web.Processing.Data;
//+
namespace Nalarium.Web.Processing
{
    public abstract class Component
    {
        internal static Type _Type = typeof(Component);
        //+
        private String key;
        //+
        private FactoryDataList _factoryDataList;
        private ProcessorDataList _processorDataList;
        private EndpointDataList _endpointDataList;

        //+
        //- @Key -//
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public String Key
        {
            get { return key; }
            set
            {
                //+ only set once
                if (String.IsNullOrEmpty(key))
                {
                    key = value;
                }
            }
        }

        //- @Installed -//
        /// <summary>
        /// Gets or sets a value indicating whether this component is installed.
        /// </summary>
        /// <value><c>true</c> if installed; otherwise, <c>false</c>.</value>
        public Boolean Installed { get; internal set; }

        //- @ParameterMap -//
        /// <summary>
        /// Represents a key/value map of component parameters.
        /// </summary>
        public Map ParameterMap { get; internal set; }

        //- @Register -//
        /// <summary>
        /// Registers the specified processing information data.
        /// </summary>
        /// <param name="processorDataList">The processor data list.</param>
        /// <param name="factoryDataList">The factory data list.</param>
        public abstract void Register();

        //- ~Connect -//
        internal void Connect(FactoryDataList factoryDataList, ProcessorDataList processorDataList, EndpointDataList endpointDataList)
        {
            _factoryDataList = factoryDataList;
            _processorDataList = processorDataList;
            _endpointDataList = endpointDataList;
        }

        //- #AddEndpoint -//
        protected void AddEndpoint(EndpointData endpointData)
        {
            endpointData.Source = Key;
            _endpointDataList.Add(endpointData);
        }

        //- #AddFactory -//
        protected void AddFactory(FactoryData factoryData)
        {
            factoryData.Source = Key;
            _factoryDataList.Add(factoryData);
        }

        //- #AddProcessor -//
        protected void AddProcessor(ProcessorData processorData)
        {
            processorData.Source = Key;
            _processorDataList.Add(processorData);
        }
    }
}