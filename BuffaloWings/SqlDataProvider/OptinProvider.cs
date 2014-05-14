using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SqlDataProvider
{
    public static class OptinProvider
    {
        private static readonly ConnectionStringSettings ConnectionString = ConfigurationManager.ConnectionStrings["dwlcsql"];

        public static void WriteOptin(string user, string token, string from)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var connectionFactory = DbProviderFactories.GetFactory(ConnectionString.ProviderName);
                using (var conn = (SqlConnection)connectionFactory.CreateConnection())
                {
                    if (conn != null)
                    {
                        conn.ConnectionString = ConnectionString.ConnectionString;
                        conn.Open();
                        var com =
                            new SqlCommand(
                                "if not exists (select 1 from token where id = @user) insert into token values(@user, @time,@from,@token) else update token set time = @time, token=@token where id = @user",
                                conn);
                        com.Parameters.Add("@user", SqlDbType.NVarChar).Value = user;
                        com.Parameters.Add("@time", SqlDbType.DateTime).Value = DateTime.UtcNow.AddHours(8);
                        com.Parameters.Add("@from", SqlDbType.NVarChar).Value = from;
                        com.Parameters.Add("@token", SqlDbType.NVarChar).Value = token;
                        com.ExecuteNonQuery();
                    }
                }
            }
        }

        public static int ReadOptin(string from)
        {
            var connectionFactory = DbProviderFactories.GetFactory(ConnectionString.ProviderName);
            using (var conn = (SqlConnection)connectionFactory.CreateConnection())
            {
                if (conn != null)
                {
                    conn.ConnectionString = ConnectionString.ConnectionString;
                    conn.Open();
                    var com = new SqlCommand(
                        "select count(*) FROM [dbo].token",
                        conn);
                    if (!from.Equals("all"))
                    {
                        com = new SqlCommand(
                            "select count(*) FROM [dbo].token where [from]=@from",
                            conn);
                        com.Parameters.Add("@from", SqlDbType.NVarChar).Value = from;
                    }

                    using (var result = com.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            var number = result.GetInt32(0);
                            return number;
                        }
                    }
                }
            }
            return 0;
        }
    }
}
