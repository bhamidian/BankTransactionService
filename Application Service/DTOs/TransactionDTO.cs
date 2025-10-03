using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankTransferService.ApplicationService.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsSuccessfull { get; set; }
        public string SourceCardNumber { get; set; }
        public string DestinationCardNumber { get; set; }
    }
}


