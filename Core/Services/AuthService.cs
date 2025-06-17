using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using ServicesAbstractions;
using Shared;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using System.ComponentModel.DataAnnotations;
using ValidationException = Domain.Exceptions.ValidationException;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.OrdersModels;

namespace Services
{
    public class AuthService(IMapper mapper, UserManager<AppUser> userManager, IOptions<JwtOptions> options) : IAuthService
    {

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnAuthorizedException();

            var flag = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!flag) throw new UnAuthorizedException();

            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user),
            };

        }


        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            // ✅ في حالة أن الفرونت مش بيرسل UserName
            if (string.IsNullOrWhiteSpace(registerDto.UserName))
                registerDto.UserName = registerDto.Email;

            // 🔁 تحقق من التكرار
            if (await CheckEmailExistsAsync(registerDto.Email))
                throw new DuplicatedEmailBadRequestException(registerDto.Email);

            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName ?? registerDto.Email.Split('@')[0], // 👈 عالج نقص الـ UserName هنا
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description);
                throw new ValidationException(errors);
            }

            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user),
            };
        }



        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<UserResultDto> GetCurrentUserAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) throw new UserNotFoundException(email);

            return new UserResultDto()
            { 
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user),
            };
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);

            if (user is null) throw new UserNotFoundException(email);

            var result = mapper.Map<AddressDto>(user.Address);

            return result;

        }


        public async Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto address, string email)
        {
            var user = await userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                throw new UserNotFoundException(email);

          
            if (user.Address == null)
            {
              
                user.Address = mapper.Map<Address>(address);
            }
            else
            {
            
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;
            }

            await userManager.UpdateAsync(user);

            return address;
        }

        private async Task<string> GenerateJwtTokenAsync(AppUser user)
        {

            // Header 
            // Payload
            // Signature

            var JwtOptions = options.Value;

            var AuthClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                AuthClaim.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey));

            var token = new JwtSecurityToken
            (
                issuer: JwtOptions.Issuer,
                audience: JwtOptions.Audience,
                claims: AuthClaim,
                expires: DateTime.UtcNow.AddDays(JwtOptions.DurationInDays),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );

            // Token 

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}