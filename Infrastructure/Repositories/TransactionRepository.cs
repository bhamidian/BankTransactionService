using BankTransferService.ApplicationService.DTOs;
using BankTransferService.Domain.Contracts.Repositories;
using BankTransferService.Infrastructure.Persistence;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BankTransferService.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context) => _context = context;

        private static readonly Random _rnd = new Random();
        
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

        public string Generatekey()
        {
            string numbers = "0123456789";
            int length = 5;
            string source = @"C:\Users\Abolfazl\source\repos\BankTransferService\BankTransferService\transactionkey.txt";


            string result = new string(
                Enumerable.Repeat(numbers, length)
                          .Select(s => s[_rnd.Next(s.Length)])
                          .ToArray());

            try
            {
                File.WriteAllText(source, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            _context.SaveChanges();
            return result;
        }
    }
}
