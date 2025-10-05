using BankTransferService.Application_Service.DTOs;
using BankTransferService.ApplicationService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransferService.Domain.Contracts.Repositories
{
    public interface ITransactionRepository
    {
        TransactionDto? GetById(int id);
        List<TransactionDto> GetAll();
        List<TransactionDto> GetByCard(string cardNumber);
        int Add(TransactionDto dto);
        string Generatekey();




    }
}
