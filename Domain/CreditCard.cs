using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class CreditCard
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public long CardNumber { get; set; }
        public int CVC { get; set; }
        public DateTime Expiry { get; set; }
    }
}