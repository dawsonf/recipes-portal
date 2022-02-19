using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Recipes.Domain.Models
{
    public class User
    {
        [Key]
        public int UserCode { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public Int16 UserGroup { get; set; }
    }

    public class UserLogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

}
