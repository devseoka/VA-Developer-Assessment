using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using Va.Developer.Assessment.Infrastructure.Persistence.Context;

namespace Va.Developer.Assessment.Application.Services
{
    public class TransactionManager(VaDeveloperContext context) : ITransactionManager
    {
        private readonly VaDeveloperContext _context = context;
        private IDbTransaction _transaction;

        public VaDeveloperContext WorflowContext => _context;

        public async Task<IDbTransaction> BeginDatabaseTransactionAsync()
        {
            if (_context.Database.CurrentTransaction == null)
            {
                var dbTransaction = await _context.Database.BeginTransactionAsync();
                _transaction = dbTransaction.GetDbTransaction();
                return _transaction;
            }
            return _context.Database.CurrentTransaction.GetDbTransaction();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CurrentTransaction.RollbackAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CurrentTransaction.CommitAsync();
            }
        }
    }
}
