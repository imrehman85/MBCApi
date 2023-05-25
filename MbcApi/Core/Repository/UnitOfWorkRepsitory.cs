using MbcApi.Core.DbContext;
using MbcApi.Core.Interfaces;

namespace MbcApi.Core.Repository
{
    public class UnitOfWorkRepsitory : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWorkRepsitory(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            roomRepo = new RoomRepo(_dbContext);
        }

        public IRoomRepo roomRepo { get; set; }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
