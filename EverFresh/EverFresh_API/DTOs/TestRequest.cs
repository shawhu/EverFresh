using ServiceStack;

namespace EverFresh_API
{
    [Route("/test")]
    public class TestRequest
    {
        public string keyword { get; set; }
    }
}