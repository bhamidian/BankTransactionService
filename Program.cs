using BankTransferService.Application_Service.DTOs;
using BankTransferService.Application_Service.Services.BankTransferService.Application_Service.Services;
using BankTransferService.Infrastructure.Persistence;
using BankTransferService.Infrastructure.Repositories;
using LibraryMS.Framework;
using Microsoft.EntityFrameworkCore;

public class Program
{
    static void Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BankTransferServiceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;")
            .Options;

        var context = new AppDbContext(options);

        var cardRepo = new CardRepository(context);
        var txRepo = new TransactionRepository(context);
        var uow = new UnitOfWork(context);

        var cardService = new CardService(cardRepo, uow);
        var txService = new TransactionService(cardRepo, txRepo, uow);

        RunMenu(cardService, txService);
    }


    static void RunMenu(CardService cardService, TransactionService txService)
    {
        while (true)
        {
            Console.WriteLine("\n=== Bank Transfer Menu ===");
            Console.WriteLine("1. Show all cards");
            Console.WriteLine("2. Transfer money");
            Console.WriteLine("3. Show transactions for a card");
            Console.WriteLine("4. Block a card");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ShowCards(cardService);
                    break;
                case "2":
                    DoTransfer(txService);
                    break;
                case "3":
                    ShowTransactions(txService);
                    break;
                case "4":
                    BlockCard(cardService);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    static void ShowCards(CardService cardService)
    {
        Console.WriteLine("\n--- Cards ---");
        var all = cardService.GetAll();
        foreach (var c in all)
        {
            Console.WriteLine($"{c.CardNumber} | {c.HolderName} | Balance: {c.Balance} | Active: {c.IsActive}");
        }
    }

    static void DoTransfer(TransactionService txService)
    {
        Console.Write("\nEnter source card number: ");
        string source = Console.ReadLine();

        Console.Write("Enter destination card number: ");
        string dest = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        Console.Write("Enter amount: ");
        float amount = float.Parse(Console.ReadLine());

        var result = txService.Transfer(new TransferRequestDto
        {
            SourceCardNumber = source,
            DestinationCardNumber = dest,
            Password = password,
            Amount = amount
        });

        Console.WriteLine(result.Message);
        if (result.Success && result.Transaction != null)
        {
            Console.WriteLine($"Transaction ID: {result.Transaction.Id}, Amount: {result.Transaction.Amount}, Success: {result.Transaction.IsSuccessfull}");
        }
    }

    static void ShowTransactions(TransactionService txService)
    {
        Console.Write("\nEnter card number: ");
        string card = Console.ReadLine();

        var txs = txService.GetByCard(card);
        Console.WriteLine("--- Transactions ---");
        ConsolePainter.WriteTable(txs);

    }

    static void BlockCard(CardService cardService)
    {
        Console.Write("\nEnter card number to block: ");
        string card = Console.ReadLine();
        cardService.Block(card);
        Console.WriteLine("Card blocked successfully.");
    }
}
