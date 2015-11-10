#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Diagnostics;

namespace Nalarium.Web.Processing.Data
{
    [DebuggerDisplay("{Selector}, {Type}, {Text}")]
    public class EndpointData
    {
        private String _guid;

        //+
        //- @Selector -//
        /// <summary>
        /// Selector type to use in endpoint selection.
        /// </summary>
        public SelectorType Selector { get; set; }

        //- @Type -//
        /// <summary>
        /// Type of the endpoint.
        /// </summary>
        public String Type { get; set; }

        //- @Text -//
        /// <summary>
        /// Endpoint text to use in URL matching.
        /// </summary>
        public String Text { get; set; }

        //- @AccessRuleGroup-//
        public String AccessRuleGroup { get; set; }

        //- @TextWithoutSlash -//
        /// <summary>
        /// Endpoint text without ending slash.
        /// </summary>
        public String TextWithoutSlash { get; internal set; }

        //- @RequireSlash -//
        /// <summary>
        /// True is an ending slash is required for this endpoint to be used.
        /// </summary>
        public Boolean RequireSlash { get; set; }

        //- @ParameterValue -//
        /// <summary>
        /// Value of the parameter for use with the endpoint.
        /// </summary>
        public String ParameterValue { get; set; }

        //- ~SubEndpointDataList -//
        public EndpointDataList SubEndpointDataList { get; set; }

        //- ~Source -//
        internal String Source { get; set; }

        //- ~OriginalMatchText -//
        internal String OriginalMatchText { get; set; }

        //- ~Guid -//
        internal String Guid
        {
            get
            {
                if (_guid == null)
                {
                    _guid = GuidCreator.GetNewGuid();
                }
                //+
                return _guid;
            }
        }

        //- @ParameterMap -//
        /// <summary>
        /// Endpoint data for use in the underlying handler.
        /// </summary>
        public Map ParameterMap { get; set; }

        //+
        //- @Clone -//
        public EndpointData Clone()
        {
            var newMap = new Map(ParameterMap);
            var newData = new EndpointData
                          {
                              OriginalMatchText = OriginalMatchText,
                              Text = Text,
                              TextWithoutSlash = TextWithoutSlash,
                              Selector = Selector,
                              Type = Type,
                              ParameterValue = ParameterValue,
                              ParameterMap = newMap,
                              RequireSlash = RequireSlash,
                              SubEndpointDataList = SubEndpointDataList != null ? SubEndpointDataList.Clone() : new EndpointDataList(),
                              Source = Source
                          };
            //+
            return this;
        }

        //- ~GetTextWithoutSlash -//
        internal static String GetTextWithoutSlash(String text)
        {
            if (text.EndsWith("/"))
            {
                return text.Substring(0, text.Length - 1);
            }
            //+
            return text;
        }

        //- @Create -//
        /// <summary>
        /// Creates an endpoint for programmatic use in a component (with ending slash not required).
        /// </summary>
        /// <param name="type">Selector type.</param>
        /// <param name="text">URL text to match.</param>
        /// <param name="name">Endpoint type.</param>
        /// <returns>EndpointData object.</returns>
        public static EndpointData Create(SelectorType type, String text, String name)
        {
            return Create(type, text, name, String.Empty, false);
        }

        /// <summary>
        /// Creates an endpoint for programmatic use in a component.
        /// </summary>
        /// <param name="type">Selector type.</param>
        /// <param name="text">URL text to match.</param>
        /// <param name="name">Endpoint type.</param>
        /// <param name="requireSlash">true is slash is required, false if not.</param>
        /// <returns>EndpointData object.</returns>
        public static EndpointData Create(SelectorType type, String text, String name, Boolean requireSlash)
        {
            return Create(type, text, name, String.Empty, requireSlash);
        }

        /// <summary>
        /// Creates an endpoint for programmatic use in a component (with ending slash not required).
        /// </summary>
        /// <param name="type">Selector type.</param>
        /// <param name="text">URL text to match.</param>
        /// <param name="name">Endpoint type.</param>
        /// <param name="parameter">Parameter to pass to the endpoint.</param>
        /// <returns>EndpointData object.</returns>
        public static EndpointData Create(SelectorType type, String text, String name, String parameter)
        {
            return Create(type, text, name, parameter, false);
        }

        /// <summary>
        /// Creates an endpoint for programmatic use in a component.
        /// </summary>
        /// <param name="type">Selector type.</param>
        /// <param name="text">URL text to match.</param>
        /// <param name="name">Endpoint type.</param>
        /// <param name="parameter">Parameter to pass to the endpoint.</param>
        /// <param name="requireSlash">true is slash is required, false if not.</param>
        /// <returns>EndpointData object.</returns>
        public static EndpointData Create(SelectorType type, String text, String name, String parameter, Boolean requireSlash)
        {
            return new EndpointData
                   {
                       Type = name,
                       Selector = type,
                       RequireSlash = requireSlash,
                       Text = text,
                       TextWithoutSlash = GetTextWithoutSlash(text),
                       ParameterValue = parameter,
                   };
        }

        /// <summary>
        /// Creates an endpoint for programmatic use in a component.
        /// </summary>
        /// <param name="type">Selector type.</param>
        /// <param name="text">URL text to match.</param>
        /// <param name="name">Endpoint type.</param>
        /// <param name="parameterMap">Parameter map to send to the endpoint.</param>
        /// <returns>EndpointData object.</returns>
        public static EndpointData Create(SelectorType type, String text, String name, Map parameterMap)
        {
            return Create(type, text, name, parameterMap, false);
        }

        /// <summary>
        /// Creates an endpoint for programmatic use in a component.
        /// </summary>
        /// <param name="type">Selector type.</param>
        /// <param name="text">URL text to match.</param>
        /// <param name="name">Endpoint type.</param>
        /// <param name="parameterMap">Parameter map to send to the endpoint.</param>
        /// <param name="requireSlash">true is slash is required, false if not.</param>
        /// <returns>EndpointData object.</returns>
        public static EndpointData Create(SelectorType type, String text, String name, Map parameterMap, Boolean requireSlash)
        {
            return new EndpointData
                   {
                       Type = name,
                       Selector = type,
                       RequireSlash = requireSlash,
                       Text = text,
                       TextWithoutSlash = GetTextWithoutSlash(text),
                       ParameterMap = parameterMap
                   };
        }
    }
}