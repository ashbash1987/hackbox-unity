using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hackbox.Parameters
{
    public static class CommonParameters
    {
        public static readonly Dictionary<string, Type> AllParameterLookup = new Dictionary<string, Type>()
        {
            ["align"] = typeof(StringParameter),
            ["background"] = typeof(StringParameter),
            ["border"] = typeof(StringParameter),
            ["borderColor"] = typeof(ColorParameter),
            ["borderRadius"] = typeof(StringParameter),
            ["choices"] = typeof(ChoicesParameter),
            ["color"] = typeof(ColorParameter),
            ["event"] = typeof(StringParameter),
            ["fontSize"] = typeof(StringParameter),
            ["height"] = typeof(StringParameter),
            ["hover"] = typeof(ParameterListParameter),
            ["label"] = typeof(StringParameter),
            ["margin"] = typeof(StringParameter),
            ["multiSelect"] = typeof(BoolParameter),
            ["padding"] = typeof(StringParameter),
            ["radius"] = typeof(StringParameter),
            ["shadow"] = typeof(StringParameter),
            ["submit"] = typeof(ParameterListParameter),
            ["text"] = typeof(StringParameter),
            ["value"] = typeof(StringParameter),
            ["width"] = typeof(StringParameter),
        };

        public static Parameter CreateAnyParameter(string parameterName)
        {
            if (!AllParameterLookup.TryGetValue(parameterName, out Type typeLookup))
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