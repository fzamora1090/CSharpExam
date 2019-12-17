using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventsCenter.Models
{
    public class User
    {
        [Key]  // denotes PK
        public int UserId { get; set; }

        [Required(ErrorMessage = "is required")]
        [MinLength(2, ErrorMessage = "Must be longer than 2 characters")]
        [Display(Name = "First Name")]
        public string FirstName {get; set;}

        [Required(ErrorMessage = "is required")]
        [MinLength(2, ErrorMessage = "Must be longer than 2 characters")]
        [Display(Name = "Last Name")]
        public string LastName {get; set;}

        [Display(Name = "Email")]
        [Required(ErrorMessage = "is required!")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage= "is required")]
        [MinLength(8, ErrorMessage = "must be at least 8 characters")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password must containe 1 letter, 1 number and 1 special character!")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage= "doesn't match!")]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }
        public string FullName()
        {
            return FirstName + " " + LastName;
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


// navigation property for the relationship
        public List<Event> Events { get; set; } // 1 user: N Activities relationship
        public List<Join> RegisteredEvents { get;set; } //1 user : to M joins relationship

    }
}