using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dal.Core;

namespace dal.DomainClasses
{
    [Table("Order")]
    public class Order : IObjectWithState
    {

        [NotMapped]
        public State State { get; set; }

        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public Customer Customer { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
