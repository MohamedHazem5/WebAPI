using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Data
{
    public class AuthRepo : IAuthRepo
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepo(DataContext context , IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ServiceRespone<string>> Login(string username, string password)
        {
            var response = new ServiceRespone<string>();
            var user= await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
            if(user is null)
            {
                response.Sucess = false;
                response.Message = "User is not Exists.";
            }
            else if (!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
            {
                response.Sucess = false;
                response.Message = "Passowrd is Wrong.";
            }
            else 
            {
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceRespone<int>> Register(User user, string password)
        {
            var response=new ServiceRespone<int>();

            if(await UserExists(user.Username))
            {
                response.Sucess=false;
                response.Message="The User is Already Exists.";
                return response;
            }

            CreatePasswordHash(password,out byte[] passwordHash, out byte[] PasswordSalt);
            user.PasswordHash=passwordHash;
            user.PasswordSalt=PasswordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            response.Data =user.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(u=>u.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }



        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //we use H meg Sha 512 Cryptograph algorithm
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return ComputeHash.SequenceEqual(passwordHash);
            }

        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Username)
            };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;

            if(appSettingsToken is null)
            throw new Exception ("AppSettings is not found");

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
            
            SigningCredentials creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor 
            {
                Subject = new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);


        }

    }
}