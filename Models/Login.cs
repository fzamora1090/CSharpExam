using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventsCenter.Models
{
    [NotMapped] //dont add tabel to DB
    public class Login
    {
        [Required(ErrorMessage = "is required!")]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage= "is required")]
        [MinLength(8, ErrorMessage = "must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}