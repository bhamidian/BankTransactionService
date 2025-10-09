using BankTransferService.Application_Service.DTOs;
using BankTransferService.ApplicationService.DTOs;
using BankTransferService.Domain.Contracts.Repositories;
using BankTransferService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BankTransferService.Infrastructure.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _context;

        public CardRepository(AppDbContext context) => _context = context;

        public CardDto? GetById(string cardNumber)
        {
            return _context.Cards
                .Where(c => c.CardNumber == cardNumber)
                .Select(c => new CardDto
                {
                    CardNumber = c.CardNumber,
                    HolderName = c.HolderName,
                    Balance = c.Balance,
                    IsActive = c.IsActive
                })
                .FirstOrDefault();
        }

        public CardTransferDto? GetDestById(string cardNumber)
        {
            return _context.Cards
                .Where(c => c.CardNumber == cardNumber)
                .Select(c => new CardTransferDto
                {
                    CardNumber = c.CardNumber,
                    HolderName = c.HolderName,
 
                })
                .FirstOrDefault();
        }

        public List<CardDto> GetAll()
        {
            return _context.Cards
                .Select(c => new CardDto
                {
                    CardNumber = c.CardNumber,
                    HolderName = c.HolderName,
                    Balance = c.Balance,
                    IsActive = c.IsActive
                })
                .ToList();
        }

        public void Add(CardDto dto)
        {
            var entity = new Card
            {
                CardNumber = dto.CardNumber,
                HolderName = dto.HolderName,
                Balance = dto.Balance,
                IsActive = dto.IsActive
            };
            _context.Cards.Add(entity);
        }

        public void UpdateBasic(CardDto dto)
        {
            var entity = _context.Cards.FirstOrDefault(c => c.CardNumber == dto.CardNumber);
            if (entity is null) return;

            entity.HolderName = dto.HolderName;
            entity.Balance = dto.Balance;
            entity.IsActive = dto.IsActive;

            _context.Cards.Update(entity);
        }

        public void Delete(string cardNumber)
        {
            var entity = _context.Cards.FirstOrDefault(c => c.CardNumber == cardNumber);
            if (entity is null) return;
            _context.Cards.Remove(entity);
        }


        public void UpdateBalance(string cardNumber, float newBalance)
        {
            var tracked = _context.Cards.Local.FirstOrDefault(c => c.CardNumber == cardNumber);
            if (tracked != null)
            {
                tracked.Balance = newBalance;
                return;
            }

            var stub = new Card { CardNumber = cardNumber };
            _context.Attach(stub);
            _context.Entry(stub).Property(c => c.Balance).CurrentValue = newBalance;
            _context.Entry(stub).Property(c => c.Balance).IsModified = true;
        }

        public void ChangeBalance(string cardNumber, float amount)
        {
            _context.Cards
                .Where(c => c.CardNumber == cardNumber)
                .ExecuteUpdate(c => c
                    .SetProperty(x => x.Balance, x => x.Balance + amount));
        }

        public bool CheckPassword(string cardNumber, string password)
        {
            var entity = _context.Cards
                .AsNoTracking()
                .Where(c => c.CardNumber == cardNumber)
                .Select(c => new { c.Password, c.IsActive })
                .FirstOrDefault();

            if (entity is null || !entity.IsActive) return false;
            return entity.Password == password; 
        }

        public void IncrementFailedAttempt(string cardNumber)
        {
            var entity = _context.Cards.FirstOrDefault(c => c.CardNumber == cardNumber);
            if (entity is null) return;

            entity.FailedAttempts++;
            if (entity.FailedAttempts >= 3)
                entity.IsActive = false; 

        }

        public void ResetFailedAttempt(string cardNumber)
        {
            var stub = _context.Cards.FirstOrDefault(c => c.CardNumber == cardNumber);
            if (stub is null) return;

            if (stub.FailedAttempts != 0)
                stub.FailedAttempts = 0;

        }

        public void Block(string cardNumber)
        {
            var stub = new Card { CardNumber = cardNumber, IsActive = false };
            _context.Attach(stub);
            _context.Entry(stub).Property(c => c.IsActive).IsModified = true;
        }

        public bool Login(string cardnumber, string password)
        {
            return _context.Cards
                .Any(x => x.CardNumber == cardnumber && x.Password == password);

        }


        public bool ChangePassword(string cardNumber, string oldPassword, string newPassword)
        {
            var card = _context.Cards.FirstOrDefault(c => c.CardNumber == cardNumber && c.Password == oldPassword);
            if (card is null)
                return false;

            card.Password = newPassword;
            _context.SaveChanges();
            return true;
        }


    }
}
