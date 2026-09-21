using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using my_first_project.CustomValidators;

namespace my_first_project.Models
{
    public class Person: IValidatableObject
    {
        [Required(ErrorMessage = "{0} can't be empty or null")]

        [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} need to be in {2} and {1} character")]
        [RegularExpression("^[A-Za-z .]+$", ErrorMessage = "{0} should contains only alphabet, space and .")]
        [Display(Name = "Person Name")]
        public string PersonName {get; set;} = string.Empty;
        [Required(ErrorMessage = "{0} can't be blank")]
        [EmailAddress(ErrorMessage = "{0} should be a proper email address")]
        public string Email {get; set;} = string.Empty;

        [Phone(ErrorMessage = "{0} should be a valid phone number")]
        public string? Phone {get; set;}

        [Required(ErrorMessage = "{0} can't be blank")]
        [Display(Name = "Password")]
        public string Password {get; set;} = string.Empty;

        [Required(ErrorMessage = "{0} can't be blank")]
        [Compare("Password", ErrorMessage ="{0} and {1} do not match")]
        [Display(Name = "Re-enter Password")]
        public string ConfirmPassword {get; set;} = string.Empty;

        [Range(18, 100, ErrorMessage = "{0} should be between {1} and {2} otherwise you are not a voter")]
        public int? Age {get; set;}

        public DateTime DateOfBirth {get; set;}
        public int? Price {get; set;}

        //====================================
        public DateTime FromDate {get; set;}

        [DateRangeValidator("FromDate", ErrorMessage = "'From Date' should be older than or equal to 'To Date'")]
        public DateTime ToDate {get; set;}

        public List<string?> Tags {get; set;} = new List<string?>();

        public override string ToString()
        {
            return $"Person object - Person name: {PersonName}, Email: {Email}, Phone: {Phone}, Age: {Age}, From: {FromDate}, To: {ToDate}, Tags: {string.Join(", ", Tags)}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Price.HasValue == false){
                yield return new ValidationResult("Price should be supplied", new []{nameof(Price)});
            }
        }
    }
}