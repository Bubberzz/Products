using Products.Domain.Entities;
using Products.Domain.ValueObjects;

namespace Products.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(ProductId id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(ProductId  id);
}