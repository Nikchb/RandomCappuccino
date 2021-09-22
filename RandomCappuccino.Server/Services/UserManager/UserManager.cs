using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RandomCappuccino.Server.Data;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.IdentityManager;
using RandomCappuccino.Server.Services.UserManager.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.UserManager
{
    public class UserManager : ServiceBase, IUserManager
    {
        private readonly DataBaseContext context;
        private readonly IMapper mapper;
        private readonly IIdentityManager identityManager;

        public UserManager(DataBaseContext context, IMapper mapper, IIdentityManager identityManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.identityManager = identityManager;
        }

        public async Task<ServiceContentResponse<UserDTO>> CreateUser(CreateUserDTO model)
        {
            if(context.Users.FirstOrDefault(v=>v.Email == model.Email) != null)
            {
                return Decline<UserDTO>("This email is already used");
            }

            model.Password = AppFunctions.HashPassword(model.Password);
            
            var user = mapper.Map<User>(model);

            try
            {
                await context.Users.AddAsync(user);
                await context.UserRoles.AddRangeAsync(model.Roles.Select(v => new UserRole { UserId = user.Id, Role = v }));
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline<UserDTO>("User creation is failed");
            }

            return Accept(mapper.Map<UserDTO>(user));
        }

        public async Task<ServiceContentResponse<UserDTO>> GetUserInfo()
        {
            var user = await context.Users.FindAsync(identityManager.UserId);            
            if(user == null)
            {
                return Decline<UserDTO>("User is not found");
            } 

            return Accept(mapper.Map<UserDTO>(user));
        }

        public async Task<ServiceContentResponse<UserDTO>> UpdateUserInfo(UpdateUserInfoDTO model)
        {
            var user = await context.Users.FindAsync(identityManager.UserId);
            if (user == null)
            {
                return Decline<UserDTO>("User is not found");
            }

            mapper.Map(model, user);

            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline<UserDTO>("User information update is failed");
            }

            return Accept(mapper.Map<UserDTO>(user));
        }

        public async Task<ServiceResponse> UpdateUserPassword(UpdateUserPasswordDTO model)
        {
            var user = await context.Users.FindAsync(identityManager.UserId);
            if (user == null)
            {
                return Decline("User is not found");
            }

            if(user.Password != AppFunctions.HashPassword(model.CurrentPassword))
            {
                return Decline("Wrong password");
            }

            user.Password = AppFunctions.HashPassword(model.NewPassword);            

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

        public async Task<ServiceContentResponse<IEnumerable<string>>> GetUserRoles()
        {
            return await GetUserRoles(identityManager.UserId);           
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

        public async Task<ServiceResponse> AddUserRoles(params string[] roles)
        {
            var user = await context.Users.FindAsync(identityManager.UserId);
            if (user == null)
            {
                return Decline("User is not found");
            }

            try
            {
                var userRoles = await context.UserRoles.Where(v => v.UserId == identityManager.UserId).Select(v => v.Role).ToArrayAsync();
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

        public async Task<ServiceResponse> RemoveUserRoles(params string[] roles)
        {
            var user = await context.Users.FindAsync(identityManager.UserId);
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
                return Decline<UserDTO>("User is not found");
            }

            if(user.Password != AppFunctions.HashPassword(password))
            {
                return Decline<UserDTO>("Wrong password");
            }

            return Accept(mapper.Map<UserDTO>(user));
        }
    }
}
