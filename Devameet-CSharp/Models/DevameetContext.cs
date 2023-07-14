﻿using Microsoft.EntityFrameworkCore;

namespace Devameet_CSharp.Models
{
    public class DevameetContext : DbContext
    {
        public DevameetContext(DbContextOptions<DevameetContext> option) : base(option)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}