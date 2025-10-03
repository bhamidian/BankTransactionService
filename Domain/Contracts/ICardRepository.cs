using BankTransferService.Application_Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransferService.Domain.Contracts
{
    public interface ICardRepository
    {
        CardDto? GetById(string cardNumber);
        List<CardDto> GetAll();
        void Add(CardDto card);
        void UpdateBasic(CardDto card);                 
        void UpdateBalance(string cardNumber, float newBalance);
        void IncrementFailedAttempt(string cardNumber); 
        void ResetFailedAttempt(string cardNumber);     
        void Block(string cardNumber);
        bool CheckPassword(string cardNumber, string password);


    }
}
