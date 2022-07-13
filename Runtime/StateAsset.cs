using UnityEngine;

namespace Hackbox
{    
    [CreateAssetMenu(menuName = "Hackbox/State Asset")]
    public class StateAsset : ScriptableObject
    {
        public State State = null;
    }
}
