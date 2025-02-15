namespace Va.Developer.Assessment.Application.Contracts.Response
{
    public interface IResponse
    {
        public string Message { get; set; }
        public bool Succeeded { get; set; }
    }
}