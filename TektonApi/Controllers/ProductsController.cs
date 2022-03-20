using MediatR;
using Microsoft.AspNetCore.Mvc;
using TektonApi.Handlers.Products.Commands;
using TektonApi.Handlers.Products.Queries;
using TektonApi.Models;

namespace TektonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            return Ok(await _mediator.Send(new GetProductQuery() { Id = id }));
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult> CreateProduct(CreateProductCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        // PUT: api/Products/5
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(UpdateProductCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
