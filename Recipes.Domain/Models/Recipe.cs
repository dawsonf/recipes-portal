using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipes.Domain.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        public DateTime rec_date_created { get; set; }
        public string rec_ingredients { get; set; }
        public string rec_name { get; set; }
        public string rec_route_name { get; set; }
        public int usr_code { get; set; }
        public string usr_first_name { get; set; }
        public string usr_surname { get; set; }
        public string rec_image { get; set; }

        public class Ingredient
        {
            [Key]
            public string ingredient { get; set; }
        }

    }

    

}

