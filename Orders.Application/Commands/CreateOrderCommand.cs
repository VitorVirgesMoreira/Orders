using MediatR;
using Orders.Domain.DTOs;

namespace Orders.Application.Commands
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
