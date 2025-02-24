using System.ComponentModel.DataAnnotations.Schema;

namespace Va.Developer.Assessment.Domain.Models
{
    public class Account : DataModelBase
    {
        [Column("person_code")]
        public int PersonCode { get; set; }
        [Column("account_number")]
        public string AccountNumber { get; set; }
        [Column("outstanding_balance", TypeName = "money")]
        public decimal Balance { get; set; }
        [NotMapped]
        public virtual ICollection<Transaction> Transactions {get; set;}
    }
}