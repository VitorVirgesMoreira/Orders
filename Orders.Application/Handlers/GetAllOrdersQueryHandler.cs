using MediatR;
using MongoDB.Driver;
using Orders.Application.Queries;
using Orders.Domain.DTOs;
using Orders.Infra.Context;

namespace Application.Handlers
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        private readonly MongoDbContext _mongoDbContext;

        public GetAllOrdersQueryHandler(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _mongoDbContext.Orders
                .Find(_ => true)
                .ToListAsync(cancellationToken);

            return orders;
        }
    }
}