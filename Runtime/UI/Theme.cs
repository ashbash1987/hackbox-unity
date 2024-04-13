using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Hackbox.UI
{
    [CreateAssetMenu(menuName = "Hackbox/Theme")]
    public class Theme : ScriptableObject
    {
        [Header("Header")]
        [Tooltip("Color of the text of the header section.")]
        public Color HeaderColor = Color.black;
        [BackgroundString]
        [Tooltip("The background of the header section.")]
        public string HeaderBackground = "#ffffff";
        [Tooltip("The minimum height of the header section.")]
        public string HeaderMinHeight = "50px";
        [Tooltip("The maximum height of the header section.")]
        public string HeaderMaxHeight = "50px";
        [Tooltip("The header section font family.")]
        public string HeaderFontFamily = "";

        [Header("Main")]
        [Tooltip("Color of the text of the main section.")]
        public Color MainColor = Color.white;
        [BackgroundString]
        [Tooltip("The background of the main section.")]
        public string MainBackground = "#0000ff";
        [Tooltip("The minimum width of the main section.")]
        public string MainMinWidth = "300px";
        [Tooltip("The maximum width of the main section.")]
        public string MainMaxWidth = "350px";

        [Header("Fonts")]
        public List<string> Fonts = new List<string>();

        public Color? HeaderBackgroundColor
        {
            get
            {
                if (ColorUtility.TryParseHtmlString(HeaderBackground, out Color color))
                {
                    return color;
                }
                return null;
            }
            set => HeaderBackground = value?.ToHTMLStringWithAlpha();
        }

        public Color? MainBackgroundColor
        {
            get
            {
                if (ColorUtility.TryParseHtmlString(MainBackground, out Color color))
                {
                    return color;
                }
                return null;
            }
            set => MainBackground = value?.ToHTMLStringWithAlpha();
        }

        private JObject _obj = new JObject();

        #region Public Methods
        public static Theme Create(string name)
        {
            Theme theme = ScriptableObject.CreateInstance<Theme>();
            theme.name = name;
            return theme;
        }
        #endregion

        #region Internal Methods
        internal JObject GenerateJSON(int version)
        {
            JObject header = new JObject();
            header["color"] = HeaderColor.ToHTMLString();
            header["background"] = HeaderBackground;
            header["minHeight"] = HeaderMinHeight;
            header["maxHeight"] = HeaderMaxHeight;
            if (!string.IsNullOrEmpty(HeaderFontFamily))
            {
                header["fontFamily"] = HeaderFontFamily;
            }
            _obj["header"] = header;

            JObject main = new JObject();
            main["color"] = MainColor.ToHTMLString();
            main["background"] = MainBackground;
            main["minWidth"] = MainMinWidth;
            main["maxWidth"] = MainMaxWidth;
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
        #endregion
    }
}
