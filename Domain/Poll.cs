namespace voting_api.Domain
{
    public class Poll
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<Option> Options { get; set; } = new List<Option>();
        public List<Vote> Votes { get; set; } = new List<Vote>();
        public Poll()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
        public Poll(string title, string description, DateTime startDate, DateTime endDate)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = true;
        }
        public void AddOption(Option option)
        {
            Options.Add(option);
        }
        public void RemoveOption(Option option)
        {
            Options.Remove(option);
        }
    }
}
