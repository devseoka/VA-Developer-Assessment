using System.ComponentModel.DataAnnotations.Schema;

namespace Va.Developer.Assessment.Domain.Models
{
    public class Person : DataModelBase
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("surname")]
        public string Surname { get; set; }
        [Column("id_number")]
        public string IdNumber { get; set; }
        [NotMapped]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}