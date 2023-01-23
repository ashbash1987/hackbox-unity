using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hackbox.Parameters
{
    public static class CommonParameters
    {
        public static readonly Dictionary<string, Type> StyleParameterLookup = new Dictionary<string, Type>()
        {
            ["fontSize"] = typeof(StringParameter),
            ["color"] = typeof(ColorParameter),
            ["align"] = typeof(StringParameter),

            ["width"] = typeof(StringParameter),
            ["height"] = typeof(StringParameter),
            ["padding"] = typeof(StringParameter),
            ["margin"] = typeof(StringParameter),

            ["background"] = typeof(StringParameter),
            ["shadow"] = typeof(StringParameter),

            ["border"] = typeof(StringParameter),
            ["borderRadius"] = typeof(StringParameter),
            ["borderColor"] = typeof(ColorParameter),
            ["radius"] = typeof(StringParameter),

            ["hover"] = typeof(ParameterListParameter),
        };

        public static readonly Dictionary<string, Type> NormalParameterLookup = new Dictionary<string, Type>()
        {
            ["text"] = typeof(StringParameter),
            ["label"] = typeof(StringParameter),
            ["event"] = typeof(StringParameter),
            ["value"] = typeof(StringParameter),

            ["multiSelect"] = typeof(BoolParameter),
            
            ["submit"] = typeof(ParameterListParameter),

            ["choices"] = typeof(ChoicesParameter),

            ["min"] = typeof(IntParameter),
            ["max"] = typeof(IntParameter),
            ["step"] = typeof(IntParameter),
        };

        public static readonly Dictionary<string, Type> HeaderParameterLookup = new Dictionary<string, Type>()
        {
            ["color"] = typeof(ColorParameter),
            ["background"] = typeof(StringParameter),
            ["text"] = typeof(StringParameter)
        };

        public static readonly Dictionary<string, Type> MainParameterLookup = new Dictionary<string, Type>()
        {
            ["background"] = typeof(StringParameter),
            ["align"] = typeof(StringParameter)
        };

        public static IEnumerable<string> AllParameterKeys => StyleParameterLookup.Keys
                                                               .Concat(NormalParameterLookup.Keys)
                                                               .Concat(HeaderParameterLookup.Keys)
                                                               .Concat(MainParameterLookup.Keys).Distinct();

        public static Parameter CreateStyleParameter(string parameterName)
        {
            return CreateParameter(parameterName, StyleParameterLookup);
        }

        public static Parameter CreateNormalParameter(string parameterName)
        {
            return CreateParameter(parameterName, NormalParameterLookup);
        }

        public static Parameter CreateHeaderParameter(string parameterName)
        {
            return CreateParameter(parameterName, HeaderParameterLookup);
        }

        public static Parameter CreateMainParameter(string parameterName)
        {
            return CreateParameter(parameterName, MainParameterLookup);
        }

        public static Parameter CreateAnyParameter(string parameterName)
        {
            return CreateParameter(parameterName, StyleParameterLookup, NormalParameterLookup, HeaderParameterLookup, MainParameterLookup);
        }

        private static Parameter CreateParameter(string parameterName, params Dictionary<string, Type>[] parameterLookups)
        {
            Type typeLookup = parameterLookups.Select(x =>
            {
                if (x.TryGetValue(parameterName, out Type type))
                {
                    return type;
                }

                return null;
            }).Where(x => x != null).FirstOrDefault();

            if (typeLookup == null)
            {
                Debug.LogWarning($"Failed to find type information for parameter <i>{parameterName}</i>.");
                return null;
            }

            Parameter parameter = (Parameter)Activator.CreateInstance(typeLookup);
            parameter.Name = parameterName;

            return parameter;
        }
    }
}