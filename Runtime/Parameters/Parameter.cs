﻿using System;
using Newtonsoft.Json;

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

        public virtual void WriteProp(JsonTextWriter json)
        {
        }
    }

    [Serializable]
    public class Parameter<T> : Parameter
    {
        public Parameter()
        {            
        }

        public Parameter(T value)
        {
            Value = value;
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
