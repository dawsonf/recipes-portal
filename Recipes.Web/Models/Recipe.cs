using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Recipes.Web.Models
{
    public class Recipe
    {
        public class Ingredient
        {
            public string ingredient { get; set; }
        }
    }

    public class Root
    {
        public int Id { get; set; }
        public DateTime rec_date_created { get; set; }

        [DisplayName("Ingredients")]
        [Required(ErrorMessage = "Ingredients are required.")]
        public string rec_ingredients { get; set; }

        [DisplayName("Recipe/Meal Name")]
        [Required(ErrorMessage = "Recipe name is required")]
        public string rec_name { get; set; }
        public string rec_route_name { get; set; }
        public string rec_image { get; set; }
        public int usr_code { get; set; }
        public string usr_first_name { get; set; }
        public string usr_surname { get; set; }
    }


}
