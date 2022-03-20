using MediatR;
using Microsoft.EntityFrameworkCore;
using TektonApi.Data;
using TektonApi.Interface;
using TektonApi.Models;

namespace TektonApi.Handlers.Products.Commands
{
    public class UpdateProductCommand : ProductDto, IRequest, ICacheable
    {
        public string Key => CommonResponses.Updated;
    }

    public class UpdatedProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public UpdatedProductCommandHandler(AppDbContext context, ILogger<UpdatedProductCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.Products.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            
            if (item == null)
            {
                //Create information log and return message to the client if item not found
                _logger.LogInformation($"{CommonResponses.NotFound}, Id: {request.Id}");
                throw new ApplicationException($"{CommonResponses.NotFound}, Id: {request.Id}");
            }

            var itemExists = await _context.Products
                .FirstOrDefaultAsync(x =>
                x.Name.ToLower().Equals(request.Name.Trim().ToLower()), cancellationToken);

            //Prevent repeating items name
            if (itemExists != null && !itemExists.Id.Equals(item.Id))
            {
                _logger.LogInformation(CommonResponses.Exists);
                throw new ApplicationException(CommonResponses.Exists);
            }

            try
            {
                item.Update(
                    request.Name.Trim(),
                    request.Description,
                    request.Price);

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
                //Create error log and return message to the client if item could not be updated
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException(CommonResponses.NotUpdated);
            }

            return Unit.Value;
        }
    }
}
