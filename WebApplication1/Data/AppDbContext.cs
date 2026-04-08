using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using New_PRO.Models;

namespace New_PRO.Data
{
      public class AppDbContext : IdentityDbContext<UserRegister>
        {
            public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options) { }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                builder.Entity<UserRegister>().ToTable("AspNetUsers");
            }
        }


    
}
