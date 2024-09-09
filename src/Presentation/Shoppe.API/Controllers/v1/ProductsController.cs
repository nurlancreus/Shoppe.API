using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Abstractions.UoW;

namespace Shoppe.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProductsController : ApplicationControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _unitOfWork.ProductReadRepository.GetAllAsync();

            return Ok(products);
        }
    }
}
