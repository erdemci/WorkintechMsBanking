using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MsBanking.Card.Domain.Dto;
using MsBanking.Card.Domain.Entity;

namespace MsBanking.Card.Services
{
    public class CardService : ICardService
    {
        private readonly CardDbContext db;
        private readonly IMapper mapper;

        public CardService(CardDbContext _db, IMapper _mapper)
        {
            mapper = _mapper;
            db = _db;
        }

        public async Task<CardResponseDto> AddCard(CardRequestDto card)
        {
            var cardEntity = mapper.Map<MsBanking.Card.Domain.Entity.Card>(card);
            cardEntity.CreatedDate = DateTime.Now;
            cardEntity.IsActive = true;
            cardEntity.UpdatedDate = DateTime.Now;
            db.Cards.Add(cardEntity);
            await db.SaveChangesAsync();
            return mapper.Map<CardResponseDto>(cardEntity);
        }

        public async Task<CardResponseDto> GetCard(int id)
        {
            var card = await db.Cards.FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<CardResponseDto>(card);
        }

        public async Task<List<CardResponseDto>> GetCards()
        {
            var cards = await db.Cards.ToListAsync();
            return mapper.Map<List<CardResponseDto>>(cards);
        }

        public async Task<CardResponseDto> UpdateCard(int id, CardRequestDto card)
        {
            var cardEntity = await db.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (cardEntity == null)
                return null;
            cardEntity.UpdatedDate = DateTime.Now;
            mapper.Map(card, cardEntity);
            await db.SaveChangesAsync();
            return mapper.Map<CardResponseDto>(cardEntity);
        }

        public async Task<bool> DeleteCard(int id)
        {
            var cardEntity = await db.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (cardEntity == null)
                return false;
            cardEntity.IsActive = false;
            cardEntity.UpdatedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return true;
        }

    }
}
