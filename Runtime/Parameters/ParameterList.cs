using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Hackbox.Parameters
{
    [Serializable]
    public sealed class ParameterList
    {
        public ParameterList()
        {
        }

        public ParameterList(ParameterList from)
        {
            Parameters = new List<Parameter>(from.Parameters.Select(x => (Parameter)Activator.CreateInstance(x.GetType(), x)));
        }

        [HideInInspector]
        [SerializeReference]
        public List<Parameter> Parameters = new List<Parameter>();

        public Parameter this[int parameterIndex] => Parameters[parameterIndex];
        public Parameter this[string parameterName] => Parameters.Find(x => x.Name == parameterName);

        public Parameter<T> GetGenericParameter<T>(string parameterName)
        {
            Parameter parameter = this[parameterName];
            if (parameter is Parameter<T> typedParameter)
            {
                return typedParameter;
            }

            if (parameter != null)
            {
                throw new Exception($"Trying to get parameter {parameterName} but using the wrong parameter type. Expected {parameter.GetType().Name}, asking for {typeof(T).Name}");
            }

            Parameter newParameter = DefaultParameters.CreateDefaultAnyParameter(parameterName);
            if (newParameter is Parameter<T> newTypedParameter)
            {
                Parameters.Add(newTypedParameter);
                return newTypedParameter;
            }

            return null;
        }

        public T GetParameter<T>(string parameterName) where T: Parameter, new()
        {
            Parameter parameter = this[parameterName];
            if (parameter is T typedParameter)
            {
                return typedParameter;
            }

            if (parameter != null)
            {
                throw new Exception($"Trying to get parameter {parameterName} but using the wrong parameter type. Expected {parameter.GetType().Name}, asking for {typeof(T).Name}");
            }

            Parameter newParameter = DefaultParameters.CreateDefaultAnyParameter(parameterName);
            if (newParameter is T newTypedParameter)
            {
                Parameters.Add(newTypedParameter);
                return newTypedParameter;
            }
                      
            return null;
        }

        public void SetParameterValue<T>(string parameterName, T value)
        {
            GetGenericParameter<T>(parameterName).Value = value;
        }
    }
}
