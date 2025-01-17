using MediatR;
using Orders.Domain.DTOs;

namespace Orders.Application.Queries
{
    public class GetAllOrdersQuery : IRequest<List<OrderDto>>
    {
    }
}
