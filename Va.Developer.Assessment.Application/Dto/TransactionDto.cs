using System.Text.Json.Serialization;

namespace Va.Developer.Assessment.Application.Dto
{
    public class TransactionDto : DtoModelBase
    {
        public int AccountId { get; set; }
        public DateTime ProcessedDate { get; set; }
        [JsonIgnore]
        public DateTime OrderedDate { get; set; }
        public decimal Total { get; set; }
    }
}