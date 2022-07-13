using Newtonsoft.Json.Linq;
using UnityEngine;
using Hackbox.Parameters;

namespace Hackbox.UI
{
    [CreateAssetMenu(menuName = "Hackbox/Preset")]
    public class Preset : ScriptableObject
    {
        public enum PresetType
        {
            Text,
            TextInput,
            Buzzer,
            Button,
            Choices
        }

        public PresetType Type = PresetType.Text;
        public ParameterList ParameterList = new ParameterList();

        private JObject _obj = new JObject();

        public JObject GenerateJSON()
        {
            _obj["type"] = Type.ToString();
            _obj["props"] = GenerateProps();

            return _obj;
        }

        private JObject GenerateProps()
        {
            JObject props = new JObject();
            foreach (Parameter parameter in ParameterList.Parameters)
            {
                parameter.ApplyValueToJObject(props);
            }

            return props;
        }

        public override int GetHashCode()
        {
            return GetInstanceID();
        }

        public override bool Equals(object other)
        {
            if (other is Preset otherPreset)
            {
                return GetInstanceID() == otherPreset.GetInstanceID();
            }

            return base.Equals(other);
        }
    }
}
