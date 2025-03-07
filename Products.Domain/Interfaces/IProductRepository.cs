using Products.Domain.Entities;

namespace Products.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    
    Task AddAsync(Product product);
}