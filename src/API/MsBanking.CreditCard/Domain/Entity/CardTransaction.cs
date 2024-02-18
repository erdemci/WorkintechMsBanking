using MsBanking.Common.Entity;

namespace MsBanking.Card.Domain.Entity
{
    public class CardTransaction:BaseEntity
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
