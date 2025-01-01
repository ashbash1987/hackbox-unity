using Newtonsoft.Json;
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

        #region Public Methods
        public static Theme Create(string name)
        {
            Theme theme = ScriptableObject.CreateInstance<Theme>();
            theme.name = name;
            return theme;
        }
        #endregion

        #region Internal Methods
        internal void WriteJSON(JsonTextWriter json)
        {
            json.WriteStartObject();
            {
                json.WritePropertyName("header");
                json.WriteStartObject();
                {
                    json.WritePropertyName("color"); json.WriteValue(HeaderColor.ToHTMLString());
                    json.WritePropertyName("background"); json.WriteValue(HeaderBackground);
                    json.WritePropertyName("minHeight"); json.WriteValue(HeaderMinHeight);
                    json.WritePropertyName("maxHeight"); json.WriteValue(HeaderMaxHeight);

                    if (!string.IsNullOrEmpty(HeaderFontFamily))
                    {
                        json.WritePropertyName("fontFamily"); json.WriteValue(HeaderFontFamily);
                    }
                }
                json.WriteEndObject();

                json.WritePropertyName("main");
                json.WriteStartObject();
                {
                    json.WritePropertyName("color"); json.WriteValue(MainColor.ToHTMLString());
                    json.WritePropertyName("background"); json.WriteValue(MainBackground);
                    json.WritePropertyName("minWidth"); json.WriteValue(MainMinWidth);
                    json.WritePropertyName("maxWidth"); json.WriteValue(MainMaxWidth);

                }
                json.WriteEndObject();
            }

            json.WriteEndObject();
        }
        #endregion
    }
}
