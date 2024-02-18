using MassTransit;
using MsBanking.Common.Dto;
using MsBanking.Common.Helper;
using MsBanking.Core.Account.Domain.Const;
using MsBanking.Core.Account.Services;
using Serilog;
using System.Text.Json;

namespace MsBanking.Core.Account.Consumers
{
    public class AccountConsumer : IConsumer<CustomerResponseDto>
    {
        private readonly IAccountService accountService;
        private readonly IHttpClientHandler httpClientHandler;

        public AccountConsumer(IAccountService _accountService, IHttpClientHandler _httpClientHandler)
        {
           accountService= _accountService;
           httpClientHandler = _httpClientHandler;
        }

        public async Task Consume(ConsumeContext<CustomerResponseDto> context)
        {
            CustomerResponseDto message = context.Message;
            var branch = await GetBranchByCityId(message.CityId);

            AccountDto account = new()
            {
                AccountNumber = GenerateRandomAccountNumber(),//generate random
                IbanNumber = GenerateRandomIban(),
                AccountType = (int)AccountTypeEnum.Retail,
                Balance = 0,
                Currency = "TRY",
                UserId = message.Id,
                BranchId = branch.Id,//httpclientla branchservice çağrılacak//senkron şekilde
                AccountSuffix = 350
            };

            await accountService.CreateAccount(account);

        }

        private string GenerateRandomIban()
        {
            Random random = new Random();
            string bankCode = "WBANK"; // A fictional bank code
            string branchCode = random.Next(0, 9999).ToString("D4"); // 4 digit branch code
            string accountNumber = random.Next(0, 999999999).ToString("D9"); // 9 digit account number

            // Construct the IBAN
            string iban = $"TR{random.Next(0, 99).ToString("D2")}{bankCode}{branchCode}{accountNumber}";

            return iban;
        }

        //generate random accountNumber
        private string GenerateRandomAccountNumber()
        {
            Random random = new Random();
            string accountNumber = random.Next(0, 999999999).ToString("D9"); // 9 digit account number

            return accountNumber;
        }
        
        private async Task<BranchResponseDto> GetBranchByCityId(int cityId) 
        {
            try
            {
                const string BRANCH_SERVICE_URL = "https://localhost:5005/api/v1/branch/getbycityid/";
                string responseStr = await httpClientHandler.GetStringAsync(BRANCH_SERVICE_URL + cityId);

                if (string.IsNullOrEmpty(responseStr))
                    return new BranchResponseDto();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                BranchResponseDto branchResponse = JsonSerializer.Deserialize<BranchResponseDto>(responseStr, options);
                return branchResponse!;
            }
            catch (Exception e)
            {
                Log.Logger.Error("Error occured while getting branch by cityId, error:{e}", e);
                Console.WriteLine("Error occured while getting branch by cityId, error:{e}", e);
                return new BranchResponseDto()
                {
                    Id=1,
                };
            }
           

        }
    }
}
