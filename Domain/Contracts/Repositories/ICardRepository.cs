using BankTransferService.Application_Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransferService.Domain.Contracts.Repositories
{
    public interface ICardRepository
    {
        CardDto? GetById(string cardNumber);
        CardTransferDto? GetDestById(string cardNumber);
        List<CardDto> GetAll();
        bool Login(string username, string password);        
        void Add(CardDto card);
        void UpdateBasic(CardDto card);                 
        void UpdateBalance(string cardNumber, float newBalance);
        void IncrementFailedAttempt(string cardNumber); 
        void ResetFailedAttempt(string cardNumber);     
        void Block(string cardNumber);
        bool CheckPassword(string cardNumber, string password);
        bool ChangePassword(string cardNumber,string oldPassword, string newPassword);


    }
}
