using MediatR;
using Orders.Application.Commands;
using Orders.Domain.DTOs;
using Orders.Domain.Entities;
using Orders.Domain.Enums;
using Orders.Infra.Context;

namespace Orders.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly MongoDbContext _mongoDbContext;
        private readonly ApplicationDbContext _context;

        public CreateOrderCommandHandler(
            ApplicationDbContext context,
            MongoDbContext mongoDbContext)
        {
            _context = context;
            _mongoDbContext = mongoDbContext;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = CreateOrderFromRequest(request);

            await SaveOrderToDatabase(order);
            await SaveOrderToMongoDb(order, request.CustomerName);

            return order.Id;
        }

        private Order CreateOrderFromRequest(CreateOrderCommand request)
        {
            var orderItems = request.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice
            }).ToList();

            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                Items = orderItems,
                TotalAmount = orderItems.Sum(item => item.TotalPrice)
            };

            return order;
        }

        private async Task SaveOrderToDatabase(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        private async Task SaveOrderToMongoDb(Order order, string customerName)
        {
            var orderDto = MapOrderToDto(order, customerName);
            await _mongoDbContext.Orders.InsertOneAsync(orderDto);
        }

        private OrderDto MapOrderToDto(Order order, string customerName)
        {
            return new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = customerName,
                Date = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                Items = order.Items.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                }).ToList()
            };
        }
    }
}