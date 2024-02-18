using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MsBanking.Card.Domain;
using MsBanking.Card.Domain.Dto;

namespace MsBanking.Card.Services
{
    public class CardTransactionService : ICardTransactionService
    {
        private readonly CardDbContext db;
        private readonly IMapper mapper;

        public CardTransactionService(CardDbContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }

        public async Task<List<CardTransactionResponseDto>> GetAllTransanctions(int cardId)
        {
            var cardTransactions = await db.CardTransactions.Where(x => x.CardId == cardId).ToListAsync();
            var cardTransactionResponse = mapper.Map<List<CardTransactionResponseDto>>(cardTransactions);
            return cardTransactionResponse;
        }


        public async Task<CardTransactionResponseDto> CreateCardTransaction(CardTransactionRequestDto cardTransaction)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var cardTran = mapper.Map<MsBanking.Card.Domain.Entity.CardTransaction>(cardTransaction);

                    if (cardTran.TransactionType == (int)CardTransactionTypeEnum.Deposit)
                    {
                        await DepositToCard(cardTran.CardId, cardTran.Amount);
                    }
                    else if (cardTran.TransactionType == (int)CardTransactionTypeEnum.Withdraw)//Para çekme & Harcama
                    {
                        await WithdrawFromCard(cardTran.CardId, cardTran.Amount);
                    }

                    cardTran.TransactionDate = DateTime.Now;
                    cardTran.IsActive = true;
                    cardTran.CreatedDate = DateTime.Now;
                    cardTran.UpdatedDate = DateTime.Now;

                    db.CardTransactions.Add(cardTran);
                    await db.SaveChangesAsync();

                    var cardTransactionResponse = mapper.Map<CardTransactionResponseDto>(cardTran);

                    var card = await db.Cards.FindAsync(cardTransaction.CardId);
                    cardTransactionResponse.TotalBalance = card.CreditLimit;//Kullanılabilir bakiye


                    transaction.Commit();
                    return cardTransactionResponse;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }



        private async Task DepositToCard(int cardId, decimal amount)
        {

            var account = await db.Cards.FindAsync(cardId);
            if (account == null)
            {
                throw new Exception("Card not found");
            }
            account.CreditLimit += amount;
            await db.SaveChangesAsync();
        }

        private async Task WithdrawFromCard(int cardId, decimal amount)
        {

            var card = await db.Cards.FindAsync(cardId) ?? throw new Exception("Card not found");
            if (card != null && card.CreditLimit < amount)
            {
                throw new Exception("Insufficient balance");
            }
            card.CreditLimit -= amount;
            await db.SaveChangesAsync();
        }

    }
}
