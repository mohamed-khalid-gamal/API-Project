using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTO;

using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IConfiguration _configuration;
        UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        //create new user "Regestration" Post
        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegUserDTO RD)
        {
            if (ModelState.IsValid)
            {
                IdentityResult res = await _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = RD.UserName,
                    Email = RD.Email
                }, RD.PassWord);
                if (res.Succeeded)
                {
                    return Ok("added with success");
                }
                else
                {
                    return BadRequest(res.Errors.FirstOrDefault());
                }
            }

            return BadRequest(ModelState);


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LogDTO LogUSer)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(LogUSer.UserName);
                if (user != null)
                {
                    var ch = await _userManager.CheckPasswordAsync(user, LogUSer.Password);
                    if (ch)
                    {
                        // claims token
                        var claim = new List<Claim>();
                        claim.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claim.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claim.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var roles = await _userManager.GetRolesAsync(user);
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        foreach (var item in roles)
                        {
                            claim.Add(new Claim(ClaimTypes.Role, item));
                        }
                        JwtSecurityToken token = new JwtSecurityToken(
                                issuer: _configuration["JWT:ISS"],
                                audience: _configuration["JWT:aud"],
                                claims: claim,
                                expires: DateTime.Now.AddHours(1),
                                signingCredentials: signingCredentials
                            );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            exp = token.ValidTo
                        });
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }
        // "Login" Post


    }
}
