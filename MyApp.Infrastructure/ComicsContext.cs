namespace MyApp.Infrastructure;

public class ComicsContext : DbContext
{
    public ComicsContext(DbContextOptions<ComicsContext> options)
        : base(options)
    {
    }

    public DbSet<City> Cities => Set<City>();
    public DbSet<Character> Characters => Set<Character>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
                    .Property(c => c.Name)
                    .HasMaxLength(50);

        modelBuilder.Entity<Power>()
                    .Property(c => c.Name)
                    .HasMaxLength(50);

        modelBuilder.Entity<Character>()
                    .Property(c => c.GivenName)
                    .HasMaxLength(50);

        modelBuilder.Entity<Character>()
                    .Property(c => c.Surname)
                    .HasMaxLength(50);

        modelBuilder.Entity<Character>()
                    .Property(c => c.AlterEgo)
                    .HasMaxLength(50);

        modelBuilder.Entity<Character>()
                    .Property(c => c.Occupation)
                    .HasMaxLength(50);

        modelBuilder.Entity<Character>()
                    .Property(e => e.Gender)
                    .HasConversion(new EnumToStringConverter<Gender>(new ConverterMappingHints(size: 50)));
    }
}
