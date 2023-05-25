using MbcApi.Core.DbContext;
using MbcApi.Core.Entities;
using MbcApi.Core.Interfaces;

namespace MbcApi.Core.Repository
{
    public class RoomRepo : GenericRepo<Rooms>, IRoomRepo
    {
        public RoomRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
