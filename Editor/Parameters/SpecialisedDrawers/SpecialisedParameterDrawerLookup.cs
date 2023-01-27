using System;
using System.Collections.Generic;

namespace Hackbox.Parameters
{
    public static class SpecialisedParameterDrawerLookup
    {
        public static readonly Dictionary<string, Type> AllParameterDrawerLookup = new Dictionary<string, Type>()
        {
            ["background"] = typeof(BackgroundParameterDrawer)
        };

        public static BaseParameterDrawer CreateSpecialisedParameterDrawer(Parameter parameter)
        {
            return CreateSpecialisedParameterDrawer(parameter.Name);
        }

        public static BaseParameterDrawer CreateSpecialisedParameterDrawer(string parameterName)
        {
            if (AllParameterDrawerLookup.TryGetValue(parameterName, out Type parameterDrawerType))
            {
                return (BaseParameterDrawer)Activator.CreateInstance(parameterDrawerType);
            }

            return null;
        }
    }
}