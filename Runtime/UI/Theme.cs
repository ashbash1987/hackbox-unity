using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Hackbox.UI
{
    [CreateAssetMenu(menuName = "Hackbox/Theme")]
    public class Theme : ScriptableObject
    {
        public Color HeaderColor = Color.black;
        [BackgroundString]
        public string HeaderBackground = "#ffffff";
        public Color MainColor = Color.white;
        [BackgroundString]
        public string MainBackground = "#0000ff";

        private JObject _obj = new JObject();

        internal JObject GenerateJSON(int version)
        {
            JObject header = new JObject();
            header["color"] = HeaderColor.ToHTMLString();
            header["background"] = HeaderBackground;
            _obj["header"] = header;

            JObject main = new JObject();
            main["color"] = MainColor.ToHTMLString();
            main["background"] = MainBackground;
            _obj["main"] = main;

            return _obj;
        }
    }
}
