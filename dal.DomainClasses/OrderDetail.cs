using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dal.Core;

namespace dal.DomainClasses
{
    [Table("OrderDetail")]
    public class OrderDetail : IObjectWithState
    {

        [NotMapped]
        public State State { get; set; }


        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ArticleId { get; set; }
        public short? Count { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        public virtual Article Article { get; set; }
        public virtual Order Order { get; set; }
    }
}
