using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Security.Authentication;

namespace EverFresh.Model
{
    public class MemberModel
    {
        public int member_id { get; set; }
        public string fullname { get; set; }
        public string enc_password { get; set; }
        public string email { get; set; }
        public string cellphone { get; set; }

        public MemberModel() { }

        public static MemberModel GetMember(int member_id)
        {
            return new MemberModel();
        }
        public static MemberModel GetMember(string token)
        {
            return new MemberModel();
        }
        public static bool SignUp(string email, string cellphone, string password)
        {
            SqlDataObject dbo = new SqlDataObject();
            dbo.SqlComm = "select * from t_member where member_id = -1";
            DataTable dt = dbo.GetDataTable();
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            dr["email"] = email;
            dr["cellphone"] = cellphone;
            dr["password"] = Common.Encrypt(password);
            dbo.Update(dt);
            return true;
        }
        public static MemberModel SignIn(string email, string cellphone, string password)
        {
            SqlDataObject dbo = new SqlDataObject();
            dbo.SqlComm = "select * from t_member where cellphone = @cellphone";
            DataTable dt = dbo.GetDataTable(new System.Data.SqlClient.SqlParameter("@cellphone", email));
            if (dt.Rows.Count == 0)
                return null;//没找到
            DataRow dr = dt.Rows[0];
            MemberModel mm = new MemberModel();
            mm.cellphone = dr["cellphone"].ToString();
            mm.email = dr["email"].ToString();
            mm.enc_password = dr["enc_password"].ToString();
            if (Common.PasswordCompare(mm.enc_password,password))
            {
                return mm;
            }
            else
            {
                throw new AuthenticationException();
            }
        }
    }
}