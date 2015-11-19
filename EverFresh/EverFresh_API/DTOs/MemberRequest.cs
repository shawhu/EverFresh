using ServiceStack;

namespace EverFresh_API
{
    [Route("/member/signin")]
    public class SignInRequest
    {
        public string email { get; set; }
        public string cellphone { get; set; }
        public string password { get; set; }

    }
    [Route("/member/signup")]
    public class SignUpRequest
    {
        public string email { get; set; }
        public string cellphone { get; set; }
        public string password { get; set; }

    }
    [Route("/member")]
    public class GetMemberRequest
    {
    }
    [Route("/member/getall")]
    public class GetAllMembersRequest
    {
    }

}