using FinalProject_MVC_With_Identity.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_MVC_With_Identity
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ProfileEntity> Profiles => Set<ProfileEntity>();

    }
}
