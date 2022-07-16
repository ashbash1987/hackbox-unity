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


        public T GetParameter<T>(string parameterName) where T : Parameter, new()
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

            typedParameter = new T() { Name = parameterName };
            Parameters.Add(typedParameter);

            return typedParameter;
        }
    }
}
