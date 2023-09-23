using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Hackbox.UI
{
    [CreateAssetMenu(menuName = "Hackbox/Theme")]
    public class Theme : ScriptableObject
    {
        [Header("Header")]
        public Color HeaderColor = Color.black;
        [BackgroundString]
        public string HeaderBackground = "#ffffff";
        public string HeaderFontFamily = "";

        [Header("Main")]
        public Color MainColor = Color.white;
        [BackgroundString]
        public string MainBackground = "#0000ff";

        [Header("Fonts")]
        public List<string> Fonts = new List<string>();

        private JObject _obj = new JObject();

        internal JObject GenerateJSON(int version)
        {
            JObject header = new JObject();
            header["color"] = HeaderColor.ToHTMLString();
            header["background"] = HeaderBackground;
            if (!string.IsNullOrEmpty(HeaderFontFamily))
            {
                header["fontFamily"] = HeaderFontFamily;
            }
            _obj["header"] = header;

            JObject main = new JObject();
            main["color"] = MainColor.ToHTMLString();
            main["background"] = MainBackground;
            _obj["main"] = main;

            if (Fonts != null && Fonts.Count > 0)
            {
                JArray fonts = new JArray();
                foreach (string font in Fonts)
                {
                    JObject fontObject = new JObject();
                    fontObject["family"] = font;
                    fonts.Add(fontObject);
                }
                _obj["fonts"] = fonts;
            }

            return _obj;
        }
    }
}
