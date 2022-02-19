//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Recipes.Domain.Interfaces;
using Recipes.Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Recipes.Domain.Models.Recipe;
using Recipes.Helpers;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Recipes.Model;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    public class RecipesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
      
        public RecipesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region All user functions

        [Authorize]
        [HttpGet(nameof(GetRecipes))]
        public async Task<ActionResult> GetRecipes()
        {
            IEnumerable<Recipe> result = await _unitOfWork.Recipes.ExecWithStoredProcedureResults("PS_REC_Recipes");

            _unitOfWork.Complete();

            if (result.Count() > 0)
            {
                var json = JsonConvert.SerializeObject(result);
                return Ok(json);
            } 
            else
            {
                return BadRequest("Try again...");
            }
        }

        [Authorize]
        [HttpGet(nameof(GetRecipe))]
        public async Task<ActionResult> GetRecipe(/*int Id*/string rec_name)
        {

            IEnumerable<Recipe> result = await _unitOfWork.Recipes.ExecWithStoredProcedureResults("PS_REC_Name @rec_name",
                                                                   new SqlParameter("rec_name", SqlDbType.VarChar) { Value = rec_name });

            _unitOfWork.Complete();

            if (result.Count() > 0)
            {
                return Ok(result);
            }
            else
            {
                return Ok(result);
            }
        }
        #endregion

        #region Admin functions

        [Authorize(Role.Admin)]
        [HttpPost(nameof(AddRecipe))]
        public async Task<ActionResult> AddRecipe(string rec_name, string rec_ingredients, int usr_code, [FromBody] AddRecipe image)
        {
            try
            {

                //Does json string conform to model?
                var deserialize = JsonConvert.DeserializeObject<List<Ingredient>>(rec_ingredients);

                int result = await _unitOfWork.Recipes.ExecWithStoredProcedure("PI_REC_Recipe @rec_name, @rec_ingredients, @usr_code, @rec_image ",
                                                                       new SqlParameter("rec_name", SqlDbType.VarChar) { Value = rec_name },
                                                                       new SqlParameter("rec_ingredients", SqlDbType.VarChar) { Value = rec_ingredients },
                                                                       new SqlParameter("usr_code", SqlDbType.Int) { Value = usr_code },
                                                                       new SqlParameter("rec_image", SqlDbType.VarChar) { Value = image.rec_image });

                _unitOfWork.Complete();

                if (result > 0)
                {
                    return Ok("Success");
                }
                else
                {
                    return BadRequest("Try again...");
                }
            }
            catch
            {
                return BadRequest("Try again...");
                //TODO: Middleware/exceptions and logging
            }
        }




        [Authorize(Role.Admin)]
        [HttpPost(nameof(UpdateRecipe))]
        public async Task<ActionResult> UpdateRecipe(string rec_name, string rec_ingredients)
        {
            try
            {
                //Does json string conform to model?
                var deserialize = JsonConvert.DeserializeObject<List<Ingredient>>(rec_ingredients);

                int result = await _unitOfWork.Recipes.ExecWithStoredProcedure("PU_REC_Recipe @rec_name, @rec_ingredients",
                                                                       new SqlParameter("rec_name", SqlDbType.VarChar) { Value = rec_name },
                                                                       new SqlParameter("rec_ingredients", SqlDbType.VarChar) { Value = rec_ingredients });

                _unitOfWork.Complete();

                if (result > 0)
                {
                    return Ok("Success");
                }
                else
                {
                    return BadRequest("Try again...");
                }
            }
            catch
            {
                return BadRequest("Try again...");
            }
        }

        [Authorize(Role.Admin)]
        [HttpPost(nameof(DeleteRecipe))]
        public async Task<ActionResult> DeleteRecipe(int Id)
        {
            try
            {
                int result = await _unitOfWork.Recipes.ExecWithStoredProcedure("PD_REC_Recipe @rec_code ",
                                                                       new SqlParameter("rec_code", SqlDbType.VarChar) { Value = Id });

                _unitOfWork.Complete();

                if (result > 0)
                {
                    return Ok("Success");
                }
                else
                {
                    return BadRequest("Try again...");
                }
            }
            catch
            {
                return BadRequest("Try again...");
            }
        }
        #endregion
    }
}

