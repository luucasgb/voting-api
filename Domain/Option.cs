namespace voting_api.Domain
{
    public class Option
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int VoteCount { get; set; }

        public Option(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            VoteCount = 0;
        }
    }
}