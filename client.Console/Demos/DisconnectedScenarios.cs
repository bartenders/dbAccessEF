using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dal.Core;
using dal.DomainClasses;
using dal.Services;

namespace client.Console.Demos
{
    public static class DisconnectedScenarios
    {


        public static void ConcurrencyDemo()
        {

            System.Console.WriteLine("------------------------------");
            System.Console.WriteLine(new StackTrace().GetFrame(0).GetMethod().Name);
            System.Console.WriteLine("------------------------------");

            Customer customer; 
            // User 1 holt Customer
            using (var srv = new CustomerService())
            {
                customer = srv.All<Customer>().FirstOrDefault();
            }

            // User 2 will auch etwas machen und ist schneller
            var t = Task.Factory.StartNew(() => 
              {
                  using (var srv = new CustomerService())
                  {
                      var c = srv.All<Customer>().FirstOrDefault();
                      if (c != null) 
                      {
                          c.LastName = "XYZ4";
                          c.State = State.Modified;
                          srv.Commit(new List<Customer>{c});
                      }
                  }
              });

            t.Wait();

            if (customer != null)
            {
                customer.LastName = "ABC";
                customer.State = State.Modified;

                System.Console.WriteLine("-Modified Customer: {0} {1}", customer.FistName, customer.LastName);

                using (var srv = new CustomerService())
                {
                    OperationResult result = srv.Commit(new List<Customer> {customer});
                    if (result.ResultType == OperationResultType.ConcurencyExeption)
                        System.Console.WriteLine(result.Message);
                        
                }
                System.Console.WriteLine("-Reseted Customer: {0} {1}", customer.FistName, customer.LastName);
            }



        }


        public static void SetValuesDemo()
        {

            System.Console.WriteLine("------------------------------");
            System.Console.WriteLine(new StackTrace().GetFrame(0).GetMethod().Name);
            System.Console.WriteLine("------------------------------");

            // This Customer comes from WEB or anywhere else
            var disconnectedCustomer = new {
                                            FistName = "Max",
                                            LastName = "Müller"
                                        };


            using (var srv = new CustomerService())
            {
               Customer customer =  srv.All<Customer>().FirstOrDefault();
               if (customer != null)
               {
                   System.Console.WriteLine("-Loaded Customer: {0} {1}", customer.FistName, customer.LastName);
                   var entry = srv.DbContext.Entry(customer);
                   entry.CurrentValues.SetValues(disconnectedCustomer);
                   System.Console.WriteLine("-Updated Customer: {0} {1}", customer.FistName, customer.LastName);

               }

            }

        }


        public static void CrudDemo()
        {

            System.Console.WriteLine("------------------------------");
            System.Console.WriteLine(new StackTrace().GetFrame(0).GetMethod().Name);
            System.Console.WriteLine("------------------------------");


            List<Customer> customers;


            /****************************************************************************
            * Client:"Give me List<Customer>"
            *****************************************************************************/

            using (var customerService = new CustomerService())
            {
                customers = customerService.AllIncluding<Customer>("Orders.OrderDetails.Article").Take(2).ToList();
                //customers = customerService.AllIncluding(c => c.Orders).Take(2).ToList();
            }

            /****************************************************************************
            * Modifications
            * No Context => Disconnected
            *****************************************************************************/



            //foreach (Customer c in customers)
            //{
            //    System.Console.WriteLine("Customer: {0} {1}", c.FistName, c.LastName);
            //    foreach (Order o in c.Orders)
            //    {
            //        System.Console.WriteLine("\tOrder: {0} {1}", o.Id, o.OrderDate);
            //    }
            //}

            foreach (Customer c in customers)
            {
                System.Console.WriteLine("Customer: {0} {1}", c.FistName, c.LastName);
                foreach (Order o in c.Orders)
                {
                    System.Console.WriteLine("\tOrder: {0} {1}", o.Id, o.OrderDate);

                    foreach (OrderDetail d in o.OrderDetails)
                    {
                        System.Console.WriteLine("\t\tOrderDetail: {0} {1}", d.Count, d.Article.Name);
                    }
                }
            }

            System.Console.Read();



            // Update existing Customer
            Customer customer = customers.First();

            // UPDATE
            customer.FistName = "MaxXXXX";
            customer.State = State.Modified;

            // DELETE 
            //customer.State = State.Deleted;
            //foreach (Order o in customer.Orders)
            //{
            //    o.State = State.Deleted;
            //    foreach (OrderDetail d in o.OrderDetails)
            //    {
            //        d.Article.State = State.Deleted;
            //        d.State = State.Deleted;
            //    }
            //}


            /****************************************************************************
            * Modifications
            * No Context => Disconnected
            **************************************************************************/


            // Persist to DB

            using (var service = new CustomerService())
            {
                service.Commit(customers);
            }

        }
    }
}
