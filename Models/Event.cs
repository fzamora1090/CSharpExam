using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventsCenter.Models
{
    public class Event
    {
        [Key]
        public int EventId { get;set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "is required!")]
        [MinLength(2, ErrorMessage = "Must be longer than 2 characters")]
        public string Title { get;set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "is required!")]
        [MinLength(2, ErrorMessage = "Must be longer than 2 characters")]        
        public string Description { get;set; }

        [Display(Name = "Time")]
        public string Time { get;set; }

        [Display(Name = "Duration")]
        public string Duration { get;set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Must enter a date fr this activity!")]
        [FutureDate(ErrorMessage = "Date should be in the future.")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get;set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        //Navigation Props
        public int UserId { get; set; }
        public User Coordinator { get; set; } // 1 user related to each Activity! 1-M relationship -- navigation property
        public List<Join> Registrants { get;set; } //1 User : to M Joins relationship
    }
}