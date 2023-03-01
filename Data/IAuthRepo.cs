using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Data
{
    public interface IAuthRepo
    {
        Task<ServiceRespone<int>> Register(User user, string password);
        Task<ServiceRespone<string>> Login(string username,string password);
        Task<bool> UserExists(string username);
    }
}