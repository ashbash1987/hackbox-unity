using System.Collections.Generic;
using Hackbox.Parameters;

namespace Hackbox.Builders
{
    /// <summary>
    /// Builder class for constructing a ParameterList with various parameters.
    /// </summary>
    public sealed class ParameterListBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterListBuilder"/> class.
        /// </summary>
        public ParameterListBuilder()
        {
            ParameterList = new ParameterList();
        }

        /// <summary>
        /// Gets the ParameterList being built.
        /// </summary>
        public ParameterList ParameterList
        {
            get;
            private set;
        }

        #region Public Methods
        /// <summary>
        /// Creates a new instance of the <see cref="ParameterListBuilder"/> class.
        /// </summary>
        /// <returns>A new instance of <see cref="ParameterListBuilder"/>.</returns>
        public static ParameterListBuilder Create()
        {
            return new ParameterListBuilder();
        }

        /// <summary>
        /// Sets the 'choices' parameter.
        /// </summary>
        /// <param name="choices">The list of choices to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetChoices(List<ChoicesParameter.Choice> choices)
        {
            ParameterList.SetParameterValue("choices", choices);
            return this;
        }

        /// <summary>
        /// Sets the 'event' parameter.
        /// </summary>
        /// <param name="eventName">The event name to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetEvent(string eventName)
        {
            ParameterList.SetParameterValue("event", eventName);
            return this;
        }

        /// <summary>
        /// Sets the 'label' parameter.
        /// </summary>
        /// <param name="label">The label to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetLabel(string label)
        {
            ParameterList.SetParameterValue("label", label);
            return this;
        }

        /// <summary>
        /// Sets the 'min' (minimum value) parameter.
        /// </summary>
        /// <param name="minimumValue">The minimum value to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetMinimum(int minimumValue)
        {
            ParameterList.SetParameterValue("min", minimumValue);
            return this;
        }

        /// <summary>
        /// Sets the 'max' (maximum value) parameter.
        /// </summary>
        /// <param name="maximumValue">The maximum value to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetMaximum(int maximumValue)
        {
            ParameterList.SetParameterValue("max", maximumValue);
            return this;
        }

        /// <summary>
        /// Sets the 'multiSelect' parameter.
        /// </summary>
        /// <param name="multiSelect">The multi-select value to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetMultiSelect(bool multiSelect)
        {
            ParameterList.SetParameterValue("multiSelect", multiSelect);
            return this;
        }

        /// <summary>
        /// Sets the 'min' and 'max' range parameters.
        /// </summary>
        /// <param name="minimumValue">The minimum value to set.</param>
        /// <param name="maximumValue">The maximum value to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetRange(int minimumValue, int maximumValue)
        {
            SetMinimum(minimumValue);
            return SetMaximum(maximumValue);
        }

        /// <summary>
        /// Sets the 'persistent' parameter.
        /// </summary>
        /// <param name="persistent">The persistent value to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetPersistent(bool persistent)
        {
            ParameterList.SetParameterValue("persistent", persistent);
            return this;
        }

        /// <summary>
        /// Sets the 'step' parameter.
        /// </summary>
        /// <param name="step">The step value to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetStep(int step)
        {
            ParameterList.SetParameterValue("step", step);
            return this;
        }

        /// <summary>
        /// Sets the 'style' parameter.
        /// </summary>
        /// <param name="parameterList">The style parameter list to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetStyle(ParameterList parameterList)
        {
            ParameterList.SetParameterValue("style", parameterList);
            return this;
        }

        /// <summary>
        /// Sets the 'submit' parameter.
        /// </summary>
        /// <param name="parameterList">The submit parameter list to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetSubmit(ParameterList parameterList)
        {
            ParameterList.SetParameterValue("submit", parameterList);
            return this;
        }

        /// <summary>
        /// Sets the 'text' parameter.
        /// </summary>
        /// <param name="text">The text to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetText(string text)
        {
            ParameterList.SetParameterValue("text", text);
            return this;
        }

        /// <summary>
        /// Sets the 'type' parameter.
        /// </summary>
        /// <param name="type">The type to set.</param>
        /// <returns>The current instance of <see cref="ParameterListBuilder"/>.</returns>
        public ParameterListBuilder SetType(string type)
        {
            ParameterList.SetParameterValue("type", type);
            return this;
        }

        /// <summary>
        /// Implicitly converts a <see cref="ParameterListBuilder"/> to a <see cref="ParameterList"/>.
        /// </summary>
        /// <param name="builder">The builder to convert.</param>
        public static implicit operator ParameterList(ParameterListBuilder builder) => builder.ParameterList;
        #endregion
    }
}
