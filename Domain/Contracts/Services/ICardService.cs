using BankTransferService.Application_Service.DTOs;

namespace BankTransferService.Domain.Contracts.Services
{
    public interface ICardService
    {
        CardDto? Get(string cardNumber);
        CardTransferDto? Getdestinfo(string cardNumber);
        bool Authenticate(string cardNumber, string password); 
        void Block(string cardNumber);
        bool ChangePassword(string cardNumber, string oldPassword, string newPassword);
        void UpdateBasic(CardDto card);                       
        void SetActive(string cardNumber, bool isActive);
        void UpdateBalance(string cardNumber, float newBalance);
    }
}
