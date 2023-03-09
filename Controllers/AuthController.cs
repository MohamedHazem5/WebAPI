using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.User;

namespace WebAPI.Controllers
{   [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _authRepo;
        public AuthController(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("Register")]
        public async Task <ActionResult<ServiceRespone<int>>> Register(UserRegisterDto request)
        {
            var response = await _authRepo.Register(
                new User {Username= request.Username}, request.password
            );
            if(!response.Sucess)
            {
                return BadRequest(response);    
            }
            return Ok(response);
        }


        [HttpPost("Login")]
        public async Task <ActionResult<ServiceRespone<int>>> Login(UserLoginDto request)
        {
            var response = await _authRepo.Login(request.Username,request.password);
            if(!response.Sucess)
            {
                return BadRequest(response);    
            }
            return Ok(response);
        }

    }
}