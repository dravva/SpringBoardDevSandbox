using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using SqlDataProvider.contract;
using System.Configuration;
namespace SqlDataProvider
{
    public static class FeedbackProvider
    {
        private static readonly ConnectionStringSettings ConnectionString = ConfigurationManager.ConnectionStrings["dwlcsql"];

        public static IEnumerable<string> ReadFeedback(int start, int end)
        {
            var connectionFactory = DbProviderFactories.GetFactory(ConnectionString.ProviderName);
            using (var conn = (SqlConnection)connectionFactory.CreateConnection())
            {
                if (conn != null)
                {
                    conn.ConnectionString = ConnectionString.ConnectionString;
                    conn.Open();
                    var ret = new List<string>();
                    //read total number
                    using (var com = new SqlCommand("select count(*) FROM [dbo].feedback", conn))
                    {
                        using (var result = com.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                var total = result.GetInt32(0);
                                ret.Add(string.Format("total={0}", total));
                            }
                        }
                    }

                    using (
                        var con =
                            new SqlCommand(
                                "select * from (SELECT ROW_NUMBER() OVER(ORDER BY time DESC) AS Row,id,[time],[user],[category],[item],[value],[feedback] FROM [dbo].feedback ) as A where A.Row>=@start and A.Row<=@end",
                                conn))
                    {
                        con.Parameters.Add("@start", SqlDbType.Int).Value = start;
                        con.Parameters.Add("@end", SqlDbType.Int).Value = end;
                        using (var result = con.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                var row = result.GetInt64(0);
                                var time = result.GetDateTime(2);
                                var user = result.GetString(3);
                                var category = result.GetString(4);
                                var item = result.GetString(5);
                                var value = result.GetString(6);
                                var feedback = result.GetString(7);

                                ret.Add(string.Format("row {0}: user {1} give feedback {2} on {3}:{4}:{5} at {6}", row,
                                    user,
                                    feedback, category, item, value,
                                    time));
                            }
                        }
                    }
                    return ret;
                }
            }
            return new List<string>();
        }

        public static FeedbackResult GetFeedback(string user, string category, string item)
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
                            "SELECT count(*),feedback FROM [dbo].[feedback] where category=@category and item=@item group by feedback",
                            conn);

                    if (!string.IsNullOrWhiteSpace(user))
                    {
                        com =
                        new SqlCommand(
                            "SELECT count(*),feedback FROM [dbo].[feedback] where category=@category and item=@item and user=@user group by feedback",
                            conn);
                        com.Parameters.Add("@user", SqlDbType.NVarChar).Value = user;
                    }
                    com.Parameters.Add("@category", SqlDbType.NVarChar).Value = category;
                    com.Parameters.Add("@item", SqlDbType.NVarChar).Value = item.Trim();
                    using (var result = com.ExecuteReader())
                    {
                        int total = 0, positive = 0, negative = 0;
                        while (result.Read())
                        {
                            var count = result.GetInt32(0);
                            var feedbackGroup = result.GetString(1);
                            total += count;
                            if (feedbackGroup.Equals("+"))
                            {
                                positive = count;
                            }
                            else if (feedbackGroup.Equals("-"))
                            {
                                negative = count;
                            }
                        }
                        return new FeedbackResult { status = "success", up = positive, down = negative, total = total };
                    }
                }
            }
            return new FeedbackResult { status = "fail" };
        }

        public static FeedbackResult WriteFeedback(string user, string category, string item, string value, string feedback)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                if (string.IsNullOrWhiteSpace(user))
                {
                    user = "undefine";
                }

                var connectionFactory = DbProviderFactories.GetFactory(ConnectionString.ProviderName);
                using (var conn = (SqlConnection)connectionFactory.CreateConnection())
                {
                    if (conn != null)
                    {
                        conn.ConnectionString = ConnectionString.ConnectionString;
                        conn.Open();
                        var com =
                            new SqlCommand(
                                "insert into [dbo].[feedback] values( @id,@time,@user,@category,@item,@value,@feedback )",
                                conn);
                        com.Parameters.Add("@id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                        com.Parameters.Add("@time", SqlDbType.DateTime).Value = DateTime.UtcNow.AddHours(8);
                        com.Parameters.Add("@user", SqlDbType.NVarChar).Value = user;
                        com.Parameters.Add("@category", SqlDbType.NVarChar).Value = category;
                        com.Parameters.Add("@item", SqlDbType.NVarChar).Value = item.Trim();
                        com.Parameters.Add("@value", SqlDbType.NVarChar).Value = value.Trim();
                        com.Parameters.Add("@feedback", SqlDbType.NVarChar).Value = feedback;
                        com.ExecuteNonQuery();

                        //read the feedback
                        com =
                            new SqlCommand(
                                "SELECT count(*),feedback FROM [dbo].[feedback] where category=@category and item=@item group by feedback",
                                conn);
                        com.Parameters.Add("@category", SqlDbType.NVarChar).Value = category;
                        com.Parameters.Add("@item", SqlDbType.NVarChar).Value = item.Trim();
                        using (var result = com.ExecuteReader())
                        {
                            int total = 0, positive = 0, negative = 0;
                            while (result.Read())
                            {
                                var count = result.GetInt32(0);
                                var feedbackGroup = result.GetString(1);
                                total += count;
                                if (feedbackGroup.Equals("+"))
                                {
                                    positive = count;
                                }
                                else if (feedbackGroup.Equals("-"))
                                {
                                    negative = count;
                                }
                            }
                            return new FeedbackResult { status = "success", up = positive, down = negative, total = total };
                        }
                    }
                }
            }
            return new FeedbackResult { status = "fail" };
        }

        public static IList<FeedbackGroupResult> GetGroupedResult()
        {
            var connectionFactory = DbProviderFactories.GetFactory(ConnectionString.ProviderName);
            using (var conn = (SqlConnection)connectionFactory.CreateConnection())
            {
                if (conn != null)
                {
                    conn.ConnectionString = ConnectionString.ConnectionString;
                    conn.Open();
                    var ret = new List<FeedbackGroupResult>();

                    using (
                        var con =
                            new SqlCommand(
                                "select category,item,feedback,count(*) as number from feedback group by category,item,feedback",
                                conn))
                    {
                        using (var result = con.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                var category = result.GetString(0);
                                var item = result.GetString(1);
                                var feedback = result.GetString(2);
                                var number = result.GetInt32(3);

                                ret.Add(new FeedbackGroupResult
                                {
                                    category = category,
                                    feedback = feedback,
                                    item = item,
                                    number = number
                                });
                            }
                        }
                    }
                    return ret;
                }
            }
            return new List<FeedbackGroupResult>();
        }

        public static IList<FeedbackGroupDetail> GetGroupDetail(string category,string item)
        {
            var connectionFactory = DbProviderFactories.GetFactory(ConnectionString.ProviderName);
            using (var conn = (SqlConnection)connectionFactory.CreateConnection())
            {
                if (conn != null)
                {
                    conn.ConnectionString = ConnectionString.ConnectionString;
                    conn.Open();
                    var ret = new List<FeedbackGroupDetail>();

                    using (
                        var com =
                            new SqlCommand(
                                "select [user],[time],[feedback] from feedback where category=@category and item=@item order by [time] desc",
                                conn))

                    {
                        com.Parameters.Add("@category", SqlDbType.NVarChar).Value = category;
                        com.Parameters.Add("@item", SqlDbType.NVarChar).Value = item.Trim();
                        using (var result = com.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                var user = result.GetString(0);
                                var time = result.GetDateTime(1);
                                var feedback = result.GetString(2);

                                ret.Add(new FeedbackGroupDetail
                                {
                                    feedback = feedback,
                                    time = time.ToString(CultureInfo.InvariantCulture),
                                    user = user
                                });
                            }
                        }
                    }
                    return ret;
                }
            }
            return new List<FeedbackGroupDetail>();
        }
    }
}
