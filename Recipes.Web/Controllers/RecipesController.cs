using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Recipes.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Recipes.Web.Models.Recipe;

namespace Recipes.Web.Controllers
{
    public class RecipesController : Controller
    {
        //TODO: implement controller base and read value from config
        private readonly string _baseUrl = "https://localhost:7133/api/";
        private readonly IConfiguration _config;

        public RecipesController(IConfiguration config)
        {
            _config = config;
        }


        #region All user functions
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(int counter, int displayedRows)
        {
            //Get Recipes list
            List<Root> recipesResponse = await RefreshRecipesList();

            if (recipesResponse is not null)
            {
                ViewBag.ReceivedRecipes = recipesResponse;

                return View(ViewBag.ReceivedRecipes);
            }
            else
            {
                //TODO: Toaster error
                return View("Index");
            }
        }
        //TODO: change recipes list/view to partial view 

        //[Route("recipes/{counter?}/{displayedRows?}")]
        //[HttpGet]
        //public async Task<IActionResult> IndexAsync(int counter, int displayedRows)
        //{
        //    try { 

        //        int rows = _config.GetValue<int>("WebAppSettings:FetchLimit");
        //        List<Root> recipesResponse;

        //        if (counter > 1)
        //        {
        //            var currentCounter = displayedRows;
        //            var rowsToFetch = currentCounter + rows;
        //            recipesResponse = await RefreshRecipesList();
        //            var data = recipesResponse.Take(rowsToFetch).ToList();

        //            ViewBag.ReceivedRecipes = data;
        //            ViewBag.Data = data.Count();

        //            return View(ViewBag.ReceivedRecipes);
        //        }
        //        else
        //        {
        //            recipesResponse = await RefreshRecipesList();
        //            var data = recipesResponse.Take(rows).ToList();

        //            ViewBag.ReceivedRecipes = data;
        //            ViewBag.Data = data.Count();

        //            return View(ViewBag.ReceivedRecipes);
        //        }
        //    }
        //    catch
        //    {
        //        //TODO: Toaster error
        //        return View("Index");
        //    }
        //}

        [Route("recipe/{rec_route_name?}")]
        [HttpGet]
        public async Task<IActionResult> Recipe(/*int Id*/ string rec_route_name)
        {

            //Get Recipes list
            List<Root> recipeResponse = await
                               _baseUrl
                               .AppendPathSegment("Recipes/GetRecipe/")
                               // .WithOAuthBearerToken("eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlciIsIlJvbGUiOiJHdWVzdCIsIm5iZiI6MTY0NTAzMTMxNCwiZXhwIjoxNjQ1MTE3NzE0LCJpc3MiOiJodHRwOi8vd3d3LnJlY2lwZS5jb20iLCJhdWQiOiJodHRwOi8vd3d3LnJlY2lwZS5jb20ifQ.WbXhHggr6-HrmZ1SAcOvy_4PNnm3C3merGjI2sBnDfs")
                               .SetQueryParams(new
                               {
                                   rec_name = rec_route_name
                               })
                               .GetAsync()
                               .ReceiveJson<List<Root>>();

            if (recipeResponse.Count() > 0)
            {
                var ingredients = recipeResponse.Select(x => x.rec_ingredients);
                var ingredientsObject = JsonConvert.DeserializeObject<List<Ingredient>>(ingredients.First());

                ViewBag.Ingredients = ingredientsObject;
                ViewBag.ReceivedRecipe = recipeResponse;

                return View(recipeResponse);
            }
            else
            {
                return RedirectToAction("Index", ViewBag.ReceivedRecipes);
            }
        }
        #endregion

        #region Admin functions
        [HttpPost]
        public async Task<IActionResult> DeleteRecipe(int Id)
        {
            var response = await
                      _baseUrl
                      //.WithOAuthBearerToken("eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlciIsIlJvbGUiOiJHdWVzdCIsIm5iZiI6MTY0NTAzMTMxNCwiZXhwIjoxNjQ1MTE3NzE0LCJpc3MiOiJodHRwOi8vd3d3LnJlY2lwZS5jb20iLCJhdWQiOiJodHRwOi8vd3d3LnJlY2lwZS5jb20ifQ.WbXhHggr6-HrmZ1SAcOvy_4PNnm3C3merGjI2sBnDfs")
                      .AppendPathSegment("Recipes/DeleteRecipe/")
                       .SetQueryParams(new
                       {
                           Id = Id
                       })
                      .PostAsync();

            //Refresh list
            List<Root> recipesResponse = await RefreshRecipesList();

            if (response.StatusCode == 200)//Ok
            {
                //TODO: Toaster Success
                ViewBag.ReceivedRecipes = recipesResponse;
                return RedirectToAction("Index", ViewBag.ReceivedRecipes);

                //  View(recipesResponse);
            }
            else
            {
                //TODO: Toaster error
                return RedirectToAction("Index", ViewBag.ReceivedRecipes);
            }
        }

        public async Task<IActionResult> AddRecipe(string data, IFormFile photo)
        {
            if (data != null)
            {
                Root root = JsonConvert.DeserializeObject<Root>(data);

                var o = JsonConvert.DeserializeObject<JObject>(root.rec_ingredients);

                foreach (var item in o)
                {
                    root.rec_ingredients = item.Value.ToString();
                }

                byte[] fileBytes = null ;
                var b64 = "";

                if (photo != null)
                {
                    if (photo.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            photo.CopyTo(ms);
                            fileBytes = ms.ToArray();
                            b64 = Convert.ToBase64String(fileBytes);
                        }

                        var response = await
                            _baseUrl
                            .AppendPathSegment("Recipes/AddRecipe/")
                             //.WithOAuthBearerToken("eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlciIsIlJvbGUiOiJHdWVzdCIsIm5iZiI6MTY0NTAzMTMxNCwiZXhwIjoxNjQ1MTE3NzE0LCJpc3MiOiJodHRwOi8vd3d3LnJlY2lwZS5jb20iLCJhdWQiOiJodHRwOi8vd3d3LnJlY2lwZS5jb20ifQ.WbXhHggr6-HrmZ1SAcOvy_4PNnm3C3merGjI2sBnDfs")
                             .SetQueryParams(new
                             {
                                 rec_name = root.rec_name,
                                 rec_ingredients = root.rec_ingredients,
                                 usr_code = 1// Only admin user can perform this function - but probably a better idea to grab this value from the User Object and pass through to controller
                             })
                            .PostJsonAsync(new { rec_image = b64 });

                        //Refresh list
                        List<Root> recipesResponse = await RefreshRecipesList();

                        if (response.StatusCode == 200)//Ok
                        {
                            //TODO: Toaster Success
                            ViewBag.ReceivedRecipes = recipesResponse;
                            return View("Index", ViewBag.ReceivedRecipes);

                        }
                        else
                        {
                            //TODO: Toaster error
                            return View(recipesResponse);
                        }
                    }
                }
            }
            else
            {
                return View();
            }
            return View();

        }

        public async Task<IActionResult> UpdateRecipe(string data)
        {
            if (data != null)
            {
                Root root = JsonConvert.DeserializeObject<Root>(data);

                ViewBag.rec_name = root.rec_name;
                //ViewBag.ingredients = rec_ingredients;vv  

                var o = JsonConvert.DeserializeObject<JObject>(root.rec_ingredients);

                foreach (var item in o)
                {
                    root.rec_ingredients = item.Value.ToString();
                }

                var response = await
                          _baseUrl
                          .AppendPathSegment("Recipes/UpdateRecipe/")
                           // .WithOAuthBearerToken("eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlciIsIlJvbGUiOiJHdWVzdCIsIm5iZiI6MTY0NTAzMTMxNCwiZXhwIjoxNjQ1MTE3NzE0LCJpc3MiOiJodHRwOi8vd3d3LnJlY2lwZS5jb20iLCJhdWQiOiJodHRwOi8vd3d3LnJlY2lwZS5jb20ifQ.WbXhHggr6-HrmZ1SAcOvy_4PNnm3C3merGjI2sBnDfs")

                           .SetQueryParams(new
                           {
                               rec_name = root.rec_name,
                               rec_ingredients = root.rec_ingredients
                           })
                          .PostAsync();

                //Refresh list
                List<Root> recipesResponse = await RefreshRecipesList();

                if (response.StatusCode == 200)//Ok
                {
                    //TODO: Toaster Success
                    ViewBag.ReceivedRecipes = recipesResponse;
                    return View();

                }
                else
                {
                    //TODO: Toaster error
                    return View(ViewBag.ReceivedRecipes);
                }
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region Helpers
        private async Task<List<Root>> RefreshRecipesList()
        {
            return await
                                  _baseUrl
                                  .AppendPathSegment("Recipes/GetRecipes/")
                                  // .WithOAuthBearerToken("eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlciIsIlJvbGUiOiJHdWVzdCIsIm5iZiI6MTY0NTAzOTIyNywiZXhwIjoxNjQ1MTI1NjI3LCJpc3MiOiJodHRwOi8vd3d3LnJlY2lwZS5jb20iLCJhdWQiOiJodHRwOi8vd3d3LnJlY2lwZS5jb20ifQ.IUalQIJf1MVolCU8WNianC5XMDYphcy8NIF2zNv4ul4")
                                  .GetAsync()
                                  .ReceiveJson<List<Root>>();
        }
        #endregion
    }
}
