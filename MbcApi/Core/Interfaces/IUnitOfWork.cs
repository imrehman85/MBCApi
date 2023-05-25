namespace MbcApi.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IRoomRepo roomRepo { get; set; }

        int Complete();
    }
}
