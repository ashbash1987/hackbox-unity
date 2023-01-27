using System;
using System.Collections.Generic;
using UnityEngine;
using Hackbox.UI;

namespace Hackbox.Parameters
{
    public static class DefaultParameters
    {
        public static readonly Dictionary<string, Parameter> AllParameterLookup = new Dictionary<string, Parameter>()
        {
            ["align"] = new StringParameter() { Value = "start" },
            ["background"] = new StringParameter() { Value = "black" },
            ["border"] = new StringParameter() { Value = "2px solid black" },
            ["borderColor"] = new ColorParameter() { Value = Color.black },
            ["borderRadius"] = new StringParameter() { Value = "10px" },
            ["choices"] = new ChoicesParameter() { Value = new List<ChoicesParameter.Choice>() },
            ["color"] = new ColorParameter() { Value = Color.black },
            ["event"] = new StringParameter() { Value = "" },
            ["fontSize"] = new StringParameter() { Value = "20px" },
            ["height"] = new StringParameter() { Value = "100px" },
            ["hover"] = new ParameterListParameter() { Value = new ParameterList() },
            ["label"] = new StringParameter() { Value = "Label" },
            ["margin"] = new StringParameter() { Value = "10px 0px" },
            ["multiSelect"] = new BoolParameter() { Value = false },
            ["padding"] = new StringParameter() { Value = "0px 10px" },
            ["radius"] = new StringParameter() { Value = "10px" },
            ["shadow"] = new StringParameter() { Value = "5px 5px #000000" },
            ["submit"] = new ParameterListParameter() { Value = new ParameterList() },
            ["text"] = new StringParameter() { Value = "Text" },
            ["value"] = new StringParameter() { Value = "value" },
            ["width"] = new StringParameter() { Value = "100%" },
        };

        public static readonly Dictionary<string, Parameter> HeaderParameterLookup = new Dictionary<string, Parameter>()
        {
            ["color"] = new ColorParameter() { Value = Color.black },
            ["background"] = new StringParameter() { Value = "black" },
            ["text"] = new StringParameter() { Value = "" }
        };

        public static readonly Dictionary<string, Parameter> MainParameterLookup = new Dictionary<string, Parameter>()
        {
            ["background"] = new StringParameter() { Value = "black" },
            ["align"] = new StringParameter() { Value = "start" }
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
                                ["event"] = new StringParameter() { Value = "text" }
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
                                ["color"] = new ColorParameter() { Value = Color.black }
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
                                ["border"] = new StringParameter() { Value = "4px solid black" },
                                ["borderRadius"] = new StringParameter() { Value = "0px" },
                                ["color"] = new ColorParameter() { Value = Color.black },
                                ["fontSize"] = new StringParameter() { Value = "30px" }
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
                                ["background"] = new StringParameter() { Value = "white" },
                                ["border"] = new StringParameter() { Value = "4px solid black" },
                                ["borderRadius"] = new StringParameter() { Value = "10px" },
                                ["color"] = new ColorParameter() { Value = Color.black },
                                ["fontSize"] = new StringParameter() { Value = "20px" },
                                ["padding"] = new StringParameter() { Value = "0 20px" },
                                ["margin"] = new StringParameter() { Value = "10px 0px" },
                                ["width"] = new StringParameter() { Value = "100%" },

                                ["hover"] = new ParameterListParameter() { Value = new ParameterList() }
                            };
                        }
                        else if (parameterChain[parameterChain.Length - 1].Name == "choices")
                        {
                            return GetDefaultStyleParameters(parent);
                        }
                        else if (parameterChain[parameterChain.Length - 1].Name == "hover")
                        {
                            Dictionary<string, Parameter> parameters = GetDefaultStyleParameters(parent);
                            parameters.Remove("hover");
                            return parameters;
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