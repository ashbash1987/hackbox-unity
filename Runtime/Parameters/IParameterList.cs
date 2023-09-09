using System.Collections;

namespace Hackbox.Parameters
{
    public interface IParameterList : IEnumerable
    {
        Parameter this[string parameterName]
        {
            get;
        }

        Parameter<ValueT> GetGenericParameter<ValueT>(string parameterName);
        ParamT GetParameter<ParamT>(string parameterName) where ParamT : Parameter, new();
        ValueT GetParameterValue<ValueT>(string parameterName);
        void SetParameterValue<ValueT>(string parameterName, ValueT value);
    }
}
