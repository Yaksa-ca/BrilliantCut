﻿
namespace EPiTube.FasetFilter.Core
{
    public interface IFilterOptionModel//<out TValue>
    {
        string Id { get; }
        string Text { get; }
        object Value { get; }
        object DefaultValue { get; }
        int Count { get; }
    }

    public class FilterOptionModel : IFilterOptionModel
    {
        public FilterOptionModel(string id, string text, object value, object defaultValue, int count)
        {
            Id = id;
            Text = text;
            Value = value;
            DefaultValue = defaultValue;
            Count = count;
        }

        public string Id { get; private set; }
        public string Text { get; private set; }
        public object Value { get; private set; }
        public object DefaultValue { get; private set; }
        public int Count { get; private set; }
    }
}
