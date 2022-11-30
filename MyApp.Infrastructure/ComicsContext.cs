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
                    .HasIndex(city => city.Name).IsUnique();

        modelBuilder.Entity<CityEntity>()
                    .ToTable("Cities");

        modelBuilder.Entity<CityEntity>().Property(city => city.Name)
                    .HasMaxLength(50);

        modelBuilder.Entity<PowerEntity>()
                    .ToTable("Powers");

        modelBuilder.Entity<CharacterEntity>()
                    .HasMany(left => left.Powers)
                    .WithMany(right => right.Characters)
                    .UsingEntity(join => join.ToTable("CharacterPowers"));

        modelBuilder.Entity<PowerEntity>()
                    .HasIndex(power => power.Name).IsUnique();

        modelBuilder.Entity<PowerEntity>()
                    .Property(power => power.Name)
                    .HasMaxLength(50);

        modelBuilder.Entity<CharacterEntity>()
                    .ToTable("Characters");

        modelBuilder.Entity<CharacterEntity>()
                    .Property(character => character.AlterEgo)
                    .HasMaxLength(50);

        modelBuilder.Entity<CharacterEntity>()
                    .Property(character => character.GivenName)
                    .HasMaxLength(50);

        modelBuilder.Entity<CharacterEntity>()
                    .Property(character => character.Surname)
                    .HasMaxLength(50);

        modelBuilder.Entity<CharacterEntity>()
                    .Property(character => character.Occupation)
                    .HasMaxLength(50);

        modelBuilder.Entity<CharacterEntity>()
                    .Property(character => character.Gender)
                    .HasConversion(new EnumToStringConverter<Gender>(new ConverterMappingHints(size: 50)));

        modelBuilder.Entity<CharacterEntity>()
                    .Property(character => character.ImageUrl)
                    .HasMaxLength(250);
    }
}
