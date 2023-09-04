using IUstaProject.Models;
using Microsoft.EntityFrameworkCore;

namespace IUstaProject.Data
{
    public class WorkerDbContext:DbContext
    {
        public WorkerDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
