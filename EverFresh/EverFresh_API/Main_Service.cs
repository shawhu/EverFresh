using ServiceStack;
using EverFresh.Model;
using System.Collections.Generic;
using System;

namespace EverFresh_API
{
    [EnableCors(allowedMethods: "GET, POST, PUT, DELETE, OPTIONS", allowedHeaders: "Accept,Content-Type,Authorization")]
    public class Main_Service:Service
    {
        //Sign Up, Register,注册
        public string Post(SignUpRequest req)
        {
            if (MemberModel.SignUp(req.email, req.cellphone, req.password))
                return "";
            else
                throw new ArgumentException("Signing up failed.");
        }
        //Sign In, Login, 登录
        public MemberModel Post(SignInRequest req)
        {
            return new MemberModel();
        }
        //获取member信息
        public MemberModel Get(GetMemberRequest req)
        {
            var token = base.Request.Headers["Authorization"];
            MemberModel mm = MemberModel.GetMember(token);
            return mm;
        }
        //获取所有member列表
        public List<MemberModel> Get(GetAllMembersRequest req)
        {
            var token = base.Request.Headers["Authorization"];
            return MemberModel.GetAllMembers(true);
        }
















        //testing

        public List<MemberModel> Get(TestRequest req)
        {
            NoSqlDataObject dbo = new NoSqlDataObject();
            var result = dbo.GetOnlineUsers();
            return result;
        }
    }
}