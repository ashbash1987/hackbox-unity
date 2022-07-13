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
            ["border"] = typeof(StringParameter),
            ["shadow"] = typeof(StringParameter),
            ["radius"] = typeof(StringParameter),
            ["width"] = typeof(StringParameter),
            ["height"] = typeof(StringParameter),
            ["fontSize"] = typeof(StringParameter),

            ["textColor"] = typeof(ColorParameter),
            ["backgroundColor"] = typeof(ColorParameter),
            ["borderColor"] = typeof(ColorParameter),
            ["hoverTextColor"] = typeof(ColorParameter),
            ["hoverBackgroundColor"] = typeof(ColorParameter),
            ["hoverBorderColor"] = typeof(ColorParameter),

            ["text"] = typeof(StringParameter),
            ["label"] = typeof(StringParameter),
            ["value"] = typeof(StringParameter),
            ["choices"] = typeof(ChoicesParameter),
            ["event"] = typeof(StringParameter)
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