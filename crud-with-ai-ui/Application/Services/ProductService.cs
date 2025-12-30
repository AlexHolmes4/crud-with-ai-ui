using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
        => _productRepository.GetAllAsync(cancellationToken);

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => _productRepository.GetByIdAsync(id, cancellationToken);

    public Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
        => _productRepository.AddAsync(product, cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => _productRepository.DeleteAsync(id, cancellationToken);
}
