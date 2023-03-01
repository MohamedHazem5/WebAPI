using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Dtos.User
{
    public class UserRegisterDto
    {
        public string Username { get; set; }=string.Empty;
        public string password { get; set; }=string.Empty;
        
    }
}