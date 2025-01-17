using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Orders.Domain.DTOs;

namespace Orders.Infra.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            _database = client.GetDatabase("OrderManagementSystem");
        }

        public IMongoCollection<OrderDto> Orders => _database.GetCollection<OrderDto>("Orders");
    }
}
