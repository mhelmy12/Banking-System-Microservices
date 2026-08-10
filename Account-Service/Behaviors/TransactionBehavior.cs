using System;
using Account_Service.Data;
using Account_Service.Helpers;
using MediatR;

namespace Account_Service.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ITransactionCommand
{
    private readonly AccountDbContext dbContext;

    public TransactionBehavior(AccountDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    
        try
        {
            var response = await next();
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return response;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
