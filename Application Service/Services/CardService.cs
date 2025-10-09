using BankTransferService.Application_Service.DTOs;
using BankTransferService.Domain.Contracts.Repositories;
using BankTransferService.Domain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransferService.Application_Service.Services
{


    namespace BankTransferService.Application_Service.Services
    {
        public class CardService : ICardService
        {
            private readonly ICardRepository _cards;
            private readonly IUnitOfWork _uow;

            public CardService(ICardRepository cards, IUnitOfWork uow)
            {
                _cards = cards;
                _uow = uow;
            }

            public CardService()
            {
            }

            public CardDto? Get(string cardNumber)
                => _cards.GetById(cardNumber);

            public CardTransferDto? Getdestinfo(string cardNumber)
                => _cards.GetDestById(cardNumber);

            public bool Authenticate(string cardNumber, string password)
            {
                var card = _cards.GetById(cardNumber);
                if (card is null) return false;          
                if (!card.IsActive) return false;       

                if (_cards.CheckPassword(cardNumber, password))
                {
                    _cards.ResetFailedAttempt(cardNumber);
                    _uow.Save();
                    return true;                           
                }

                _cards.IncrementFailedAttempt(cardNumber);
                _uow.Save();
                return false;                             
            }

            public void Block(string cardNumber)
            {
                _cards.Block(cardNumber);
                _uow.Save();
            }

            public void UpdateBasic(CardDto card)
            {
                _cards.UpdateBasic(card);
                _uow.Save();
            }

            public void SetActive(string cardNumber, bool isActive)
            {
                var dto = _cards.GetById(cardNumber);
                if (dto is null) return;
                dto.IsActive = isActive;
                _cards.UpdateBasic(dto);
                _uow.Save();
            }

            public void UpdateBalance(string cardNumber, float newBalance)
            {
                _cards.UpdateBalance(cardNumber, newBalance);
                _uow.Save();
            }

            public List<CardDto> GetAll()
            {
                return _cards.GetAll();
            }

            public bool ChangePassword(string cardNumber, string oldPassword, string newPassword)
            {
                return _cards.ChangePassword(cardNumber, oldPassword, newPassword);


            }

            public void IncreaseBalance(string cardNumber, float amount)
            {
                _cards.ChangeBalance(cardNumber, amount);
                _uow.Save();
            }

            public void DecreaseBalance(string cardNumber, float amount)
            {
                _cards.ChangeBalance(cardNumber, -amount);
                _uow.Save();
                
            }

            public void ChangeBalance(string cardNumber, float amount)
            {
                throw new NotImplementedException();
            }
        }
    }

}
