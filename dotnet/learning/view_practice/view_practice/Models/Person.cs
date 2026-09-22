namespace view_practice.Models
{
    public class Person
    {
        public string? name { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public Gender personGender { get; set; }
    }

    public enum Gender
    {
        Male, Female, Other
    }
}