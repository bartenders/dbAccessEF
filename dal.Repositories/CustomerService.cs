using System.Collections.Generic;
using System.Linq;
using dal.Contexts;
using dal.Core;
using dal.DomainClasses;

namespace dal.Services
{
    public class CustomerService : ServiceBase<CustomerContext>
    {

        public List<Customer> AllCustomersWhoHaveOrdered
        {
            get { return DbContext.Customers.Where(c => c.Orders.Any()).ToList(); }
        }

    }


}
