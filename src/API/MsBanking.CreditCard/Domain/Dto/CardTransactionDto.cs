using AutoMapper;

namespace MsBanking.Card.Domain.Dto
{
    public class CardTransactionDto
    {
        public int CardId { get; set; }
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }
        public string Description { get; set; }
  
    }

    public class CardTransactionRequestDto : CardTransactionDto
    {
    }

    public class CardTransactionResponseDto : CardTransactionDto
    {
        public int Id { get; set; }
        public decimal TotalBalance { get; set; }
    }

    public class CardTransactionDtoProfile : Profile
    {
        public CardTransactionDtoProfile()
        {
            CreateMap<MsBanking.Card.Domain.Entity.CardTransaction, CardTransactionDto>().ReverseMap();
            CreateMap<MsBanking.Card.Domain.Entity.CardTransaction, CardTransactionResponseDto>().ReverseMap();
            CreateMap<MsBanking.Card.Domain.Entity.CardTransaction, CardTransactionRequestDto>().ReverseMap();
        }
    }
}
