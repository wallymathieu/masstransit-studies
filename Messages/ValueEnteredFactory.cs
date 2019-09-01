namespace MassTransitStudies.Messages
{
    public static class ValueEnteredFactory
    {
        private class ValueEnteredMessage : ValueEntered
        {
            public string Value { get; set; }
        }
        public static ValueEntered Create(string value)
        {
            return new ValueEnteredMessage { Value = value };
        }
    }
}
