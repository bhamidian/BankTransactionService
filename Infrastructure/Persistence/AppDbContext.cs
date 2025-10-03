using Microsoft.EntityFrameworkCore;

namespace BankTransferService.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Card> Cards { get; set; }
        string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=BankTransferServiceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public AppDbContext() { }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.SourceCard)
                .WithMany(c => c.SourceTransactions)
                .HasForeignKey(t => t.SourceCardId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.DestinationCard)
                .WithMany(c => c.DestinationTransactions)
                .HasForeignKey(t => t.DestinationCardId)
                .OnDelete(DeleteBehavior.NoAction);





            base.OnModelCreating(modelBuilder);

            // کارت‌ها
            modelBuilder.Entity<Card>().HasData(
                new Card
                {
                    CardNumber = "6037991234567890",
                    HolderName = "Alice Johnson",
                    Balance = 1500000f,
                    IsActive = true,
                    Password = "1234",
                    FailedAttempts = 0
                },
                new Card
                {
                    CardNumber = "6274120987654321",
                    HolderName = "Bob Smith",
                    Balance = 500000f,
                    IsActive = true,
                    Password = "9999",
                    FailedAttempts = 0
                },
                new Card
                {
                    CardNumber = "5022291111222233",
                    HolderName = "Charlie Brown",
                    Balance = 2500000f,
                    IsActive = true,
                    Password = "1111",
                    FailedAttempts = 0
                },
                new Card
                {
                    CardNumber = "4580123456789012",
                    HolderName = "David Miller",
                    Balance = 750000f,
                    IsActive = true,
                    Password = "2222",
                    FailedAttempts = 0
                },
                new Card
                {
                    CardNumber = "6219865432109876",
                    HolderName = "Emma Wilson",
                    Balance = 1250000f,
                    IsActive = true,
                    Password = "3333",
                    FailedAttempts = 0
                },
                new Card
                {
                    CardNumber = "5892109988776655",
                    HolderName = "Frank Thomas",
                    Balance = 300000f,
                    IsActive = true,
                    Password = "4444",
                    FailedAttempts = 0
                }

            );

            // تراکنش‌ها
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    Id = 1,
                    Amount = 200000f,
                    TransactionDate = new DateTime(2025, 1, 3, 12, 0, 0),
                    IsSuccessfull = true,
                    SourceCardId = "6037991234567890",
                    DestinationCardId = "6274120987654321"
                },
                new Transaction
                {
                    Id = 2,
                    Amount = 150000f,
                    TransactionDate = new DateTime(2025, 1, 1, 12, 0, 0),
                    IsSuccessfull = true,
                    SourceCardId = "6274120987654321",
                    DestinationCardId = "5022291111222233"
                }
            );
        }


    }


   


    
}
