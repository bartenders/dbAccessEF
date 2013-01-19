using System;
using System.Data.Entity;

namespace dal.Core
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        int Commit();
        TContext Context { get; }
    }

}