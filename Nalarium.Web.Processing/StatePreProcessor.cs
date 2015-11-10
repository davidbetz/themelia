#region Copyright
//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
#endregion
using System;
//+
namespace Nalarium.Web.Processing
{
    public class StateInitProcessor : Nalarium.Web.Processing.InitProcessor
    {
        //- @OnInitProcessorExecute -//
        public override InitProcessor Execute()
        {
            StateTracker.InitState();
            if (Http.Method != HttpVerbs.Post)
            {
                return null;
            }
            //+ message
            String message = Http.Form.Get("__THEMELIAMESSAGE");
            if (!String.IsNullOrEmpty(message))
            {
                Int32 hashPosition = message.IndexOf("#");
                if (hashPosition > -1)
                {
                    String messageParameterSeries = message.Substring(hashPosition + 1, message.Length - hashPosition - 1);
                    StateTracker.PostedMessageParameterArray = messageParameterSeries.Split('#');
                    message = message.Substring(0, hashPosition);
                }
                StateTracker.PostedMessage = message.Replace("@", "::");
                StateTracker.Message = StateTracker.PostedMessage;
            }
            //+ state
            StringKeyValueMap stringKeyValueMap = new StringKeyValueMap();
            String state = Http.Form.Get("__THEMELIASTATE");
            if (!String.IsNullOrEmpty(state))
            {
                String[] partArray = GetStatePartArray(state);
                LoadControlData(stringKeyValueMap, partArray[0]);
                LoadValueData(stringKeyValueMap, partArray[1]);
            }
            StateTracker.OriginalState = stringKeyValueMap;
            //+
            return null;
        }

        //- $LoadControlData -//
        private static void LoadControlData(StringKeyValueMap stringKeyValueMap, String blogState)
        {
            Nalarium.Data.Base64StorableMap map = new Nalarium.Data.Base64StorableMap(blogState);
            map.GetKeyList().ForEach((controlKey) =>
            {
                String controlId = map[controlKey];
                String value = Http.Form.Get(controlId);
                //+ re-register
                StateTracker.Set(StateEntryType.ControlId, controlKey, controlId);
                //+ load
                if (!String.IsNullOrEmpty(value))
                {
                    stringKeyValueMap.Add(controlKey, new StringKeyValue
                    {
                        Key = controlId,
                        Value = value
                    });
                }
                else
                {
                    stringKeyValueMap.Add(controlKey, new StringKeyValue
                    {
                        Key = controlId,
                        Value = null
                    });
                }
            });
        }

        //- $LoadControlData -//
        private static void LoadValueData(StringKeyValueMap stringKeyValueMap, String blogState)
        {
            Nalarium.Data.Base64StorableMap map = new Nalarium.Data.Base64StorableMap(blogState);
            map.GetKeyList().ForEach((key) => StateTracker.Set(StateEntryType.Value, key, map[key]));
        }

        //- $GetStatePartArray -//
        private String[] GetStatePartArray(String state)
        {
            String controlIdPart = String.Empty;
            String valuePart = String.Empty;
            Int32 splitterLocation = state.IndexOf(":");
            if (splitterLocation > -1)
            {
                controlIdPart = state.Substring(1, splitterLocation - 1);
                valuePart = state.Substring(splitterLocation + 2, state.Length - splitterLocation - 2);
            }
            else
            {
                if (state[0] == 'C')
                {
                    controlIdPart = state.Substring(1, state.Length - 1);
                }
                else if (state[0] == 'V')
                {
                    valuePart = state.Substring(1, state.Length - 1);
                }
            }
            //+
            return new String[2] { controlIdPart, valuePart };
        }
    }
}