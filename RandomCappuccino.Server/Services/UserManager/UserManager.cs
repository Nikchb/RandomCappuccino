using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RandomCappuccino.Server.Data;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.UserManager.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.UserManager
{
    public class UserManager : ServiceBase<UserDTO>, IUserManager
    {
        private readonly DataBaseContext context;
        private readonly IMapper mapper;

        public UserManager(DataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ServiceContentResponse<UserDTO>> CreateUser(CreateUserDTO model)
        {
            if(context.Users.FirstOrDefault(v=>v.Email == model.Email) != null)
            {
                return Decline("This email is already used");
            }

            model.Password = HashPassword(model.Password);
            
            var user = mapper.Map<User>(model);

            try
            {
                await context.Users.AddAsync(user);
                await context.UserRoles.AddRangeAsync(model.Roles.Select(v => new UserRole { UserId = user.Id, Role = v }));
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline("User creation is failed");
            }

            return Accept(mapper.Map<UserDTO>(user));
        }

        public async Task<ServiceContentResponse<UserDTO>> GetUserInfo(string userId)
        {
            var user = await context.Users.FindAsync(userId);            
            if(user == null)
            {
                return Decline("User is not found");
            } 

            return Accept(mapper.Map<UserDTO>(user));
        }

        public async Task<ServiceContentResponse<UserDTO>> UpdateUserInfo(string userId, UpdateUserInfoDTO model)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return Decline("User is not found");
            }

            mapper.Map(model, user);

            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline("User information update is failed");
            }

            return Accept(mapper.Map<UserDTO>(user));
        }

        public async Task<ServiceResponse> UpdateUserPassword(string userId, UpdateUserPasswordDTO model)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return Decline("User is not found");
            }

            if(user.Password != HashPassword(model.CurrentPassword))
            {
                return Decline("Wrong password");
            }

            user.Password = HashPassword(model.NewPassword);            

            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline("User password update is failed");
            }

            return Accept();
        }

        public async Task<ServiceContentResponse<IEnumerable<string>>> GetUserRoles(string userId)
        {
            IEnumerable<string> roles;
            try
            {
                roles = await context.UserRoles.Where(v => v.UserId == userId).Select(v => v.Role).ToArrayAsync();                
            }
            catch
            {
                return Decline<IEnumerable<string>>("User roles request is failed");
            }
            return Accept(roles);
        }

        public async Task<ServiceResponse> AddUserRoles(string userId, params string[] roles)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return Decline("User is not found");
            }

            try
            {
                var userRoles = await context.UserRoles.Where(v => v.UserId == userId).Select(v => v.Role).ToArrayAsync();
                await context.UserRoles.AddRangeAsync(
                    roles.Where(v=>userRoles.Contains(v) == false)
                         .Select(v => new UserRole { Role = v, UserId = user.Id }));
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline("User roles creation is failed");
            }
            return Accept();
        }

        public async Task<ServiceResponse> RemoveUserRoles(string userId, params string[] roles)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return Decline("User is not found");
            }

            try
            {
                var userRoles = await context.UserRoles.Where(v => v.UserId == user.Id && roles.Contains(v.Role)).ToArrayAsync();
                context.UserRoles.RemoveRange(userRoles);
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline("User roles removal is failed");
            }
            return Accept();
        }

        public async Task<ServiceContentResponse<UserDTO>> CheckPassword(string email, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(v=>v.Email == email);
            if (user == null)
            {
                return Decline("User is not found");
            }

            if(user.Password != HashPassword(password))
            {
                return Decline("Wrong password");
            }

            return Accept(mapper.Map<UserDTO>(user));
        }

        public string HashPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            using (var hash = SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                {
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                }
                return hashedInputStringBuilder.ToString();
            }
        }
    }
}
