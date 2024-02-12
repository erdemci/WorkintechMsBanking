using Microsoft.AspNetCore.Http.HttpResults;
using MsBanking.Common.Entity;
using MsBanking.Core.Services;

namespace MsBanking.Core.Apis
{
    public static class CustomerApi
    {
        public static IEndpointRouteBuilder MapCustomerApi(this IEndpointRouteBuilder app)
        {
            app.MapGet("/customers", GetAllCustomers);
            app.MapGet("/customers/{id}", GetCustomer);
            app.MapPost("/customers", CreateCustomer);
            return app;
        }

        private static async Task<Results<Ok<List<Customer>>, NotFound>> GetAllCustomers(ICustomerService service)
        {
            var customers = await service.GetCustomers();
            if (!customers.Any())
                return TypedResults.NotFound();
            return TypedResults.Ok(customers);
        }

        private static async Task<Results<Ok<Customer>, NotFound>> GetCustomer(ICustomerService service, int id)
        {
            var customer = await service.GetCustomer(id);
            if (customer == null)
                return TypedResults.NotFound();
            return TypedResults.Ok(customer);
        }

        private static async Task<Results<Ok<Customer>, BadRequest>> CreateCustomer(ICustomerService service, Customer customer)
        {
            var createdCustomer = await service.CreateCustomer(customer);
            return TypedResults.Ok(createdCustomer);
        }
    }
}
