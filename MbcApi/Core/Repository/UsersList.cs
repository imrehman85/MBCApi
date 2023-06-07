using MbcApi.Core.DbContext;
using MbcApi.Core.Entities;
using MbcApi.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MbcApi.Core.Repository
{
    public class UsersList : GenericRepo<ApplicationUser>, IUsersList
    {

        public UsersList(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
