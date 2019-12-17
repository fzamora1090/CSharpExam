using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventsCenter.Models
{
    public class EventsCenterDBContext : DbContext
    {
        public EventsCenterDBContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get; set;} 
        public DbSet<Event> Events {get; set;} 
        public DbSet<Join> Joins {get; set;} 
    }
}