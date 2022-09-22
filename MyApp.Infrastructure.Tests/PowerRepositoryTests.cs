using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyApp.Core;

namespace MyApp.Infrastructure.Tests;

public sealed class PowerRepositoryTests : IDisposable
{
    private readonly ComicsContext _context;
    private readonly PowerRepository _repository;

    public PowerRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ComicsContext>();
        builder.UseSqlite(connection);
        var context = new ComicsContext(builder.Options);
        context.Database.EnsureCreated();
        var flight = new Power("flight") { Id = 1 };
        var invulnerability = new Power("invulnerability") { Id = 2 };
        var combatStrategy = new Power("combat strategy") { Id = 3 };
        context.Powers.AddRange(flight, invulnerability, combatStrategy);
        context.Characters.Add(new Character { Id = 1, AlterEgo = "Superman", Powers = new[] { flight, invulnerability } });
        context.SaveChanges();

        _context = context;
        _repository = new PowerRepository(_context);
    }

    [Fact]
    public void Create_given_Power_returns_Created_with_Power()
    {
        var (response, created) = _repository.Create(new PowerCreateDto("super speed"));

        response.Should().Be(Created);

        created.Should().Be(new PowerDto(4, "super speed"));
    }

    [Fact]
    public void Create_given_existing_Power_returns_Conflict_with_existing_Power()
    {
        var (response, power) = _repository.Create(new PowerCreateDto("invulnerability"));

        response.Should().Be(Conflict);

        power.Should().Be(new PowerDto(2, "invulnerability"));
    }

    [Fact]
    public void Find_given_non_existing_id_returns_null() => _repository.Find(42).Should().BeNull();

    [Fact]
    public void Find_given_existing_id_returns_power() => _repository.Find(2).Should().Be(new PowerDto(2, "invulnerability"));

    [Fact]
    public void Read_returns_all_powers() => _repository.Read().Should().BeEquivalentTo(new[] { new PowerDto(1, "flight"), new PowerDto(2, "invulnerability"), new PowerDto(3, "combat strategy") });

    [Fact]
    public void Update_given_non_existing_Power_returns_NotFound() => _repository.Update(new PowerDto(42, "brilliant deductive skill")).Should().Be(NotFound);

    [Fact]
    public void Update_given_existing_name_returns_Conflict_and_does_not_update()
    {
        var response = _repository.Update(new PowerDto(2, "flight"));

        response.Should().Be(Conflict);

        var entity = _context.Powers.Find(2)!;

        entity.Name.Should().Be("invulnerability");
    }

    [Fact]
    public void Update_updates_and_returns_Updated()
    {
        var response = _repository.Update(new PowerDto(2, "brilliant deductive skill"));

        response.Should().Be(Updated);

        var entity = _context.Powers.Find(2)!;

        entity.Name.Should().Be("brilliant deductive skill");
    }

    [Fact]
    public void Delete_given_non_existing_Id_returns_NotFound() => _repository.Delete(42).Should().Be(NotFound);

    [Fact]
    public void Delete_deletes_and_returns_Deleted()
    {
        var response = _repository.Delete(3);

        response.Should().Be(Deleted);

        var entity = _context.Powers.Find(3);

        entity.Should().BeNull();
    }

    [Fact]
    public void Delete_given_existing_Power_with_Characters_returns_Conflict_and_does_not_delete()
    {
        var response = _repository.Delete(1);

        response.Should().Be(Conflict);

        _context.Powers.Find(1).Should().NotBeNull();
    }


    public void Dispose()
    {
        _context.Dispose();
    }
}
