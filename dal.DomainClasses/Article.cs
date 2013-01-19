using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dal.Core;

namespace dal.DomainClasses
{
    [Table("Article")]
    public sealed class Article : IObjectWithState
    {

        [NotMapped]
        public State State { get; set; }

        public Article()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public int Id { get; set; }
        public string ArticelNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
