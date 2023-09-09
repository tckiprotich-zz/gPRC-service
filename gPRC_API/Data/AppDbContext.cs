using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using gPRC_API.Models;

namespace gPRC_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        // Dataset
        public DbSet<ToDoItems> ToDoItems => Set<ToDoItems>();
        
    }
}