using FinalProject_MVC_With_Identity.Models;
using FinalProject_MVC_With_Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject_MVC_With_Identity.Services
{
    public interface IProfileManager
    {
        Task<ProfileResult> CreateAsync(IdentityUser user, UserProfile profile);
        Task<UserProfile> ReadAsync(ClaimsPrincipal claimsPrincipal);
        Task<string> DisplayNameAsync(ClaimsPrincipal claimsPrincipal);
        Task<string> DisplayRoleAsync(ClaimsPrincipal claimsPrincipal);
        Task UpdateAsync(UserProfile userProfile);
    }
    public class ProfileManager : IProfileManager
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileManager(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<UserProfile> ReadAsync(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst("UserId").Value;
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var rolesForUsers = await _userManager.GetRolesAsync(user);
            var role = rolesForUsers.FirstOrDefault();

            var profile = new UserProfile();
            var profileEntity = await _context.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId);
            if(profileEntity != null)
            {
                profile.UserId = userId;
                profile.FirstName = profileEntity.FirstName;
                profile.LastName = profileEntity.LastName;
                profile.Email = profileEntity.User.Email;
                profile.StreetName = profileEntity.StreetName;
                profile.PostalCode = profileEntity.PostalCode;
                profile.City = profileEntity.City;
                profile.ProfileImageUrl = profileEntity.ProfileImage;
                profile.Role = role;
            }
            return profile;
        }
        public async Task<string> DisplayNameAsync(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst("UserId").Value;
            var result = await ReadAsync(claimsPrincipal);
            return $"{result.FirstName} {result.LastName}";
        }
        public async Task<string> DisplayRoleAsync(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst("UserId").Value;
            var result = await ReadAsync(claimsPrincipal);
            return $"{result.Role}";
        }

        public async Task UpdateAsync(UserProfile userProfile)
        {
            var profileEntity = await _context.Profiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userProfile.UserId);
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
