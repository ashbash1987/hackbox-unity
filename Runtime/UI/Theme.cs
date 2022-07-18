using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Hackbox.UI
{
    [CreateAssetMenu(menuName = "Hackbox/Theme")]
    public class Theme : ScriptableObject
    {
        public Color HeaderColor = Color.black;
        public string HeaderBackground = "#ffffff";
        public Color MainColor = Color.white;
        public string MainBackground = "#0000ff";

        private JObject _obj = new JObject();

        public JObject GenerateJSON()
        {
            _obj["header"] = JObject.FromObject(new
            {
                color = HeaderColor.ToHTMLString(),
                background = HeaderBackground
            });
            _obj["main"] = JObject.FromObject(new
            {
                color = MainColor.ToHTMLString(),
                background = MainBackground
            });

            return _obj;
        }
    }
}
