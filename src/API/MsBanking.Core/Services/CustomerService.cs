using AutoMapper;
using MassTransit;
using MassTransit.Transports;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MsBanking.Common.Dto;
using MsBanking.Common.Entity;
using MsBanking.Core.Domain;
using Serilog;
using System.Text.Json;

namespace MsBanking.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMongoCollection<Customer> customerCollection;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint publishEndpoint;

        public CustomerService(IOptions<DatabaseOption> options,IMapper _mapper, IPublishEndpoint _publishEndpoint)
        {
            mapper = _mapper;
            publishEndpoint = _publishEndpoint;
            var dbOptions = options.Value;
            var client = new MongoClient(dbOptions.ConnectionString);//bağlantı
            var database = client.GetDatabase(dbOptions.DatabaseName);//veritabanı
            customerCollection = database.GetCollection<Customer>(dbOptions.CustomerCollectionName);//tablo
        }

        public async Task<CustomerResponseDto> GetCustomer(string id)
        {
            var customerEntity = await customerCollection.FindAsync(c => c.IsActive && c.Id == id);
            var entity = customerEntity.FirstOrDefault();
            var mapped = mapper.Map<CustomerResponseDto>(entity);
            return mapped;
        }

        public async Task<List<CustomerResponseDto>> GetCustomers()
        {
            var customerEntities = await customerCollection.FindAsync(c => c.IsActive);
            var customerList = customerEntities.ToList();
            var mapped = mapper.Map<List<CustomerResponseDto>>(customerList);
            return mapped;
        }

        public async Task<CustomerResponseDto> CreateCustomer(CustomerDto customer)
        {
            var customerEntity = mapper.Map<Customer>(customer);

            customerEntity.CreatedDate = DateTime.Now;
            customerEntity.UpdatedDate = DateTime.Now;
            customerEntity.IsActive = true;
            await customerCollection.InsertOneAsync(customerEntity);

            var customerResponse = mapper.Map<CustomerResponseDto>(customerEntity);
                
            //1.Yol --> tek yönlü ve hata durumunda diğer requestleri etkiler. Senkron çağrım.
            //httpClient.PostAsync("http://localhost:5004/api/Notification",new StringContent(JsonConvert.SerializeObject(customerResponse), Encoding.UTF8, "application/json"));

            //2.Yol Asenkron çağrım. 
            //HesapOluştur Komutunu tetiklenmesi yeterli.
            //branchCode parametresi gönderiliyor.

            //2.Yol Asenkron yol çağrımı --rabbitmq'ya mesaj bırakıyoruz, masstransit sayesinde

            await publishEndpoint.Publish<CustomerResponseDto>(customerResponse);//CreateAccountCOMMAND
            var messageStr = JsonSerializer.Serialize(customerResponse);
            Log.Logger.Information("Customer Created and Account Created Command Sent to RabbitMQ, message:{messageStr}",messageStr);

            return customerResponse;
        }
        public async Task<CustomerResponseDto> UpdateCustomer(string id,CustomerDto customer)
        {
            var customerEntity = mapper.Map<Customer>(customer);

            var existCustomer = await this.GetCustomer(id);
            if (existCustomer == null)
                return null;

            customerEntity.UpdatedDate = DateTime.Now;
            await customerCollection.ReplaceOneAsync(c => c.Id == id, customerEntity);

            var customerResponseDto = mapper.Map<CustomerResponseDto>(customerEntity);
            return customerResponseDto;
        }

        public async Task<bool>  DeleteCustomer(string id)
        {
            var entityDto = await GetCustomer(id);
            if (entityDto == null)
                return false;

            var entity = mapper.Map<Customer>(entityDto);
            entity.IsActive = false;
            var result =  await customerCollection.ReplaceOneAsync(c => c.Id == id, entity);
            return result.ModifiedCount>0;
        }
    }
}
