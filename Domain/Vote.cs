namespace voting_api.Domain
{
    public class Vote
    {
        public Guid PollId { get; set; }
        public Guid OptionId { get; set; }
        public DateTime SubmittedAt { get; set; }

        public Vote(Guid pollId, Guid optionId)
        {
            PollId = pollId;
            OptionId = optionId;
            SubmittedAt = DateTime.UtcNow;
        }

    }
}