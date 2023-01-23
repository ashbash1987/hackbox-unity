using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Serialization;
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
            Choices,
            Range
        }

        public PresetType Type = PresetType.Text;
        
        [StyleParameterList]
        public ParameterList StyleParameterList = new ParameterList();
        [NormalParameterList]
        public ParameterList ParameterList = new ParameterList();

        private JObject _obj = new JObject();

        private void OnValidate()
        {
            //This is somewhat temporary to get style-based parameters into a specific parameter collection
            for (int parameterIndex = 0; parameterIndex < ParameterList.Parameters.Count; ++parameterIndex)
            {
                Parameter parameter = ParameterList[parameterIndex];
                if (!CommonParameters.NormalParameterLookup.ContainsKey(parameter.Name) &&
                    CommonParameters.StyleParameterLookup.ContainsKey(parameter.Name))
                {
                    StyleParameterList.Parameters.Add(parameter);
                    ParameterList.Parameters.Remove(parameter);
                    parameterIndex--;
                }
            }
        }

        public JObject GenerateJSON(int version)
        {
            _obj["type"] = Type.ToString();
            _obj["props"] = GenerateProps(version);

            return _obj;
        }

        private JObject GenerateProps(int version)
        {
            JObject props = new JObject();

            switch (version)
            {
                case 1:
                    foreach (Parameter parameter in StyleParameterList.Parameters)
                    {
                        parameter.ApplyValueToJObject(props, version);
                    }
                    break;

                default:
                    JObject styleProps = new JObject();
                    props["style"] = styleProps;

                    foreach (Parameter parameter in StyleParameterList.Parameters)
                    {
                        parameter.ApplyValueToJObject(styleProps, version);
                    }
                    break;
            }

            foreach (Parameter parameter in ParameterList.Parameters)
            {
                parameter.ApplyValueToJObject(props, version);
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
