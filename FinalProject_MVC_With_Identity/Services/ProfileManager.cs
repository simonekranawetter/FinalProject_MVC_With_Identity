using FinalProject_MVC_With_Identity.Models;
using FinalProject_MVC_With_Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_MVC_With_Identity.Services
{
    public interface IProfileManager
    {
        Task<ProfileResult> CreateAsync(IdentityUser user, UserProfile profile);
        Task<UserProfile> ReadAsync(string userId);
        Task<string> DisplayNameAsync(string userId);
        Task<string> DisplayRoleAsync(string userId);
        Task UpdateAsync(UserProfile userProfile);
    }
    public class ProfileManager : IProfileManager
    {
        private readonly ApplicationDbContext _context;

        public ProfileManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileResult> CreateAsync(IdentityUser user, UserProfile profile)
        {
            if(await _context.Users.AnyAsync(x => x.Id == user.Id))
            {
                var profileEntity = new ProfileEntity
                {
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    StreetName = profile.StreetName,
                    PostalCode = profile.PostalCode,
                    City = profile.City,
                    ProfileImage = profile.ProfileImageUrl,
                    UserId = user.Id
                };
                
                _context.Profiles.Add(profileEntity);
                await _context.SaveChangesAsync();

                return new ProfileResult { Succeeded = true };
            }
            return new ProfileResult { Succeeded = false };
        }
        public async Task<UserProfile> ReadAsync(string userId)
        {
            var profile = new UserProfile();
            var profileEntity = await _context.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId);
            if(profileEntity != null)
            {
                profile.Id = userId;
                profile.FirstName = profileEntity.FirstName;
                profile.LastName = profileEntity.LastName;
                profile.Email = profileEntity.User.Email;
                profile.StreetName = profileEntity.StreetName;
                profile.PostalCode = profileEntity.PostalCode;
                profile.City = profileEntity.City;
                profile.ProfileImageUrl = profileEntity.ProfileImage;
            }
            return profile;
        }
        public async Task<string> DisplayNameAsync(string userId)
        {
            var result = await ReadAsync(userId);
            return $"{result.FirstName} {result.LastName}";
        }
        public async Task<string> DisplayRoleAsync(string userId)
        {
            var result = await ReadAsync(userId);
            return $"{result.Role}";
        }

        public async Task UpdateAsync(UserProfile userProfile)
        {
            var profileEntity = await _context.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userProfile.Id);
            profileEntity.FirstName = userProfile.FirstName;
            profileEntity.LastName = userProfile.LastName;
            profileEntity.User.Email = userProfile.Email;
            profileEntity.StreetName = userProfile.StreetName;
            profileEntity.PostalCode = userProfile.PostalCode;
            profileEntity.City = userProfile.City;
            profileEntity.ProfileImage = userProfile.ProfileImageUrl;

            await _context.SaveChangesAsync();
        }
    }




    public class ProfileResult
    {
        public bool Succeeded { get; set; } = false;
    }

}
