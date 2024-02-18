using Microsoft.EntityFrameworkCore;

namespace MsBanking.Card
{
    public class CardDbContext:DbContext
    {
        public CardDbContext() { }
        public CardDbContext(DbContextOptions<CardDbContext> options):base(options)
        {
        }
        public DbSet<Domain.Entity.Card> Cards { get; set; }
        public DbSet<Domain.Entity.CardTransaction> CardTransactions { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entity.Card>().ToTable("Card");
            modelBuilder.Entity<Domain.Entity.Card>().HasQueryFilter(x=>x.IsActive);

            modelBuilder.Entity<Domain.Entity.Card>().Property(x=>x.CreditLimit).HasPrecision(18,4);


            modelBuilder.Entity<Domain.Entity.CardTransaction>().ToTable("CardTransaction");
            modelBuilder.Entity<Domain.Entity.CardTransaction>().HasQueryFilter(x=>x.IsActive);
            modelBuilder.Entity<Domain.Entity.CardTransaction>().Property(x => x.Amount).HasPrecision(18, 4);
        }
    }
}
