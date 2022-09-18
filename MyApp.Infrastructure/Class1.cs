// namespace MyApp.Infrastructure;

// public enum Gender
// {
//     Female,
//     Male,
//     Other
// }

// public class Character
// {
//     public int Id { get; set; }

//     [StringLength(50)]
//     public string? GivenName { get; set; }

//     [StringLength(50)]
//     public string? Surname { get; set; }

//     [StringLength(50)]
//     public string? AlterEgo { get; set; }

//     [Range(1900, 2100)]
//     public int? FirstAppearance { get; set; }

//     [StringLength(50)]
//     public string? Occupation { get; set; }

//     public int? CityId { get; set; }

//     public City? City { get; set; }

//     public Gender Gender { get; set; }

//     [StringLength(250)]
//     [Url]
//     public string? ImageUrl { get; set; }

//     public ICollection<Power> Powers { get; set; }

//     public Character()
//     {
//         Powers = new HashSet<Power>();
//     }
// }

// public class City
// {
//     public int Id { get; set; }

//     [StringLength(50)]
//     public string Name { get; set; }

//     public ICollection<Character> Characters { get; set; }

//     public City(string name)
//     {
//         Name = name;
//         Characters = new HashSet<Character>();
//     }
// }

// public class Power
// {
//     public int Id { get; set; }

//     [StringLength(50)]
//     public string Name { get; set; }

//     public ICollection<Character> Characters { get; set; }

//     public Power(string name)
//     {
//         Name = name;
//         Characters = new HashSet<Character>();
//     }
// }
