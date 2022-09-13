using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hackbox.Parameters
{
    public static class CommonParameters
    {
        public static readonly Dictionary<string, Type> ParameterLookup = new Dictionary<string, Type>()
        {
            ["align"] = typeof(StringParameter),
            ["background"] = typeof(StringParameter),
            ["border"] = typeof(StringParameter),
            ["shadow"] = typeof(StringParameter),
            ["radius"] = typeof(StringParameter),
            ["width"] = typeof(StringParameter),
            ["height"] = typeof(StringParameter),
            ["padding"] = typeof(StringParameter),
            ["margin"] = typeof(StringParameter),

            ["fontSize"] = typeof(StringParameter),
            ["text"] = typeof(StringParameter),
            ["label"] = typeof(StringParameter),
            ["event"] = typeof(StringParameter),
            ["value"] = typeof(StringParameter),

            ["color"] = typeof(ColorParameter),
            ["borderColor"] = typeof(ColorParameter),

            ["multiSelect"] = typeof(BoolParameter),
            
            ["hover"] = typeof(ParameterListParameter),
            ["submit"] = typeof(ParameterListParameter),

            ["choices"] = typeof(ChoicesParameter),
        };

        public static Parameter CreateParameter(string parameterName)
        {
            if (!ParameterLookup.TryGetValue(parameterName, out Type type))
            {
                Debug.LogWarning($"Failed to find type information for parameter <i>{parameterName}</i>.");
                return null;
            }

            Parameter parameter = (Parameter)Activator.CreateInstance(type);
            parameter.Name = parameterName;

            return parameter;
        }
    }
}