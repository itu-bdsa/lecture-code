namespace MyApp.Infrastructure;

public sealed class ComicsContext : DbContext
{
    public DbSet<CityEntity> Cities => Set<CityEntity>();
    public DbSet<PowerEntity> Powers => Set<PowerEntity>();
    public DbSet<CharacterEntity> Characters => Set<CharacterEntity>();

    public ComicsContext(DbContextOptions<ComicsContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CityEntity>()
                    .HasIndex(c => c.Name).IsUnique();

        modelBuilder.Entity<CityEntity>()
            .ToTable("Cities");

        modelBuilder.Entity<CityEntity>().Property(c => c.Name)
                    .HasMaxLength(50);

        modelBuilder.Entity<PowerEntity>()
                    .ToTable("Powers");

        modelBuilder.Entity<PowerEntity>()
                    .HasIndex(c => c.Name).IsUnique();

        modelBuilder.Entity<PowerEntity>()
                    .Property(c => c.Name)
                    .HasMaxLength(50);

        modelBuilder.Entity<CharacterEntity>()
                    .ToTable("Characters");

        modelBuilder.Entity<CharacterEntity>()
                    .Property(c => c.AlterEgo)
                    .HasMaxLength(50);

        modelBuilder.Entity<CharacterEntity>()
                    .Property(c => c.GivenName)
                    .HasMaxLength(50);

        modelBuilder.Entity<CharacterEntity>()
                    .Property(c => c.Surname)
                    .HasMaxLength(50);

        modelBuilder.Entity<CharacterEntity>()
                    .Property(c => c.Occupation)
                    .HasMaxLength(50);

        modelBuilder.Entity<CharacterEntity>()
                    .Property(e => e.Gender)
                    .HasConversion(new EnumToStringConverter<Gender>(new ConverterMappingHints(size: 50)));

        modelBuilder.Entity<CharacterEntity>()
                    .Property(c => c.ImageUrl)
                    .HasMaxLength(250);

    }
}
