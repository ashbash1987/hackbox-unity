using System;
using Newtonsoft.Json.Linq;

namespace Hackbox.Parameters
{
    [Serializable]
    public class Parameter
    {
        public Parameter()
        {
        }

        public Parameter(Parameter from)
        {
            Name = from.Name;
        }

        public string Name = "";

        public virtual void ApplyValueToJObject(JObject parent, int version)
        {
        }
    }

    [Serializable]
    public class Parameter<T> : Parameter
    {
        public Parameter()
        {            
        }

        public virtual T Value
        {
            get;
            set;
        }

        public Parameter(Parameter<T> from):
            base(from)
        {
            Value = from.Value;
        }
    }
}
