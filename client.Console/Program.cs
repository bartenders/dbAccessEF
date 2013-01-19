using System.Collections.Generic;
using System.Linq;
using dal.Core;
using dal.DomainClasses;
using dal.Services;
using client.Console.Demos;

namespace client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //DisconnectedScenarios.CrudDemo(); //1
            //ConnectedScenarios.CurrentAndOriginalValueDemo(); //2
            //DisconnectedScenarios.SetValuesDemo(); // 3
            //ConnectedScenarios.LazyLoadingDemo(); //4
            DisconnectedScenarios.ConcurrencyDemo(); //5
            

            System.Console.ReadLine();




        }
    }
}
