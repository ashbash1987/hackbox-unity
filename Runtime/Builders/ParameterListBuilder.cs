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

        public ParameterListBuilder SetEvent(string eventName = "event")
        {
            ParameterList.SetParameterValue("event", eventName);
            return this;
        }

        public ParameterListBuilder SetKey(string key = "key")
        {
            ParameterList.SetParameterValue("key", key);
            return this;
        }

        public ParameterListBuilder SetLabel(string label = "Sample text")
        {
            ParameterList.SetParameterValue("label", label);
            return this;
        }

        public ParameterListBuilder SetPersistent(bool persistent = false)
        {
            ParameterList.SetParameterValue("persistent", persistent);
            return this;
        }

        public ParameterListBuilder SetStyle(ParameterList parameterList)
        {
            ParameterList.SetParameterValue("style", parameterList);
            return this;
        }

        public ParameterListBuilder SetText(string text = "Sample text")
        {
            ParameterList.SetParameterValue("text", text);
            return this;
        }

        public ParameterListBuilder SetSubmit(ParameterList parameterList)
        {
            ParameterList.SetParameterValue("submit", parameterList);
            return this;
        }

        public static implicit operator ParameterList(ParameterListBuilder builder) => builder.ParameterList;
        #endregion
    }
}
