using BankTransferService.Framework;

public class Transaction : BaseEntity
{
    public Transaction ()
    {

    }
    public float Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public bool IsSuccessfull { get; set; }

    public string SourceCardId { get; set; }
    public string DestinationCardId { get; set; }

    public Card SourceCard { get; set; }
    public Card DestinationCard { get; set; }
}
