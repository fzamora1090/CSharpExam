using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventsCenter.Models
{
    public class Join
    {
        [Key]
        public int JoinId { get;set; }
        //FK
        public int EventId { get;set; }
        //FK
        public int UserId { get;set; }
        public bool AlreadyJoined { get;set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //Nav Props
        public User Guest { get;set; }
        public Event Event { get; set; }
    }
}