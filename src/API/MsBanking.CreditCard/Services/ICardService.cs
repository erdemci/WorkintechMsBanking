using MsBanking.Card.Domain.Dto;

namespace MsBanking.Card.Services
{
    public interface ICardService
    {
        Task<CardResponseDto> AddCard(CardRequestDto card);
        Task<bool> DeleteCard(int id);
        Task<CardResponseDto> GetCard(int id);
        Task<List<CardResponseDto>> GetCards();
        Task<CardResponseDto> UpdateCard(int id, CardRequestDto card);
    }
}