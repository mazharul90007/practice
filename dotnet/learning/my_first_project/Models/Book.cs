using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace my_first_project.Models
{
    public class Book
    {
        [Required]
        public int BookId {get; set;}
        public string? Author {get; set;}

        public override string ToString()
        {
            return $"Book object - BookId: {BookId}, Author: {Author}";
        }
    }
}