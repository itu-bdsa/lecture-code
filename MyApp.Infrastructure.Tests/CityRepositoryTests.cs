using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyApp.Core;

namespace MyApp.Infrastructure.Tests;

public sealed class CityRepositoryTests : IDisposable
{
    private readonly ComicsContext _context;
    private readonly CityRepository _repository;

    public CityRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ComicsContext>();
        builder.UseSqlite(connection);
        var context = new ComicsContext(builder.Options);
        context.Database.EnsureCreated();
        context.Cities.AddRange(new City("Metropolis") { Id = 1 }, new City("Gotham City") { Id = 2 });
        context.Characters.Add(new Character { Id = 1, AlterEgo = "Superman", CityId = 1 });
        context.SaveChanges();

        _context = context;
        _repository = new CityRepository(_context);
    }

    [Fact]
    public void Create_given_City_returns_Created_with_City()
    {
        var (response, created) = _repository.Create(new CityCreateDto("Central City"));

        response.Should().Be(Created);

        created.Should().Be(new CityDto(3, "Central City"));
    }

    [Fact]
    public void Create_given_existing_City_returns_Conflict_with_existing_City()
    {
        var (response, city) = _repository.Create(new CityCreateDto("Gotham City"));

        response.Should().Be(Conflict);

        city.Should().Be(new CityDto(2, "Gotham City"));
    }

    [Fact]
    public void Find_given_non_existing_id_returns_null() => _repository.Find(42).Should().BeNull();

    [Fact]
    public void Find_given_existing_id_returns_city() => _repository.Find(2).Should().Be(new CityDto(2, "Gotham City"));

    [Fact]
    public void Read_returns_all_cities() => _repository.Read().Should().BeEquivalentTo(new[] { new CityDto(1, "Metropolis"), new CityDto(2, "Gotham City") });

    [Fact]
    public void Update_given_non_existing_City_returns_NotFound() => _repository.Update(new CityDto(42, "Central City")).Should().Be(NotFound);

    [Fact]
    public void Update_given_existing_name_returns_Conflict_and_does_not_update()
    {
        var response = _repository.Update(new CityDto(2, "Metropolis"));

        response.Should().Be(Conflict);

        var entity = _context.Cities.Find(2)!;

        entity.Name.Should().Be("Gotham City");
    }

    [Fact]
    public void Update_updates_and_returns_Updated()
    {
        var response = _repository.Update(new CityDto(2, "Central City"));

        response.Should().Be(Updated);

        var entity = _context.Cities.Find(2)!;

        entity.Name.Should().Be("Central City");
    }

    [Fact]
    public void Delete_given_non_existing_Id_returns_NotFound() => _repository.Delete(42).Should().Be(NotFound);

    [Fact]
    public void Delete_deletes_and_returns_Deleted()
    {
        var response = _repository.Delete(2);

        response.Should().Be(Deleted);

        var entity = _context.Cities.Find(2);

        entity.Should().BeNull();
    }

    [Fact]
    public void Delete_given_existing_City_with_Characters_returns_Conflict_and_does_not_delete()
    {
        var response = _repository.Delete(1);

        response.Should().Be(Conflict);

        _context.Cities.Find(1).Should().NotBeNull();
    }


    public void Dispose()
    {
        _context.Dispose();
    }
}
