using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using dal.Contexts;
using dal.Core;
using dal.DomainClasses;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace dal.Services
{
    public class CustomerServiceOld : IRepository<Customer>
    {
        private readonly CustomerContext _context;

        public CustomerContext Context
        {
            get { return _context; }
        }

        public CustomerServiceOld()
        {
            _context = new CustomerContext();
            //_context.Configuration.AutoDetectChangesEnabled = false;

            Context.Configuration.LazyLoadingEnabled = false;
            Context.Configuration.ProxyCreationEnabled = true;
            
        }

        #region Implemention of IRepository<Customer>

        public IQueryable<Customer> All
        {
            get
            {
                return Context.Customers;
            }
        }


        public IQueryable<Customer> AllIncluding(params Expression<Func<Customer, object>>[] includeProperties)
        {
            IQueryable<Customer> query = Context.Customers;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
                
            }
            return query;
        }

        public IQueryable<Customer> AllIncluding(params string[] includeProperties)
        {
            IQueryable<Customer> query = Context.Customers;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);

            }
            return query;
        }

        public Customer Find(int id)
        {
            return Context.Customers.Find(id);
        }


        public void Dispose()
        {
            Context.Dispose();
        }

        #endregion

        #region Implementation of IUnitOfWork

        // Use in Disconnected Scenarios
        // Use disconnectedEntities = null for Connected Scenarios
        public OperationResult Commit(List<Customer> customers = null)
        {
            OperationResult result;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted}))
            {

                if (customers != null)
                    foreach (var customer in customers)
                    {
                        if (customer.State != State.Unchanged)
                            Context.Customers.Add(customer);
                    }

                Context.ApplyStateChanges();
                try
                {
                    Context.SaveChanges();
                    result = new OperationResult {Status = true};
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Optimistic Concurrency reloads data from Database.
                    if (customers != null)
                    {
                        foreach (var customer in customers)
                        {
                            if (customer.State != State.Unchanged)
                            {
                                Context.Entry(customer).CurrentValues.SetValues(Context.Entry(customer).GetDatabaseValues());
                            }
                        }
                    }
                    else
                    {
                        foreach (var customer in Context.Customers)
                        {
                            if (customer.State != State.Unchanged)
                            {
                                Context.Entry(customer).CurrentValues.SetValues(Context.Entry(customer).GetDatabaseValues());
                            }
                        }
                    }

                    result = OperationResult.CreateFromException("Concurrency Exception occured!",ex);
                }
                finally
                {
                    Context.ResetEntityStates();
                    scope.Complete();
                }

            }

            return result;
        }

        #endregion


        #region Additional Repository Functions

        public List<Customer> AllCustomersWhoHaveOrdered
        {
            get { return Context.Customers.Where(c => c.Orders.Any()).ToList(); }
        }


        #endregion

 
    }
}