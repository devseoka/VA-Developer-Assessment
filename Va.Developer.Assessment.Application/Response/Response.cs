namespace Va.Developer.Assessment.Application.Response
{
    public class Response<T> : IResponse
    {
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public T Data { get; set; }
    }
}