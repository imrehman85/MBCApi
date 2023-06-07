namespace MbcApi.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IRoomRepo roomRepo { get; set; }

        public IUsersList usersList { get; set; }

        int Complete();
    }
}
