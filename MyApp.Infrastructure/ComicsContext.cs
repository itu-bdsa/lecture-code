namespace MyApp.Infrastructure;

public sealed class ComicsContext : DbContext
{
    public DbSet<City> Cities => Set<City>();
    public DbSet<Power> Powers => Set<Power>();
    public DbSet<Character> Characters => Set<Character>();

    public ComicsContext(DbContextOptions<ComicsContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
                    .HasIndex(c => c.Name).IsUnique();

        modelBuilder.Entity<City>().Property(c => c.Name)
                    .HasMaxLength(50);

        modelBuilder.Entity<Power>()
                    .HasIndex(c => c.Name).IsUnique();

        modelBuilder.Entity<Power>()
                    .Property(c => c.Name)
                    .HasMaxLength(50);

        modelBuilder.Entity<Character>()
                    .Property(c => c.AlterEgo)
                    .HasMaxLength(50);

        modelBuilder.Entity<Character>()
                    .Property(c => c.GivenName)
                    .HasMaxLength(50);

        modelBuilder.Entity<Character>()
                    .Property(c => c.Surname)
                    .HasMaxLength(50);

        modelBuilder.Entity<Character>()
                    .Property(c => c.Occupation)
                    .HasMaxLength(50);

        modelBuilder.Entity<Character>()
                    .Property(e => e.Gender)
                    .HasConversion(new EnumToStringConverter<Gender>(new ConverterMappingHints(size: 50)));

        modelBuilder.Entity<Character>()
                    .Property(c => c.ImageUrl)
                    .HasMaxLength(250);

    }
}
