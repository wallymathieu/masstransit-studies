namespace MassTransitStudies.Messages
{
    public interface IValueEntered
    {
        string Value { get; }
    }
    public static class ValueEntered
    {
        private class ValueEnteredMessage : IValueEntered
        {
            public string Value { get; set; }
        }
        public static IValueEntered Create(string value)
        {
            return new ValueEnteredMessage { Value = value };
        }
    }
}
