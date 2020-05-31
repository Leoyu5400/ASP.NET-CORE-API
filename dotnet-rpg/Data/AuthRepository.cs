

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_rpg.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
           ServiceResponse<string> response=new ServiceResponse<string>();
           User user=await _context.Users.FirstOrDefaultAsync(a=>a.UserName.ToLower().Equals(username.ToLower()));
           if(user==null)
           {
               response.Success=false;
               response.Message="User not found";
           }
           else if(!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
           {
               response.Success=false;
               response.Message="Wrong password";
           }
           else
           {
               response.Data=CreateToken(user);
           }
           return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {

            ServiceResponse<int> serviceResponse= new ServiceResponse<int>();
            if(await UserExists(user.UserName))
            {
                serviceResponse.Success=false;
                serviceResponse.Message="User Already Exists!";
                return serviceResponse;
            }
            CreatePasswordHash(password,out byte[] passwordHash,out byte[] passwordSalt);

            user.PasswordHash=passwordHash;
            user.PasswordSalt=passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

           
            serviceResponse.Data=user.Id;
            return serviceResponse;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(a=>a.UserName.ToUpper()==username.ToUpper()))
            {
                return true;
            }

            return false;
        }

        private void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using (var hmac=new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt=hmac.Key;
                passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password,byte[] passwordHash,byte[] passwordSalt)
        {
            using (var hmac=new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var ComputeHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < ComputeHash.Length; i++)
                {
                    if(ComputeHash[i]!=passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private string CreateToken(User user)
        {
               List<Claim> claims = new List<Claim>
              {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
             };

             SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
             .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            //With that key, we create new SigningCredentials and use the HmacSha512Signature algorithm for that.
             SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

             SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
               };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

             return tokenHandler.WriteToken(token);
        }
    }
}