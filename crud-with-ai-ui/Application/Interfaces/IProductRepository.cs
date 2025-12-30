using Domain.Entities;

namespace Application.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
