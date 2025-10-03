using BankTransferService.Framework;
using System.ComponentModel.DataAnnotations;

public class Card : BaseEntity
{
    [Key] 
    public string CardNumber { get; set; }
    public string HolderName { get; set; }
    public float Balance { get; set; }
    public bool IsActive { get; set; }
    public string Password { get; set; }
    public int FailedAttempts { get; set; }
    public ICollection<Transaction> SourceTransactions { get; set; } = [];
    public ICollection<Transaction> DestinationTransactions { get; set; } = [];
}
