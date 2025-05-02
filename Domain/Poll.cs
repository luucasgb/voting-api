using System.ComponentModel.DataAnnotations;

namespace voting_api.Domain
{
    public class Poll
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsAnonymous{ get; set; }

        // Navigation properties
        public ICollection<Option> Options { get; set; } = new List<Option>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();

        public Poll()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsAnonymous = true;
        }

        public Poll(string title, string description, DateTime startDate, DateTime endDate, bool isAnonymous)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = true;
            IsAnonymous = isAnonymous;
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
