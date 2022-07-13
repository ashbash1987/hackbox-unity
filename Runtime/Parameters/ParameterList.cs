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
    }
}
