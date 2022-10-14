using Authentifizierung_Service_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentifizierung_Service_API.Data
{
public class AuthContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
