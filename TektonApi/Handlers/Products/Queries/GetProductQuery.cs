using MediatR;
using Microsoft.EntityFrameworkCore;
using TektonApi.Data;
using TektonApi.Data.Entities;
using TektonApi.Interface;
using TektonApi.Models;

namespace TektonApi.Handlers.Products.Queries
{
    public class GetProductQuery : IRequest<ProductDto>, ICacheable
    {
        public int Id { get; set; }

        public string Key => $"{nameof(Product)}: {Id}";
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public GetProductQueryHandler(AppDbContext context, ILogger<GetProductQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var item = await _context.Products.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            if (item == null)
            {
                //Create information log and return message to the client if item not found
                _logger.LogInformation($"{CommonResponses.NotFound}, Id: {request.Id}");
                throw new ApplicationException($"{CommonResponses.NotFound}, Id: {request.Id}");
            }

            return new ProductDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price
            };
        }
    }
}
