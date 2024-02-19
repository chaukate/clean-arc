using App.Application.Common.Exceptions;
using App.Application.Common.Interfaces;
using App.Application.Common.Models;
using App.Domain.Entities;
using App.Domain.Enumerations;
using App.Domain.Interfaces;
using App.Infrastructure.Common.Options;
using App.Infrastructure.Persistence.Identity;
using App.Infrastructure.Persistence.Initializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace App.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions;
        public IdentityService(UserManager<User> userManager, IOptions<JwtOptions> options)
        {
            _userManager = userManager;
            _jwtOptions = options.Value;
        }

        public async Task<AuthResponseModel> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(i => i.ProfileRef)
                                               .Include(i => i.Roles)
                                               .Where(w => w.NormalizedEmail == email.ToUpper())
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new BadRequestException("Invalid email or password.");

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
                throw new BadRequestException("Invalid email or password.");

            var token = GenerateToken(user);

            var response = new AuthResponseModel
            {
                FullName = $"{user.ProfileRef.FirstName} {user.ProfileRef.LastName}",
                Token = token,
                Area = GetArea(user.Roles)
            };
            return response;
        }

        private Area GetArea(ICollection<UserRole> userRoles)
        {
            var adminRoles = new[] { RoleInitializer.SUPER_ADMIN, RoleInitializer.ADMIN };
            if (userRoles.Any(a => adminRoles.Contains(a.RoleId)))
                return Area.Admin;
            else if (userRoles.Any(a => a.RoleId == RoleInitializer.CLIENT))
                return Area.Client;

            return Area.None;
        }

        public async Task<IUser> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.Users.Include(i => i.ProfileRef)
                                                 .Where(w => w.NormalizedEmail == email.ToUpper())
                                                 .FirstOrDefaultAsync(cancellationToken);
            return dbUser;
        }

        public async Task<int> CreateCustomerAsync(string email, string currentUserEmail, string clientUrl, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                throw new BadRequestException("Email address already registered.");

            user = new User();
            user.UserName = email;
            user.Email = email;
            user.LastUpdatedAt = DateTimeOffset.UtcNow;
            user.LastUpdatedBy = currentUserEmail;
            user.Roles = new List<UserRole> { new UserRole { RoleId = RoleInitializer.CLIENT } };

            user.EventActivity = UserEventActivity.Invite;
            user.Link = clientUrl;

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
                return user.Id;

            var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
            throw new BadRequestException(errorMessage);
        }

        public async Task<bool> AcceptCustomerInvitationAsync(UserProfile userProfile, string email, string password, string token, string clientUrl, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(i => i.ProfileRef)
                                               .Where(w => w.NormalizedEmail == email.ToUpper())
                                               .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
                throw new NotFoundException();

            var codeEncodedBytes = WebEncoders.Base64UrlDecode(token);
            var codeEncoded = Encoding.UTF8.GetString(codeEncodedBytes);

            user.ProfileRef = userProfile;
            user.EventActivity = UserEventActivity.AcceptInvitation;
            user.Link = $"{clientUrl}/login";
            user.EmailConfirmed = true;
            user.IsActive = true;
            user.LastUpdatedAt = DateTimeOffset.UtcNow;
            user.LastUpdatedBy = email;

            var result = await _userManager.ConfirmEmailAsync(user, codeEncoded);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }

            // Always add password after the email has been confirmed.
            await _userManager.AddPasswordAsync(user, password);

            return result.Succeeded;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(IUser user)
        {
            var dbUser = (User)user;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync((User)dbUser);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

            return codeEncoded;
        }

        public async Task<bool> RequestChangePasswordAsync(string email, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.Users.Include(i => i.ProfileRef)
                                                 .Where(w => w.NormalizedEmail == email.ToUpper() &&
                                                             w.IsActive == true &&
                                                             w.EmailConfirmed == true)
                                                 .FirstOrDefaultAsync(cancellationToken);
            if (dbUser == null)
                throw new NotFoundException();

            var token = await _userManager.GeneratePasswordResetTokenAsync(dbUser);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = dbUser.Email;
            dbUser.Link = $"{clientUrl}/accounts/reset-password?firstname={dbUser.ProfileRef.FirstName}&lastname={dbUser.ProfileRef.LastName}&email={dbUser.Email}&token={codeEncoded}";
            dbUser.EventActivity = UserEventActivity.RequestChangePassword;

            await _userManager.UpdateAsync(dbUser);
            return true;
        }

        public async Task<bool> ChangePasswordAsync(string email, string token, string password, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.Users.Include(i => i.ProfileRef)
                                                 .Where(w => w.NormalizedEmail == email.ToUpper() &&
                                                             w.IsActive == true &&
                                                             w.EmailConfirmed == true)
                                                 .FirstOrDefaultAsync(cancellationToken);
            if (dbUser == null)
                throw new NotFoundException();

            var codeEncodedBytes = WebEncoders.Base64UrlDecode(token);
            var codeEncoded = Encoding.UTF8.GetString(codeEncodedBytes);

            dbUser.EventActivity = UserEventActivity.ChangePassword;
            dbUser.Link = $"{clientUrl}/login";
            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = dbUser.Email;

            var result = await _userManager.ResetPasswordAsync(dbUser, codeEncoded, password);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return result.Succeeded;
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = Encoding.UTF8.GetBytes(_jwtOptions.Key);
            var securityKey = new SymmetricSecurityKey(secretKey);
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"),
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Name, $"{user.ProfileRef.FirstName} {user.ProfileRef.LastName}"),
                new Claim(ClaimTypes.Email, $"{user.Email}")
            };

            var claimsIdentity = new ClaimsIdentity(claims);
            claimsIdentity.AddClaims(user.Roles.Select(s => new Claim(ClaimTypes.Role, $"{s.RoleId}")));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireInMinutes),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512),
                Issuer = _jwtOptions.Issuer
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
