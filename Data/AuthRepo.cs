using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Data
{
    public class AuthRepo : IAuthRepo
    {
        private readonly DataContext _context;
        public AuthRepo(DataContext context)
        {
            _context = context;
            
        }
        public Task<ServiceRespone<string>> Login(string username, string password)
        {
            throw new NotImplementedException();
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
    }
}