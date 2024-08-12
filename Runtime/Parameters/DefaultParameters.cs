using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hackbox.UI;

namespace Hackbox.Parameters
{
    public static class DefaultParameters
    {
        public class ParameterInfoEntry
        {
            public ParameterInfoEntry(Parameter parameter, string helpText = "")
            {
                Parameter = parameter;
                HelpText = helpText;
            }

            public readonly Parameter Parameter;
            public readonly string HelpText;
        }

        public static readonly Dictionary<string, ParameterInfoEntry> AllParameterInfo = new Dictionary<string, ParameterInfoEntry>()
        {
            ["align"] = new ParameterInfoEntry(new StringParameter() { Name = "align", Value = "start" }, "The alignment relative to others."),
            ["background"] = new ParameterInfoEntry(new StringParameter() { Name = "background", Value = "black" }, "The background (e.g. color, image)."),
            ["border"] = new ParameterInfoEntry(new StringParameter() { Name = "border", Value = "2px solid black" }, "The border."),
            ["borderColor"] = new ParameterInfoEntry(new ColorParameter() { Name = "borderColor", Value = Color.black }, "The color of the border."),
            ["borderRadius"] = new ParameterInfoEntry(new StringParameter() { Name = "borderRadius", Value = "10px" }, "The radius of the border."),
            ["choices"] = new ParameterInfoEntry(new ChoicesParameter() { Name = "choices", Value = new List<ChoicesParameter.Choice>() }, "The choices for a choice component."),
            ["color"] = new ParameterInfoEntry(new ColorParameter() { Name = "color", Value = Color.black }, "The main/text color."),
            ["event"] = new ParameterInfoEntry(new StringParameter() { Name = "event", Value = "" }, "The event name raised when submitting."),
            ["fontFamily"] = new ParameterInfoEntry(new StringParameter() { Name = "fontFamily", Value = "" }, "The font family of the label."),
            ["fontSize"] = new ParameterInfoEntry(new StringParameter() { Name = "fontSize", Value = "20px" }, "The size of the font of the label."),
            ["grid"] = new ParameterInfoEntry(new BoolParameter() { Name = "grid", Value = false }, "If set to true, aligns the child elements in a grid."),
            ["gridColumns"] = new ParameterInfoEntry(new IntParameter() { Name = "gridColumns", Value = 2 }, "The number of columns wide for the grid layout."),
            ["gridGap"] = new ParameterInfoEntry(new StringParameter() { Name = "gridGap", Value = "10px" }, "The gap between cells in the grid layout."),
            ["gridRowHeight"] = new ParameterInfoEntry(new StringParameter() { Name = "gridRowHeight", Value = "1fr" }, "The height of each cell in the grid layout."),
            ["height"] = new ParameterInfoEntry(new StringParameter() { Name = "height", Value = "100px" }, "The height."),
            ["hover"] = new ParameterInfoEntry(new ParameterListParameter() { Name = "hover", Value = new ParameterList() }, "Styling specific to when the element is being hovered."),
            ["label"] = new ParameterInfoEntry(new StringParameter() { Name = "label", Value = "Label" }, "The text that appears on the element."),
            ["margin"] = new ParameterInfoEntry(new StringParameter() { Name = "margin", Value = "10px 0px" }, "The gap outside the border of the element."),
            ["max"] = new ParameterInfoEntry(new IntParameter() { Name = "max", Value = 100 }, "The maximum value."),
            ["min"] = new ParameterInfoEntry(new IntParameter() { Name = "min", Value = 0 }, "The minimum value."),
            ["multiSelect"] = new ParameterInfoEntry(new BoolParameter() { Name = "multiSelect", Value = false }, "If set to true, allows multiple selection."),
            ["padding"] = new ParameterInfoEntry(new StringParameter() { Name = "padding", Value = "0px 10px" }, "The gap between the border and the contents of the element."),
            ["persistent"] = new ParameterInfoEntry(new BoolParameter() { Name = "persistent", Value = false }, "If set to true, the text box will remain active and clear out its contents."),
            ["radius"] = new ParameterInfoEntry(new StringParameter() { Name = "radius", Value = "10px" }, "The radius of the border."),
            ["shadow"] = new ParameterInfoEntry(new StringParameter() { Name = "shadow", Value = "5px 5px #000000" }, "The shadow of the element."),
            ["step"] = new ParameterInfoEntry(new IntParameter() { Name = "step", Value = 1 }, "The step increment."),
            ["submit"] = new ParameterInfoEntry(new ParameterListParameter() { Name = "submit", Value = new ParameterList() }, "Defines the submit button."),
            ["text"] = new ParameterInfoEntry(new StringParameter() { Name = "text", Value = "Text" }, "The text that appears on the element."),
            ["value"] = new ParameterInfoEntry(new StringParameter() { Name = "value", Value = "value" }, "The value of the element returned in events."),
            ["width"] = new ParameterInfoEntry(new StringParameter() { Name = "width", Value = "100%" }, "The width."),
        };

        public static readonly Dictionary<string, Parameter> AllParameterLookup = AllParameterInfo.Select(x => x.Value.Parameter).ToDictionary(x => x.Name);

        public static readonly Dictionary<string, Parameter> HeaderParameterLookup = new Dictionary<string, Parameter>()
        {
            ["background"] = new StringParameter() { Value = "black" },
            ["color"] = new ColorParameter() { Value = Color.black },
            ["minHeight"] = new StringParameter() { Value = "50px" },
            ["maxHeight"] = new StringParameter() { Value = "50px" },
            ["text"] = new StringParameter() { Value = "" }
        };

        public static readonly Dictionary<string, Parameter> MainParameterLookup = new Dictionary<string, Parameter>()
        {
            ["align"] = new StringParameter() { Value = "start" },
            ["background"] = new StringParameter() { Value = "black" },
            ["minWidth"] = new StringParameter() { Value = "300px" },
            ["maxWidth"] = new StringParameter() { Value = "350px" }
        };

        public static Dictionary<string, Parameter> GetDefaultParameters(object parent, params Parameter[] parameterChain)
        {
            if (parent is Preset.PresetType presetType)
            {
                switch (presetType)
                {
                    case Preset.PresetType.Text:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["text"] = new StringParameter() { Value = "Sample Text" }
                            };
                        }
                        return null;

                    case Preset.PresetType.TextInput:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["event"] = new StringParameter() { Value = "text" },
                                ["persistent"] = new BoolParameter() { Value = false },
                                ["type"] = new StringParameter() { Value = "text" },
                                ["max"] = new IntParameter() { Value = 100 },
                                ["min"] = new IntParameter() { Value = 0 },
                                ["step"] = new IntParameter() { Value = 1 }
                            };
                        }
                        return null;

                    case Preset.PresetType.Buzzer:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["event"] = new StringParameter() { Value = "buzz" },
                                ["label"] = new StringParameter() { Value = "BUZZ" }
                            };
                        }
                        return null;

                    case Preset.PresetType.Button:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["event"] = new StringParameter() { Value = "buzz" },
                                ["label"] = new StringParameter() { Value = "A: 42" },
                                ["keys"] = new StringArrayParameter() { Value = new string[0] },
                                ["value"] = new StringParameter() { Value = "A" },
                            };
                        }
                        return null;

                    case Preset.PresetType.Choices:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["choices"] = new ChoicesParameter() { Value = new List<ChoicesParameter.Choice>() },
                                ["event"] = new StringParameter() { Value = "text" },
                                ["multiSelect"] = new BoolParameter() { Value = false },
                                ["submit"] = new ParameterListParameter() { Value = new ParameterList() }
                            };
                        }
                        else if (parameterChain[parameterChain.Length - 1].Name == "submit")
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["label"] = new StringParameter() { Value = "A: 42" },
                                ["style"] = new ParameterListParameter() { Value = new ParameterList() }
                            };
                        }
                        else if (parameterChain.Length >= 2 && parameterChain[parameterChain.Length - 1].Name == "style" && parameterChain[parameterChain.Length - 2].Name == "submit")
                        {
                            return GetDefaultStyleParameters(parent);
                        }
                        else if (parameterChain.Length >= 3 && parameterChain[parameterChain.Length - 1].Name == "hover" && parameterChain[parameterChain.Length - 2].Name == "style" && parameterChain[parameterChain.Length - 3].Name == "submit")
                        {
                            return GetDefaultStyleParameters(parent, parameterChain[parameterChain.Length - 1]);
                        }
                        return null;

                    case Preset.PresetType.Range:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["event"] = new StringParameter() { Value = "buzz" },
                                ["max"] = new IntParameter() { Value = 100 },
                                ["min"] = new IntParameter() { Value = 0 },
                                ["step"] = new IntParameter() { Value = 1 }
                            };
                        }
                        return null;

                    default:
                        return null;
                }
            }

            if (parent is Preset preset)
            {
                return GetDefaultParameters(preset.Type, parameterChain);
            }

            if (parent is UIComponent component)
            {
                return GetDefaultParameters(component.Preset, parameterChain);
            }

            return null;
        }

        public static Dictionary<string, Parameter> GetDefaultStyleParameters(object parent, params Parameter[] parameterChain)
        {
            if (parent is Preset.PresetType presetType)
            {
                switch (presetType)
                {
                    case Preset.PresetType.Text:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["align"] = new StringParameter() { Value = "center" },
                                ["background"] = new StringParameter() { Value = "white" },
                                ["border"] = new StringParameter() { Value = "4px solid black" },
                                ["borderRadius"] = new StringParameter() { Value = "10px" },
                                ["color"] = new ColorParameter() { Value = Color.black },
                                ["fontFamily"] = new StringParameter() { Value = "sans-serif" },
                                ["fontSize"] = new StringParameter() { Value = "16px" },
                                ["margin"] = new StringParameter() { Value = "10px 0px" },
                                ["padding"] = new StringParameter() { Value = "10px" },
                                ["width"] = new StringParameter() { Value = "auto" },
                            };
                        }
                        return null;

                    case Preset.PresetType.TextInput:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["align"] = new StringParameter() { Value = "center" },
                                ["background"] = new StringParameter() { Value = "white" },
                                ["border"] = new StringParameter() { Value = "2px solid black" },
                                ["borderRadius"] = new StringParameter() { Value = "0px" },
                                ["color"] = new ColorParameter() { Value = Color.black },
                                ["fontFamily"] = new StringParameter() { Value = "sans-serif" },
                                ["fontSize"] = new StringParameter() { Value = "30px" },
                                ["margin"] = new StringParameter() { Value = "10px 0" },
                                ["padding"] = new StringParameter() { Value = "10px" },
                                ["width"] = new StringParameter() { Value = "100%" }
                            };
                        }
                        return null;

                    case Preset.PresetType.Buzzer:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["align"] = new StringParameter() { Value = "center" },
                                ["background"] = new StringParameter() { Value = "white" },
                                ["border"] = new StringParameter() { Value = "4px solid black" },
                                ["color"] = new ColorParameter() { Value = Color.black },
                                ["fontFamily"] = new StringParameter() { Value = "" },
                                ["fontSize"] = new StringParameter() { Value = "70px" },
                                ["height"] = new StringParameter() { Value = "300px" },
                                ["radius"] = new StringParameter() { Value = "70px" },
                                ["shadow"] = new StringParameter() { Value = "5px 5px #000000" },

                                ["hover"] = new ParameterListParameter() { Value = new ParameterList() }
                            };
                        }
                        else if (parameterChain[parameterChain.Length - 1].Name == "hover")
                        {
                            return new Dictionary<string, Parameter>()
                            {
                            };
                        }
                        return null;

                    case Preset.PresetType.Button:
                    case Preset.PresetType.Choices:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["align"] = new StringParameter() { Value = "center" },
                                ["background"] = new StringParameter() { Value = "#AAAAAA" },
                                ["border"] = new StringParameter() { Value = "2px solid black" },
                                ["borderRadius"] = new StringParameter() { Value = "10px" },
                                ["color"] = new ColorParameter() { Value = Color.black },
                                ["fontFamily"] = new StringParameter() { Value = "sans-serif" },
                                ["fontSize"] = new StringParameter() { Value = "20px" },
                                ["grid"] = new BoolParameter() { Value = false },
                                ["gridColumns"] = new IntParameter() { Value = 2 },
                                ["gridGap"] = new StringParameter() { Value = "10px" },
                                ["gridRowHeight"] = new StringParameter() { Value = "1fr" },
                                ["margin"] = new StringParameter() { Value = "10px 0px" },
                                ["padding"] = new StringParameter() { Value = "0 20px" },
                                ["width"] = new StringParameter() { Value = "100%" },

                                ["hover"] = new ParameterListParameter() { Value = new ParameterList() }
                            };
                        }
                        else if (parameterChain[parameterChain.Length - 1].Name == "choices")
                        {
                            Dictionary<string, Parameter> parameters = GetDefaultStyleParameters(parent);
                            parameters.Remove("grid");
                            parameters.Remove("gridColumns");
                            parameters.Remove("gridGap");
                            parameters.Remove("gridRowHeight");
                            return parameters;
                        }
                        else if (parameterChain[parameterChain.Length - 1].Name == "hover")
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["background"] = new StringParameter() { Value = "#AAAAAA" },
                                ["color"] = new ColorParameter() { Value = Color.black },
                            };
                        }
                        return null;

                    case Preset.PresetType.Range:
                        if (parameterChain == null || parameterChain.Length == 0)
                        {
                            return new Dictionary<string, Parameter>()
                            {
                                ["align"] = new StringParameter() { Value = "center" },
                                ["background"] = new StringParameter() { Value = "white" },
                                ["border"] = new StringParameter() { Value = "4px solid black" },
                                ["borderRadius"] = new StringParameter() { Value = "0px" },
                                ["color"] = new ColorParameter() { Value = Color.black },
                                ["fontFamily"] = new StringParameter() { Value = "" },
                                ["fontSize"] = new StringParameter() { Value = "16px" },
                                ["margin"] = new StringParameter() { Value = "10px 0" },
                                ["padding"] = new StringParameter() { Value = "10px" },
                                ["width"] = new StringParameter() { Value = "100%" }
                            };
                        }
                        return null;

                    default:
                        return null;
                }
            }

            if (parent is ChoicesParameter.Choice)
            {
                return GetDefaultStyleParameters(Preset.PresetType.Choices);
            }

            if (parent is Preset preset)
            {
                return GetDefaultStyleParameters(preset.Type, parameterChain);
            }

            if (parent is UIComponent component)
            {
                return GetDefaultStyleParameters(component.Preset, parameterChain);
            }

            return null;
        }

        public static Parameter CreateDefaultHeaderParameter(string parameterName)
        {
            return CreateParameter(parameterName, HeaderParameterLookup);
        }

        public static Parameter CreateDefaultMainParameter(string parameterName)
        {
            return CreateParameter(parameterName, MainParameterLookup);
        }

        public static Parameter CreateDefaultParameter(string parameterName, object parent, params Parameter[] parameterChain)
        {
            return CreateParameter(parameterName, GetDefaultParameters(parent, parameterChain));
        }

        public static Parameter CreateDefaultStyleParameter(string parameterName, object parent, params Parameter[] parameterChain)
        {
            return CreateParameter(parameterName, GetDefaultStyleParameters(parent, parameterChain));
        }

        public static Parameter CreateDefaultAnyParameter(string parameterName)
        {
            return CreateParameter(parameterName, AllParameterLookup);
        }

        private static Parameter CreateParameter(string parameterName, Dictionary<string, Parameter> parameterLookup)
        {
            if (parameterLookup == null)
            {
                return null;
            }

            if (!parameterLookup.TryGetValue(parameterName, out Parameter defaultParameter))
            {
                Debug.LogWarning($"Failed to find default parameter information for parameter <i>{parameterName}</i>.");
                return null;
            }

            return CreateDefaultParameter(parameterName, defaultParameter);
        }

        internal static Parameter CreateDefaultParameter(string parameterName, Parameter parameterLookup)
        {
            Parameter parameter = (Parameter)Activator.CreateInstance(parameterLookup.GetType(), parameterLookup);
            parameter.Name = parameterName;

            return parameter;
        }
    }
}