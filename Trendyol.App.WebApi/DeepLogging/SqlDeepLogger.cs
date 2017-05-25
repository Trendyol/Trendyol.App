using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Trendyol.App.WebApi.DeepLogging
{
    public class SqlDeepLogger : IDeepLogger
    {
        private readonly string _connectionString;

        public SqlDeepLogger(string connectionStringName)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ToString();

            CreateDeepLoggingTableIfNotPresent();
        }

        public void Log(string correlationId, DateTime startedOn, DateTime finishedOn, string requestUrl, string requestMethod,
            string requestHeaders, string requestContent, string responseCode, string responseHeaders, string responseContent)
        {
            string query = @"INSERT INTO [dbo].[DeepLogs]
                                   ([CorrelationId]
                                   ,[StartedOn]
                                   ,[FinishedOn]
                                   ,[TookMiliseconds]
                                   ,[RequestUrl]
                                   ,[RequestMethod]
                                   ,[RequestHeaders]
                                   ,[RequestContent]
                                   ,[ResponseCode]
                                   ,[ResponseHeaders]
                                   ,[ResponseContent])
                             VALUES
                                   (@CorrelationId
                                   ,@StartedOn
                                   ,@FinishedOn
                                   ,@TookMiliseconds
                                   ,@RequestUrl
                                   ,@RequestMethod
                                   ,@RequestHeaders
                                   ,@RequestContent
                                   ,@ResponseCode
                                   ,@ResponseHeaders
                                   ,@ResponseContent)";

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@CorrelationId", correlationId);
                cmd.Parameters.AddWithValue("@StartedOn", startedOn);
                cmd.Parameters.AddWithValue("@FinishedOn", finishedOn);
                cmd.Parameters.AddWithValue("@TookMiliseconds", (finishedOn - startedOn).Milliseconds);
                cmd.Parameters.AddWithValue("@RequestUrl", requestUrl);
                cmd.Parameters.AddWithValue("@RequestMethod", requestMethod);
                cmd.Parameters.AddWithValue("@RequestHeaders", requestHeaders);
                cmd.Parameters.AddWithValue("@RequestContent", requestContent);
                cmd.Parameters.AddWithValue("@ResponseCode", responseCode);
                cmd.Parameters.AddWithValue("@ResponseHeaders", responseHeaders);
                cmd.Parameters.AddWithValue("@ResponseContent", responseContent);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        private void CreateDeepLoggingTableIfNotPresent()
        {
            string query = @"if not exists (select * from sys.tables t join sys.schemas s on (t.schema_id = s.schema_id) where s.name = 'dbo' and t.name = 'DeepLogs') CREATE TABLE dbo.DeepLogs
                                                    (
                                                        CorrelationId nvarchar(200) NOT NULL,
                                                        StartedOn datetime NOT NULL,
                                                        FinishedOn datetime NOT NULL,
                                                        TookMiliseconds bigint NOT NULL,
                                                        RequestUrl nvarchar(MAX) NULL,
                                                        RequestMethod nvarchar(50) NULL,
                                                        RequestHeaders nvarchar(MAX) NULL,
                                                        RequestContent nvarchar(MAX) NULL,
                                                        ResponseCode nvarchar(50) NULL,
                                                        ResponseHeaders nvarchar(MAX) NULL,
                                                        ResponseContent nvarchar(MAX) NULL
                                                    )";

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}