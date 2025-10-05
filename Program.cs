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

    static void LoginMenu(CardService cardService, TransactionService txService)
    {
        while (true)
        {
            Console.WriteLine("\n===Welcome to the Bank ===");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Exit");

            var choice = Console.ReadLine();
            switch(choice)
            {
                case "1":
                    {
                        Login(cardService,txService);
                        break;
                    }
                case "2":
                    {
                        return;
                    }
            }


        }
    }


    static void RunMenu(CardService cardService, TransactionService txService)
    {
        while (true)
        {
            Console.WriteLine("\n=== Bank Transfer Menu ===");
            Console.WriteLine("1. Show all cards");
            Console.WriteLine("2. Transfer money");
            Console.WriteLine("3. Show transactions for a card");
            Console.WriteLine("4. Change password");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ShowCards(cardService);
                    break;
                case "2":
                    DoTransfer(txService,cardService);
                    break;
                case "3":
                    ShowTransactions(txService);
                    break;
                case "4":
                    ChangePassword(cardService);
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

        ConsolePainter.WriteTable(all);
        
    }

    static void ShowDesticationCard(CardService cardService,string cardnumber)
    {
 

        Console.WriteLine("\n--- Card ---");
        var card = cardService.Getdestinfo(cardnumber);

        var cards = new List<CardTransferDto>();
        cards.Add(card);

        ConsolePainter.WriteTable(cards);



    }

    static void DoTransfer(TransactionService txService,CardService cardService)
    {

        Console.WriteLine("Enter source card number: ");
        string source = Console.ReadLine();

        Console.WriteLine("Enter destination card number: ");
        string dest = Console.ReadLine();

        ShowDesticationCard(cardService, dest);

        Console.WriteLine("Do you confirm this info?");
        Console.WriteLine("1.yes");
        Console.WriteLine("2.no");

        string approve = Console.ReadLine();

        switch (approve)
        {
            case "1":
                {
                    DateTime target = DateTime.Now.AddMinutes(5);

                    Console.WriteLine("Enter the transaction key:");
                    string transactionkey = Console.ReadLine();

                    if (target > DateTime.Now)
                    {
                        if (txService.Generatekey(transactionkey))
                        {

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
                        else
                        {
                            Console.WriteLine("Invalid transaction key");

                        }
                    }
                    else
                    {
                        Console.WriteLine("Your time is off!");
                    }
                    break;

                }
            case "2":
                {
                    return;
                }

            default:
                {
                    Console.WriteLine("Invalid choice!;");
                    break;
                }

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



    static void Login(CardService cardService, TransactionService txService)
    {
        Console.WriteLine("\nEnter card number:");
        string? cardnumber = Console.ReadLine();

        Console.WriteLine("Enter password:");
        string? password = Console.ReadLine();
        if (cardService.Authenticate(cardnumber,password) == true)
        {
            RunMenu(cardService, txService);

        }
        else
        {
            Console.WriteLine("cardnumber or password is wrong");
        }
    }

    static void ChangePassword(CardService cardService)
    {
        Console.WriteLine("\nEnter your card number:");
        string cardnumber = Console.ReadLine();

        Console.WriteLine("Enter your old password:");
        string oldpassword = Console.ReadLine();

        Console.WriteLine("Enter your new password:");
        string newpassword = Console.ReadLine();

        var success =cardService.ChangePassword(cardnumber, oldpassword, newpassword);

        Console.WriteLine(
            success ? "Your password successfully changed!"
                    : "Your card or old password is wrong!"
        );




    }

    


}
