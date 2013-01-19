using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dal.DomainClasses;
using dal.Services;

namespace client.Console.Demos
{
    public static class ConnectedScenarios
    {

        public static void CurrentAndOriginalValueDemo()
        {
            System.Console.WriteLine("------------------------------");
            System.Console.WriteLine(new StackTrace().GetFrame(0).GetMethod().Name);
            System.Console.WriteLine("------------------------------");


            using (var srv = new CustomerService())
            {

                List<Customer> customers = srv.AllIncluding<Customer>("Orders").Take(2).ToList();
                Customer customer = customers.FirstOrDefault();
                if (customer != null)
                {
                    System.Console.WriteLine("-Loaded Customer: {0} {1}", customer.FistName, customer.LastName);
                    
                    // Some modifications
                    customer.FistName = "Hans";
                    customer.LastName = "Müller";

                    var entry = srv.DbContext.Entry(customer);
                    System.Console.WriteLine("-OriginalValues of Customer:");
                    foreach (string propertyName in entry.OriginalValues.PropertyNames)
                    {
                        System.Console.WriteLine("{0} = {1}", propertyName, entry.OriginalValues[propertyName]);
                    }


                    System.Console.WriteLine("-CurrentValues of Customer:");
                    foreach (string propertyName in entry.CurrentValues.PropertyNames)
                    {
                        System.Console.WriteLine("{0} = {1}", propertyName, entry.CurrentValues[propertyName]);
                    }
                    
                }


            }
        }

        public static void LazyLoadingDemo()
        {

            System.Console.WriteLine("------------------------------");
            System.Console.WriteLine(new StackTrace().GetFrame(0).GetMethod().Name);
            System.Console.WriteLine("------------------------------");

            // Einstellungen:
            //1. Context.Configuration.LazyLoadingEnabled = false; --> Falls true, werden die NavigationProperties immer geladen
            //2. Context.Configuration.ProxyCreationEnabled = true; --> Proxy tracks the changes
            //3. public virtual ICollection<OrderDetail> OrderDetails { get; set; } --> virtual
            //4. MultipleActiveResultSets=true im Connectionstring

            using (var srv = new CustomerService())
            {

                List<Customer> customers = srv.AllIncluding<Customer>("Orders").Take(2).ToList();
                
 
                foreach (Customer customer in customers)
                {
                    System.Console.WriteLine("Customer: {0} {1}", customer.FistName, customer.LastName);
                    foreach (Order order in customer.Orders)
                    {
                        System.Console.WriteLine("\tOrder: {0} {1}", order.Id, order.OrderDate);


                        srv.DbContext.Entry(order).Collection(o => o.OrderDetails).Load();

                        foreach (OrderDetail orderDetail in order.OrderDetails)
                        {
                            System.Console.WriteLine("\t\tOrderDetail: {0}", orderDetail.Count);
                        }
                    }
                }
            }

            //http://www.codeproject.com/Articles/396822/Basic-Handling-and-Tips-of-Database-Relationships#WaysEnableLazyLoading

            //Explizit loading with: LoadProperty, Load, Include 
            //_dbContext.ObjectContext.LoadProperty(classObj, c=>c.Teacher);
            //_dbContext.ObjectContext.LoadProperty(teacher, t=>t.Classes); or
            //_dbContext.Entry(classObj).Reference(c => c.Teacher).Load();
            //_dbContext.Entry(teacher).Collection(t => t.Classes).Load(); or:
            //Teacher teacher = _dbContext.Teachers.Include(o=>o.Classes).FirstOrDefault();

            // DbContext is built on top of ObjectContext, it has a property of ObjectConext 
            // which you can access directly for the feature of ObjectContext. 
            // For POCO entities in ObjectContext, function Load won’t work, use LoadProperty or Include instead. 


            //http://blogs.msdn.com/b/adonet/archive/2011/01/31/using-dbcontext-in-ef-feature-ctp5-part-6-loading-related-entities.aspx

            //Explicitly loading related entities

            //Even with lazy loading disabled it is still possible to lazily load related entities, but it must be done with an explicit call. To do so you use the Load method on the related entity’s entry. For example:
            //using (var context = new UnicornsContext())
            //{
            //    var unicorn = context.Unicorns.Find(1);
            //    var princess = context.Princesses.Find(2);

            //    // Load the princess related to a given unicorn 
            //    context.Entry(unicorn).Reference(u => u.Princess).Load();

            //    // Load the princess related to a given unicorn using a string 
            //    context.Entry(unicorn).Reference("Princess").Load();

            //    // Load the unicorns related to a given princess 
            //    context.Entry(princess).Collection(p => p.Unicorns).Load();

            //    // Load the unicorns related to a given princess using a string to 
            //    // specify the relationship 
            //    context.Entry(princess).Collection("Unicorns").Load();
            //}

            //Note that the Reference method should be used when an entity has a navigation property to another single entity. On the other hand, the Collection method should be used when an entity has a navigation property to a collection of other entities.

        }
        




    }
}
