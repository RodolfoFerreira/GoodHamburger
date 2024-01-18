namespace GoodHamburger.Repository.Interfaces
{
    public interface IOrderProductRepository
    {
        Task<int> DeleteAllById(IEnumerable<int> ids);
    }
}
