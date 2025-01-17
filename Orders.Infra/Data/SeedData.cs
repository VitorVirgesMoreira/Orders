using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Infra.Context;

namespace Orders.Infra.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            if (await context.Customers.AnyAsync() || await context.Products.AnyAsync())
                return;

            // Seed de Customers
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Vitor Virges",
                Email = "vtr.vrgs@example.com",
                Phone = "123-456-7890"
            };
            await context.Customers.AddAsync(customer);

            // Seed de Products
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Produto 1",
                    Price = 10.00m
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Produto 2",
                    Price = 20.00m
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Produto 3",
                    Price = 30.00m
                }
            };
            await context.Products.AddRangeAsync(products);

            await context.SaveChangesAsync();
        }
    }
}
