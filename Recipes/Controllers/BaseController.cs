using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Recipes.Domain.Models;
using Recipes.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    public class BaseController : Controller
    {
        #region Auth & Claims

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<string> CreateAuthenticationTicket(int userGroup, string userName)
        {
            var key = Encoding.ASCII.GetBytes("12345678901234567"); //Encoding.ASCII.GetBytes(SiteKeys.Token);
            var JWToken = new JwtSecurityToken(
            issuer: "http://www.recipe.com", //SiteKeys.WebSiteDomain,  
            audience: "http://www.recipe.com",//SiteKeys.WebSiteDomain, 
            claims: GetUserClaims(userGroup ,userName),
            notBefore: new DateTimeOffset(DateTime.Now).DateTime,
            expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        );

            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            HttpContext.Session.SetString("JWToken", token);

            return token;
            //return Task.CompletedTask;
        }

        private IEnumerable<Claim> GetUserClaims(int userGroup, string userName)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim(ClaimTypes.Name, userName);
            claims.Add(_claim);

            if (userGroup == 0)//TODO: ENUM this for a better descriptor 
                               // Admin userGroup as dictated by user group (TR_GRP_Group table on SQL)
            {
                _claim = new Claim("Role", Role.Admin);
                claims.Add(_claim);
            }
            else
            {
                _claim = new Claim("Role", Role.Guest);
                claims.Add(_claim);
            }

            return claims.AsEnumerable<Claim>();
        }

        #endregion

        #region Helpers

        public struct Role
        {
            public const string Admin = "Admin";
            public const string Guest = "Guest";
        }

        #endregion

    }
}
