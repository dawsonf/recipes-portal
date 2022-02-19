using Recipes.Domain.Interfaces;
using Recipes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Recipes.Repository
{
   public class UserRepository: GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context): base(context)
        {

        }
    }
}
