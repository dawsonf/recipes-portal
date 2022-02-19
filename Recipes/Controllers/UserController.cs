using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipes.Domain.Interfaces;
using Recipes.Domain.Models;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Configuration;
using Microsoft.AspNetCore.Http;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Anonymous Endpoints

        [AllowAnonymous]
        [HttpPost(nameof(Authenticate))]
        public async Task<ActionResult> Authenticate([FromBody] UserLogin user)
        {
            IEnumerable<User> result = await _unitOfWork.Users.ExecWithStoredProcedureResults("PS_USR_User @usr_user_name, @usr_password",
                                                                   new SqlParameter("usr_user_name", SqlDbType.VarChar) { Value = user.Username },
                                                                   new SqlParameter("usr_password", SqlDbType.VarChar) { Value = user.Password });
            _unitOfWork.Complete();

            if (result.Count() > 0)
            {
                var userGroup = result.Select(x => x.UserGroup).FirstOrDefault();
                var userName = result.Select(x => x.Username).FirstOrDefault();
                var token  = CreateAuthenticationTicket(userGroup, userName);

                var cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(1);
                cookieOptions.Domain = Request.Host.Value;
                cookieOptions.Path = "/";
                Response.Cookies.Append("jwt", token.Result, cookieOptions);

                //token.Result;

                var json = JsonConvert.SerializeObject(result.FirstOrDefault());
                return Ok(json);
            }
            else
            {
                return BadRequest("Try again...");
            }
        }
        #endregion
    }
}
