using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Repositories
{
    public class UserAuthRepo : IUserAuthRepo
    {
        private readonly JwtContext _context;
        private readonly IConfiguration _configuration;
        public UserAuthRepo(JwtContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Role AddRole(Role role)
        {
            var addedRole = _context.Roles.Add(role);
            _context.SaveChanges();
            return addedRole.Entity;
        }

        public Signup AddUser(Signup user)
        {
            var addedUser = _context.Signups.Add(user);
            _context.SaveChanges();
            return addedUser.Entity;
        }

        public bool AssignRoleToUser(AddUserRole obj)
        {
            try
            {
                var addRoles = new List<UserRole>();
                var user = _context.Signups.SingleOrDefault(s => s.Id == obj.UserId);

                if (user == null)
                    throw new Exception("user is not valid");

                foreach (Guid role in obj.RoleIds)
                {
                    var userRole = new UserRole();

                    userRole.Id = role;
                    userRole.UserId = obj.UserId;

                    addRoles.Add(userRole);
                }

                _context.UserRoles.AddRange(addRoles);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Login(Signin login)
        {
            if (login.Email != null && login.Password != null)
            {
                var user = _context.Signups.SingleOrDefault(s => s.Email == login.Email && s.Password == login.Password);
                if (user != null)
                {
                    var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Email", user.Email)
                    };

                    var userRoles = _context.UserRoles.Where(u => u.UserId == user.Id).ToList();
                    var roleIds = userRoles.Select(s => s.RoleId).ToList();
                    var roles = _context.Roles.Where(r => roleIds.Contains(r.Id)).ToList();

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwtToken;
                }
                else
                {
                    throw new Exception("user is not valid");
                }
            }
            else
            {
                throw new Exception("credentials are not valid");
            }
        }
    }
}
