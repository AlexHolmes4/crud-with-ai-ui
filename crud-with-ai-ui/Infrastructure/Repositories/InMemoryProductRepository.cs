using System.Collections.Concurrent;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
        => Task.FromResult((IReadOnlyList<Product>)_products.Values.ToList());

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
    {
        _products[product.Id] = product;
        return Task.FromResult(product);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(_products.TryRemove(id, out _));
}
