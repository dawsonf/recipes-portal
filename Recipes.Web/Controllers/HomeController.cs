using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Recipes.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //TODO: implement controller base and read value from config
        private readonly string _baseUrl = "https://localhost:7133/api/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserLogin user)
        {
            //flurl request
            var userResponse = await
                         _baseUrl
                         .AppendPathSegment("user/authenticate/")
                         //.WithOAuthBearerToken("Authorization")
                         .PostJsonAsync(new
                         {
                             username = user.Username,
                             password = user.Password
                         })
                         .ReceiveJson();

            if (userResponse is not null)
            {
                //TempData["ReceivedUser"] = JsonConvert.SerializeObject(userResponse);
                return RedirectToAction("Index", "Recipes", TempData["ReceivedUser"]);
            }
            else
            {
                //TODO: Toaster error
                return Error();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
