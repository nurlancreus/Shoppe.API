using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Domain.Entities;

namespace Shoppe.API.Controllers.v1
{
    //[ApiVersion("1.0")]
    public class ProductsController : ApplicationControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;

        public ProductsController(IUnitOfWork unitOfWork, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _unitOfWork = unitOfWork;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productReadRepository.GetAllAsync();

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var product = new Product
            {
                Name = "Product1"
            };

            await _productWriteRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
