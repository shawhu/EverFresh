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
        private ISession Connect(String node)
        {
            mycluster = Cluster.Builder()
                   .AddContactPoint(node)
                   .WithCredentials("ttqdx","?r3>4(7f")
                   .Build();
            return mycluster.Connect("mykeyspace");
        }
        public List<MemberModel> GetOnlineUsers()
        {
            if (mysession == null)
            {
                mysession = Connect("121.41.46.175");
            }
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