using System.ComponentModel.DataAnnotations.Schema;
using Va.Developer.Assessment.Domain.Contracts;

namespace Va.Developer.Assessment.Domain.Models
{
    public class Transaction : DataModelBase, ITransactionTrail
    {
        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; }
        [Column("capture_date")]
        public DateTime CaptureDate { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("account_code")]
        public int AccountCode { get; set; }
        [Column("amount", TypeName = "money")]
        public decimal Amount { get; set; }
    }
}