using MsBanking.Card.Domain.Dto;

namespace MsBanking.Card.Services
{
    public interface ICardTransactionService
    {
        Task<CardTransactionResponseDto> CreateCardTransaction(CardTransactionRequestDto cardTransaction);
        Task<List<CardTransactionResponseDto>> GetAllTransanctions(int cardId);
    }
}