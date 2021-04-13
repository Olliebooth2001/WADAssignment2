using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WAD.Models
{
    public class ApplicationDbContext: DbContext
    {
        
        public ApplicationDbContext(DbContextOptions < ApplicationDbContext > options)
            : base(options)
        {
       
        }

        public DbSet<Gig> Gigs { get; set; }
    }
}
