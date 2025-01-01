using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hackbox.Parameters
{
    [Serializable]
    public sealed class ParameterList : IParameterList
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

        #region IEnumerable Interface & Collection Initialiser Implementation
        public IEnumerator GetEnumerator()
        {
            return Parameters.GetEnumerator();
        }

        public void Add<T>(string parameterName, T value)
        {
            if (value is Parameter parameter)
            {
                int existingIndex = Parameters.FindIndex(x => x.Name == parameterName);
                parameter.Name = parameterName;
                if (existingIndex >= 0)
                {
                    Parameters[existingIndex] = parameter;
                }
                else
                {
                    Parameters.Add(parameter);
                }
            }
            else
            {
                SetParameterValue<T>(parameterName, value);
            }
        }
        #endregion

        #region Public Methods
        public Parameter<ValueT> GetGenericParameter<ValueT>(string parameterName)
        {
            Parameter parameter = this[parameterName];
            if (parameter is Parameter<ValueT> typedParameter)
            {
                return typedParameter;
            }

            if (parameter != null)
            {
                throw new Exception($"Trying to get parameter {parameterName} but using the wrong parameter type. Expected {parameter.GetType().Name}, asking for {typeof(ValueT).Name}");
            }

            Parameter newParameter = DefaultParameters.CreateDefaultAnyParameter(parameterName);
            if (newParameter is Parameter<ValueT> newTypedParameter)
            {
                Parameters.Add(newTypedParameter);
                return newTypedParameter;
            }

            return null;
        }

        public ParamT GetParameter<ParamT>(string parameterName) where ParamT: Parameter, new()
        {
            Parameter parameter = this[parameterName];
            if (parameter is ParamT typedParameter)
            {
                return typedParameter;
            }

            if (parameter != null)
            {
                throw new Exception($"Trying to get parameter {parameterName} but using the wrong parameter type. Expected {parameter.GetType().Name}, asking for {typeof(ParamT).Name}");
            }

            Parameter newParameter = DefaultParameters.CreateDefaultAnyParameter(parameterName);
            if (newParameter is ParamT newTypedParameter)
            {
                Parameters.Add(newTypedParameter);
                return newTypedParameter;
            }
                      
            return null;
        }

        public ValueT GetParameterValue<ValueT>(string parameterName)
        {
            return GetGenericParameter<ValueT>(parameterName).Value;
        }

        public void SetParameterValue<ValueT>(string parameterName, ValueT value)
        {
            GetGenericParameter<ValueT>(parameterName).Value = value;
        }

        public IEnumerable<Parameter> GetMergedParameters(ParameterList otherList)
        {
            return Parameters.Union(otherList.Parameters, ParameterListComparer.Instance);
        }
        #endregion
    }
}
