using System.Collections;

namespace Hackbox.Parameters
{
    public interface IStyleParameterList : IEnumerable
    {
        Parameter<ValueT> GetGenericStyleParameter<ValueT>(string parameterName);
        ParamT GetStyleParameter<ParamT>(string parameterName) where ParamT : Parameter, new();
        ValueT GetStyleParameterValue<ValueT>(string parameterName);
        void SetStyleParameterValue<ValueT>(string parameterName, ValueT value);
    }
}
