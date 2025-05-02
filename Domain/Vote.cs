using System.ComponentModel.DataAnnotations;

namespace voting_api.Domain
{
    public class Vote
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PollId { get; set; }

        [Required]
        public Guid OptionId { get; set; }

        public DateTime SubmittedAt { get; set; }

        // Navigation properties
        public Poll Poll { get; set; }
        public Option Option { get; set; }
    }  
}