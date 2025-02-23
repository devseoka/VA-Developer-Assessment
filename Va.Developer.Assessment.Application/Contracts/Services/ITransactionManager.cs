using System;
using System.Data;
using Va.Developer.Assessment.Infrastructure.Persistence.Context;

namespace Va.Developer.Assessment.Application.Contracts.Services
{
    public interface ITransactionManager
    {
        VaDeveloperContext WorflowContext { get; }
        Task<IDbTransaction> BeginDatabaseTransactionAsync();
        Task RollbackTransactionAsync();
        Task CommitTransactionAsync();
    }
}
