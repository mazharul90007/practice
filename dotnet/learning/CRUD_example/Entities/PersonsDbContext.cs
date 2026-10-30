using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class PersonsDbContext : DbContext
    {
        public PersonsDbContext(DbContextOptions<PersonsDbContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seed to Countries
            string countriesJson = System.IO.File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "country.json"));
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson) ?? new List<Country>();

            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(new Country()
                {
                    CountryId = country.CountryId,
                    CountryName = country.CountryName
                });
            }

            //Seed to Person
            string personsJson = System.IO.File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "persons.json"));
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson) ?? new List<Person>();

            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);

                //Fluent API
                modelBuilder.Entity<Person>().Property(temp => temp.TIN)
                .HasColumnName("TaxIdentificationNumber")
                .HasColumnType("varchar(15)")
                .HasDefaultValue("ABC45679");

                // modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();
            }

        }

        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }
    }
}