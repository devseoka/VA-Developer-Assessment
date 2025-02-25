

namespace Va.Developer.Assessment.Tests.Abstractions
{
    [Collection(nameof(AssessmentTestCollection))]
    public abstract class AssessmentBaseTests : IClassFixture<WebAssessmentFactory>
    {
        private readonly WebAssessmentFactory _web;
        protected AssessmentBaseTests(WebAssessmentFactory web) 
        {
            _web = web;
            _web.ExecuteSqlScripts("Persons").Wait();
        }
    }
}
