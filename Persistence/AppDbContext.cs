using Microsoft.EntityFrameworkCore;
using voting_api.Domain;

namespace voting_api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Options)
                .WithOne()
                .HasForeignKey(o => o.PollId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Votes)
                .WithOne()
                .HasForeignKey(v => v.PollId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Option>()
                .HasMany<Vote>()
                .WithOne()
                .HasForeignKey(v => v.OptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}