using BankTransferService.ApplicationService.DTOs;
using BankTransferService.Domain.Contracts;
using BankTransferService.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace BankTransferService.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context) => _context = context;

        public TransactionDto? GetById(int id)
        {
            return _context.Transactions
                .Where(t => t.Id == id)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    IsSuccessfull = t.IsSuccessfull,
                    SourceCardNumber = t.SourceCardId,
                    DestinationCardNumber = t.DestinationCardId
                })
                .FirstOrDefault();
        }

        public List<TransactionDto> GetAll()
        {
            return _context.Transactions
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    IsSuccessfull = t.IsSuccessfull,
                    SourceCardNumber = t.SourceCardId,
                    DestinationCardNumber = t.DestinationCardId
                })
                .ToList();
        }

        public List<TransactionDto> GetByCard(string cardNumber)
        {
            return _context.Transactions
                .Where(t => t.SourceCardId == cardNumber || t.DestinationCardId == cardNumber)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    IsSuccessfull = t.IsSuccessfull,
                    SourceCardNumber = t.SourceCardId,
                    DestinationCardNumber = t.DestinationCardId
                })
                .ToList();
        }

        public int Add(TransactionDto dto)
        {
            var entity = new Transaction
            {
                Amount = dto.Amount,
                TransactionDate = dto.TransactionDate,
                IsSuccessfull = dto.IsSuccessfull,
                SourceCardId = dto.SourceCardNumber,
                DestinationCardId = dto.DestinationCardNumber
            };

            _context.Transactions.Add(entity);
            return entity.Id; 
        }
    }
}
