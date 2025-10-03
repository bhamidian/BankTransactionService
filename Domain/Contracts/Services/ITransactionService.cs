using BankTransferService.Application_Service.DTOs;
using BankTransferService.ApplicationService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransferService.Domain.Contracts.Services
{
    public interface ITransactionService
    {
        TransferResultDto Transfer(TransferRequestDto request); 
        List<TransactionDto> GetByCard(string cardNumber);
    }


}
