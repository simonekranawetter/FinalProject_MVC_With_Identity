using FinalProject_MVC_With_Identity.Models;
using FinalProject_MVC_With_Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace FinalProject_MVC_With_Identity.Services
{
    public interface IProfileManager
    {
        Task CreateAsync(IdentityUser user, UserProfile profile);
    }
    public class ProfileManager : IProfileManager
    {
        private readonly ApplicationDbContext _context;

        public ProfileManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(IdentityUser user, UserProfile profile)
        {
            var profileEntity = new ProfileEntity
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                StreetName = profile.StreetName,
                PostalCode = profile.PostalCode,
                City = profile.City,
                UserId = user.Id
            };
                
            _context.Profiles.Add(profileEntity);
            await _context.SaveChangesAsync();
        }
    }
}
