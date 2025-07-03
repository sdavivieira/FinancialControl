using FinancialControl.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profile { get; set; }
    }

}
