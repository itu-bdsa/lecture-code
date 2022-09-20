using Microsoft.EntityFrameworkCore;

namespace MyApp.Infrastructure;

public partial class ComicsContext : DbContext
    {
        public ComicsContext(DbContextOptions<ComicsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities => Set<City>();
        public virtual DbSet<Character> Characters => Set<Character>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>().Property(c => c.AlterEgo).HasMaxLength(50);
        }
    }