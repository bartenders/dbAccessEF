using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dal.Core;

namespace dal.DomainClasses
{
    [Table("Customer")]
    public class Customer : IObjectWithState
    {

        [NotMapped]
        public State State { get; set; }

        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        public virtual ICollection<Order> Orders { get; set; }
    }
}
