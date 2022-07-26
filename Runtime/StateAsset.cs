using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hackbox.UI;
using Hackbox.Parameters;

namespace Hackbox
{    
    [CreateAssetMenu(menuName = "Hackbox/State Asset")]
    public class StateAsset : ScriptableObject
    {
        public State State = null;

        private void OnValidate()
        {
            HashSet<Parameter> parameters = new HashSet<Parameter>();

            foreach (UIComponent component in State.Components)
            {
                foreach (Parameter parameter in component.ParameterList.Parameters)
                {
                    if (parameters.Any(x => Object.ReferenceEquals(x, parameter)))
                    {
                        component.ParameterList = new ParameterList(component.ParameterList);
                    }
                    parameters.Add(parameter);
                }
            }
        }
    }
}
