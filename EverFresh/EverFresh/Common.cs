using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EverFresh.Model;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace EverFresh
{
    public class Common
    {
        /// <summary>
        /// 检查昵称是否已被用掉
        /// </summary>
        /// <param name="nickname"></param>
        public static bool doesNameExist(string nick_name)
        {
            SqlDataObject dbo = new SqlDataObject();
            dbo.SqlComm = "select * from t_member where nick_name = '" + nick_name + "' and (enc_password is not null or wechat_id is not null)";
            DataTable dt = dbo.GetDataTable();
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }
        /// <summary>
        /// 检查Email是否已被用掉
        /// </summary>
        /// <param name="email"></param>
        public static bool doesEmailExist(string email)
        {
            SqlDataObject dbo = new SqlDataObject();
            dbo.SqlComm = "select * from t_member where email = '" + email + "' and (enc_password is not null or wechat_id is not null)";
            DataTable dt = dbo.GetDataTable();
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }
        /// <summary>
        /// 检查Cellphone是否已被用掉
        /// </summary>
        /// <param name="nickname"></param>
        public static bool doesCellphoneExist(string cellphone)
        {
            SqlDataObject dbo = new SqlDataObject();
            dbo.SqlComm = "select * from t_member where cellphone = '" + cellphone + "' and (enc_password is not null or wechat_id is not null)";
            DataTable dt = dbo.GetDataTable();
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }
        /// <summary>
        /// 检查新密码是否符合规则
        /// </summary>
        /// <param name="password"></param>
        public static bool CheckPasswordSecurity(string pwd)
        {
            if (pwd.Length < 6)
                return false;
            return true;
        }

        //Redis operations

        /// <summary>
        /// 得到所有在线用户列表
        /// </summary>
        /*
        public static List<MemberModel> GetOnlineUserList()
        {
            //从Redis服务器获取
            RedisClient client = new RedisClient(ConfigurationManager.AppSettings["RedisHost"], int.Parse(ConfigurationManager.AppSettings["RedisPort"]));
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RedisAuth"]))
                client.Password = ConfigurationManager.AppSettings["RedisAuth"];
            var key = ConfigurationManager.AppSettings["OnlineUserSet"];
            var redislist = client.As<MemberModel>().Lists[key];
            List<MemberModel> OnlineUsers = redislist.GetAll();
            if (OnlineUsers == null)
                OnlineUsers = new List<MemberModel>();
            return OnlineUsers.Where(p => p.token_expire_date > Common.DateTime2Double(DateTime.Now)).ToList();
        }
        */
        
        /// <summary>
        /// 检查Email合法性
        /// </summary>
        /// <param name="address"></param>
        public static bool CheckValidEmail(string address)
        {
            if (String.IsNullOrEmpty(address))
                return false;

            return Regex.IsMatch(address, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                                RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static string Encrypt(string plaintext)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] s = sha.ComputeHash(UnicodeEncoding.UTF8.GetBytes(plaintext));
            return (BitConverter.ToString(s)).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 判断密文是否和明文匹配
        /// </summary>
        /// <param name="encrypted_password"></param>
        /// <param name="plaintext_password"></param>
        public static bool PasswordCompare(string encrypted_password, string plaintext_password)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] s = sha.ComputeHash(UnicodeEncoding.UTF8.GetBytes(plaintext_password));
            string temp = BitConverter.ToString(s).Replace("-", "").ToLower();
            return string.Equals(temp, encrypted_password, StringComparison.CurrentCultureIgnoreCase);
        }

        public static DateTime Double2DateTime(double interval)
        {
            DateTime dt = DateTime.Parse("1970-1-1 00:00:00 +0000");
            if (DateTime.Now.IsDaylightSavingTime())
                interval += 3600;
            return dt.Add(TimeSpan.FromSeconds(interval));
        }

        public static double DateTime2Double(DateTime dt)
        {
            DateTime now = DateTime.Parse("1970-1-1 00:00:00 +0000");
            double seconds = (dt - now).TotalSeconds;
            if (DateTime.Now.IsDaylightSavingTime())
                seconds -= 3600;
            return seconds;
        }

        internal static bool SendSMSCode(string message, string cellphone_number)
        {
            try
            {
                CSharpSmsApi.SMS.sendSms("da1094c41e5a343400992f1b15495dea", message, cellphone_number);
            }
            catch
            {
                return false;
            }
            return true;
        }

        internal static void SendEmailCode(string code)
        {
            //throw new NotImplementedException();
        }

        internal static string Random4DigitCode()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int numIterations = 0;
            numIterations = rand.Next(1000, 9999);
            return numIterations.ToString();
        }

        
    }
}