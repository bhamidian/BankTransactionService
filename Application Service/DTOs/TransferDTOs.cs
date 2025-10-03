using BankTransferService.ApplicationService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransferService.Application_Service.DTOs
{
    public class TransferRequestDto
    {
        public string SourceCardNumber { get; set; }
        public string DestinationCardNumber { get; set; }
        public float Amount { get; set; }
        public string Password { get; set; }
    }

    public class TransferResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string CardNumber { get; set; }
        public string HolderName { get; set; }
        public float Balance { get; set; }
        public bool IsActive { get; set; }
        public TransactionDto? Transaction { get; set; }
    }

}
