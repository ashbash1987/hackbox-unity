using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Hackbox.UI
{
    [CreateAssetMenu(menuName = "Hackbox/Theme")]
    public class Theme : ScriptableObject
    {
        public Color HeaderTextColor = Color.black;
        public Color HeaderBackgroundColor = Color.white;
        public Color MainBackgroundColor = Color.blue;

        private JObject _obj = new JObject();

        public JObject GenerateJSON()
        {
            _obj["header"] = JObject.FromObject(new
            {
                textColor = HeaderTextColor.ToHTMLString(),
                backgroundColor = HeaderBackgroundColor.ToHTMLString()
            });
            _obj["main"] = JObject.FromObject(new
            {
                backgroundColor = MainBackgroundColor.ToHTMLString()
            });

            return _obj;
        }
    }
}
