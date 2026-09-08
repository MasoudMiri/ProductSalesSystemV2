using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Application.Interfaces;
using MyProject.Domain.Entities;

namespace MyProject.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return _mapper.Map<ProductDto?>(product);
        }

        public async Task AddAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _repository.AddAsync(product);
            productDto.Id = product.Id;
        }

        public async Task UpdateAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _repository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task ToggleActiveAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product != null)
            {
                product.IsActive = !product.IsActive;
                await _repository.UpdateAsync(product);
            }
        }
    }
}