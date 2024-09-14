using System.Collections.Generic;
using Hackbox.Parameters;

namespace Hackbox.Builders
{
    public sealed class ParameterListBuilder
    {
        public ParameterListBuilder()
        {
            ParameterList = new ParameterList();
        }

        public ParameterList ParameterList
        {
            get;
            private set;
        }

        #region Public Methods
        public static ParameterListBuilder Create()
        {
            return new ParameterListBuilder();
        }

        public ParameterListBuilder SetChoices(List<ChoicesParameter.Choice> choices)
        {
            ParameterList.SetParameterValue("choices", choices);
            return this;
        }

        public ParameterListBuilder SetEvent(string eventName)
        {
            ParameterList.SetParameterValue("event", eventName);
            return this;
        }

        public ParameterListBuilder SetLabel(string label)
        {
            ParameterList.SetParameterValue("label", label);
            return this;
        }

        public ParameterListBuilder SetMinimum(int minimumValue)
        {
            ParameterList.SetParameterValue("min", minimumValue);
            return this;
        }

        public ParameterListBuilder SetMaximum(int maximumValue)
        {
            ParameterList.SetParameterValue("max", maximumValue);
            return this;
        }

        public ParameterListBuilder SetMultiSelect(bool multiSelect)
        {
            ParameterList.SetParameterValue("multiSelect", multiSelect);
            return this;
        }

        public ParameterListBuilder SetRange(int minimumValue, int maximumValue)
        {
            SetMinimum(minimumValue);
            return SetMaximum(maximumValue);
        }

        public ParameterListBuilder SetPersistent(bool persistent)
        {
            ParameterList.SetParameterValue("persistent", persistent);
            return this;
        }

        public ParameterListBuilder SetStep(int step)
        {
            ParameterList.SetParameterValue("step", step);
            return this;
        }

        public ParameterListBuilder SetStyle(ParameterList parameterList)
        {
            ParameterList.SetParameterValue("style", parameterList);
            return this;
        }

        public ParameterListBuilder SetSubmit(ParameterList parameterList)
        {
            ParameterList.SetParameterValue("submit", parameterList);
            return this;
        }

        public ParameterListBuilder SetText(string text)
        {
            ParameterList.SetParameterValue("text", text);
            return this;
        }

        public ParameterListBuilder SetType(string type)
        {
            ParameterList.SetParameterValue("type", type);
            return this;
        }

        public static implicit operator ParameterList(ParameterListBuilder builder) => builder.ParameterList;
        #endregion
    }
}
