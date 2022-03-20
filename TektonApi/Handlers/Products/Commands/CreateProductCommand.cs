using MediatR;
using Microsoft.EntityFrameworkCore;
using TektonApi.Data;
using TektonApi.Data.Entities;
using TektonApi.Interface;
using TektonApi.Models;

namespace TektonApi.Handlers.Products.Commands
{
    public class CreateProductCommand : ProductDto, IRequest, ICacheable
    {
        public string Key => CommonResponses.Created;
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public CreateProductCommandHandler(AppDbContext context, ILogger<CreateProductCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            //Prevent repeating items name
            var itemExists = await _context.Products
                .FirstOrDefaultAsync(x =>
                x.Name.ToLower().Equals(request.Name.Trim().ToLower()), cancellationToken);

            if (itemExists != null)
            {
                _logger.LogInformation(CommonResponses.Exists);
                throw new ApplicationException(CommonResponses.Exists);
            }

            try
            {
                //Create and save item
                await _context.Products.AddAsync(
                    new Product(
                        request.Name.Trim(),
                        request.Description,
                        request.Price),
                    cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                //Create error log and return message to the client if a required field is null
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException($"{CommonResponses.Required}, Field: {ex.ParamName}");
            }
            catch (Exception ex)
            {
                //Create error log and return message to the client if item could not be created
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException(CommonResponses.NotCreated);
            }

            return Unit.Value;
        }
    }
}
