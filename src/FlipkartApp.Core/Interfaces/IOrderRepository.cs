using FlipkartApp.Core.Entities;

namespace FlipkartApp.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
    }
}
