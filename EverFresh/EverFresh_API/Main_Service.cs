using ServiceStack;
using EverFresh.Model;

namespace EverFresh_API
{
    [EnableCors(allowedMethods: "GET, POST, PUT, DELETE, OPTIONS", allowedHeaders: "Accept,Content-Type,Authorization")]
    public class Main_Service:Service
    {
        public string Post(SignUpRequest req)
        {
            return "OK";
        }
        public MemberModel Post(SignInRequest req)
        {
            return new MemberModel();
        }
        public MemberModel Get(GetMemberRequest req)
        {
            var token = base.Request.Headers["Authorization"];
            MemberModel mm = MemberModel.GetMember(token);
            return mm;
        }
















        public string Get(TestRequest req)
        {
            return "Hello " + req.keyword;
        }
    }
}