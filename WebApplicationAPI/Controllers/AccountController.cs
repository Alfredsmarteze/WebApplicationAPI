using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication.Context;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtSettings jwtSettings;
        private readonly ModelContext _context;
        public AccountController(JwtSettings jwtSettings, ModelContext context)
        {
            this.jwtSettings = jwtSettings;
            this._context = context;
        }
        

    //    private IEnumerable<Users> logins = new List<Users>()
    //    {
    //        new Users()
    //    {

    //                    Id = 1,
    //                    EmailId = "admin@gmail.com",
    //                    UserName = "Admin",
    //                    Password = "Admin"
    //    },
    //        new Users()
    //    {
    //                    Id = 2,
    //                    EmailId = "admin@gmail.com",
    //                    UserName = "User1",
    //                    Password = "Admin",
    //        }
    //};

        [HttpPost]
        public IActionResult GetToken(UserLogins userLogins)
        {
            try
            {
                var Token = new UserTokens();
                var valid = _context.users.Any(x => x.UserName.Equals(userLogins.UserName)|| x.Password.Equals(userLogins.Password)); 
                //var valid = logins.Any(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase) 
                //|| x.Password.Equals(userLogins.Password, StringComparison.OrdinalIgnoreCase));
                if (valid)
                {
                    var user = _context.users.FirstOrDefault(x => x.UserName.Equals(userLogins.UserName)|| x.Password.Equals(userLogins.Password));
                 //   var user = logins.FirstOrDefault(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase)
                 //   || x.Password.Equals(userLogins.Password, StringComparison.OrdinalIgnoreCase)) ; 
                    Token = JwtHelpers.JwtHelpers.GenTokenkey(new UserTokens()
                    {
                        EmailId = user.EmailId,
                        GuidId = Guid.NewGuid(),
                        UserName = user.UserName,
                        Password = user.Password,
                        //Id = user.Id,
                    }, jwtSettings);
                    _context.userTokens.Add(Token);
                    _context.SaveChanges();
                }
                else
                {
                    return BadRequest($"Wrong password");
                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetList()
        {
            var get = _context.userTokens.ToList().AsReadOnly();//.AsQueryable().FirstOrDefault(s=>s.Id==1006);    
            return Ok(get);
            //return Ok(logins);
        }
    }
}
