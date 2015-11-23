using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cassandra;

namespace EverFresh.Model
{
    public class NoSqlDataObject
    {
        private static Cluster mycluster { get; set; }
        private static ISession mysession { get; set; }
        private static ISession Connect(String node)
        {
            mycluster = Cluster.Builder()
                   .AddContactPoint(node)
                   .WithCredentials("ttqdx","?r3>4(7f")
                   .Build();
            return mycluster.Connect("EverfreshKeySpace");
        }
        public static bool AddOnlineUser(int member_id,string auth_token)
        {
            if (mysession == null)
                mysession = Connect("121.41.46.175");
            var rs = mysession.Execute("insert into onlineuser (member_id,auth_token,login_time) values (" + member_id.ToString() + ",'" + auth_token + "',dateof(now()));");
            return true;
        }
        public static int IsTokenValid(string token)
        {
            if (mysession == null)
                mysession = Connect("121.41.46.175");
            var rs = mysession.Execute("select * from onlineuser where auth_token = '"+token+"';");
            foreach(var row in rs)
            {
                var member_id = row.GetValue<int>("member_id");
                //更新登录时间
                mysession.Execute("update onlineuser set login_time = dateof(now()) where auth_token='" + token + "';");
                return member_id;
            }
            throw new AuthenticationException("Token非法");
        }
        public List<MemberModel> GetOnlineUsers()
        {
            if (mysession == null)
                mysession = Connect("121.41.46.175");
            var rs = mysession.Execute("SELECT * FROM onlineusers");
            List<MemberModel> result = new List<MemberModel>();
            //Iterate through the RowSet
            foreach (var row in rs)
            {
                MemberModel mm = new MemberModel();
                mm.email = row.GetValue<string>("email");
                mm.cellphone = row.GetValue<string>("cellphone");
                mm.token = row.GetValue<string>("auth_token");
                result.Add(mm);
            }
            return result;
        }
        public void Close()
        {
            mycluster.Shutdown();
        }

    
    }
}