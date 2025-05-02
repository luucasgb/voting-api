using System.ComponentModel.DataAnnotations;

namespace voting_api.Domain
{
    public class Option
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid PollId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Value { get; set; }
        public int VoteCount { get; set; }

        // Navigation property
        public Poll Poll { get; set; }

        public Option(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            VoteCount = 0;
        }
    }
}