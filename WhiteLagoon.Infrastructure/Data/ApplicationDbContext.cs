﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Villa> Villas { get; set; }
    public DbSet<VillaNumber> VillaNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Villa>().HasData(
            new Villa 
            {
                Id = 1,
                Name = "Royal Villa",
                Description = "Description of Royal Villa",
                ImageUrl = "https://placehold.co/600x400",
                Occupancy = 4,
                Price = 200,
                Squarefeet = 550
            },new Villa 
            {
                Id = 2,
                Name = "Premium Pool Villa",
                Description = "Description of Premium Pool Villa",
                ImageUrl = "https://placehold.co/600x400",
                Occupancy = 4,
                Price = 300,
                Squarefeet = 580
            },new Villa 
            {
                Id = 3,
                Name = "Luxury Pool Villa",
                Description = "Description of Luxury Pool Villa",
                ImageUrl = "https://placehold.co/600x400",
                Occupancy = 3,
                Price = 400,
                Squarefeet = 720
            }
        );

        modelBuilder.Entity<VillaNumber>().HasData(
            new VillaNumber
            { 
                Villa_Number = 101,
                VillaId = 1,
            },
            new VillaNumber
            { 
                Villa_Number = 102,
                VillaId = 1,
            },
            new VillaNumber
            { 
                Villa_Number = 103,
                VillaId = 1,
            },
            new VillaNumber
            { 
                Villa_Number = 104,
                VillaId = 1,
            },
            new VillaNumber
            {
                Villa_Number = 201,
                VillaId = 2,
            },
            new VillaNumber
            {
                Villa_Number = 202,
                VillaId = 2,
            },
            new VillaNumber
            {
                Villa_Number = 203,
                VillaId = 2,
            },
            new VillaNumber
            {
                Villa_Number = 301,
                VillaId = 3,
            },
            new VillaNumber
            {
                Villa_Number = 302,
                VillaId = 3,
            }
        );
    }
}
