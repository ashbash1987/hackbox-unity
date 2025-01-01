using System.Collections.Generic;

namespace Hackbox.Parameters
{
    public sealed class ParameterListComparer : IEqualityComparer<Parameter>
    {
        public static ParameterListComparer Instance = new ParameterListComparer();

        public bool Equals(Parameter x, Parameter y) => ReferenceEquals(x, y) || x.Name.Equals(y.Name);       
        public int GetHashCode(Parameter obj) => obj.Name.GetHashCode();
    }
}
