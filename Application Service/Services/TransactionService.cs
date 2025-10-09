using BankTransferService.Application_Service.DTOs;
using BankTransferService.ApplicationService.DTOs;
using BankTransferService.Domain.Contracts.Repositories;
using BankTransferService.Domain.Contracts.Services;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

public class TransactionService : ITransactionService
{
    private readonly ICardRepository _cards;
    private readonly ITransactionRepository _txs;
    private readonly IUnitOfWork _uow;
    private readonly ICardService _cardService;

    private static readonly Regex _cardRegex = new Regex(@"^\d{16}$");

    public TransactionService(ICardRepository cards, ITransactionRepository txs, IUnitOfWork uow,ICardService cardService)
    {
        _cards = cards;
        _txs = txs;
        _uow = uow;
        _cardService = cardService;
    }

    public TransactionService()
    {
    }

    public TransferResultDto Transfer(TransferRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.SourceCardNumber) || !_cardRegex.IsMatch(request.SourceCardNumber))
            return Fail("Invalid source card number.");
        if (string.IsNullOrWhiteSpace(request.DestinationCardNumber) || !_cardRegex.IsMatch(request.DestinationCardNumber))
            return Fail("Invalid destination card number.");
        if (request.SourceCardNumber == request.DestinationCardNumber)
            return Fail("Source and destination cannot be the same.");
        if (request.Amount <= 0)
            return Fail("Amount must be greater than zero.");

        var source = _cards.GetById(request.SourceCardNumber);
        var dest = _cards.GetById(request.DestinationCardNumber);
        if (source is null || dest is null) return Fail("Source or destination card not found.");
        if (!source.IsActive || !dest.IsActive) return Fail("Source or destination card is blocked.");

        if (!_cards.CheckPassword(source.CardNumber, request.Password))
        {
            _cards.IncrementFailedAttempt(source.CardNumber);
            _uow.Save();
            return Fail("Incorrect source card password.");
        }
        _cards.ResetFailedAttempt(source.CardNumber);
        var finalrequestamount = request.Amount > 100f
            ? source.Balance -= (float)(request.Amount * 0.15)
            : source.Balance -= (float)(request.Amount * 0.05);

        if (source.Balance < request.Amount)
        {
            _uow.Save(); 
            return Fail("Insufficient balance.");
        }

        var today = DateTime.Today;
        var todayTotal = _txs.GetByCard(source.CardNumber)
                             .Where(t => t.IsSuccessfull
                                      && t.TransactionDate >= today
                                      && t.TransactionDate < today.AddDays(1)
                                      && t.SourceCardNumber == source.CardNumber)
                             .Select(t => t.Amount)
                             .DefaultIfEmpty(0f)
                             .Sum();
        var amount = todayTotal + request.Amount;

        if (amount > 250f)
            return Fail("Daily transfer limit (250) exceeded.");
 

        var originalSourceBalance = source.Balance;
        var originalDestBalance = dest.Balance;
        bool sourceDebited = false;
        bool destCredited = false;
        bool txInserted = false;

        try
        {
            _cards.UpdateBalance(source.CardNumber, originalSourceBalance - request.Amount);
            _cardService.DecreaseBalance(dest.CardNumber, originalDestBalance - request.Amount);

            sourceDebited = true;

            _cards.UpdateBalance(dest.CardNumber, originalDestBalance + request.Amount);
            destCredited = true;


            var txDto = new TransactionDto
            {
                Amount = request.Amount,
                TransactionDate = DateTime.Now,
                IsSuccessfull = true,
                SourceCardNumber = source.CardNumber,
                DestinationCardNumber = dest.CardNumber
            };
            var newId = _txs.Add(txDto);
            txDto.Id = newId;
            txInserted = true;

            _uow.Save();

            return new TransferResultDto
            {
                Success = true,
                Message = "Transfer completed successfully.",
                Transaction = txDto
            };
        }
        catch
        {
            //try
            //{
            //    if (destCredited)
            //        _cardService.DecreaseBalance(source.CardNumber, originalSourceBalance - request.Amount);
            //    //_cards.UpdateBalance(dest.CardNumber, originalDestBalance);

            //    if (sourceDebited)
            //        _cardService.IncreaseBalance(dest.CardNumber, originalDestBalance+ request.Amount);
            //    //_cards.UpdateBalance(source.CardNumber, originalSourceBalance);


            //    _uow.Save();
            //}
            //catch
            //{ 
            //    return
            //}

            return Fail("Transfer failed; amount was refunded to the source.");
        }
    }




    public List<TransactionDto> GetByCard(string cardNumber)
        => _txs.GetByCard(cardNumber);

    private static TransferResultDto Fail(string msg)
        => new TransferResultDto { Success = false, Message = msg };

    //public bool feerate(float amount,bool upperthan100)
    //{

    //    if (upperthan100 && amount)
    //    {
    //    }
    //}

    public bool Generatekey(string key)
    {
        
        string source = @"C:\Users\Abolfazl\source\repos\BankTransferService\BankTransferService\transactionkey.txt";
        var realkey = File.ReadAllText(source).Trim();
        return key == realkey;

        
    }

    public string KeyGenerator()
    {
        return _txs.Generatekey();
    }



}
