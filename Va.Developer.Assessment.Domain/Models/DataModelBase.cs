using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Va.Developer.Assessment.Domain.Models
{
    public class DataModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("code")]
        public int Code { get; set; }
    }
}