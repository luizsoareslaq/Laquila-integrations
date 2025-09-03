using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiLogs : BaseEntity
    {
        public LaqApiLogs(Guid? apiUserId
                        , string method
                        , string endpoint
                        , string queryString
                        , string requestPayload
                        , string responsePayload
                        , int statusCode
                        , string ipAddress
                        , string userAgent
                        , int executionTimeMs
                        , DateTime createdAt)
        {
            this.ApiUserId = apiUserId;
            this.Method = method;
            this.Endpoint = endpoint;
            this.QueryString = queryString;
            this.RequestPayload = requestPayload;
            this.ResponsePayload = responsePayload;
            this.StatusCode = statusCode;
            this.IpAddress = ipAddress;
            this.UserAgent = userAgent;
            this.ExecutionTimeMs = executionTimeMs;
            this.CreatedAt = createdAt;
        }
        [Column("api_user_id")]
        public Guid? ApiUserId { get; set; }
        [Column("method")]
        public string Method { get; set; }
        [Column("endpoint")]
        public string Endpoint { get; set; }
        [Column("query_string")]
        public string QueryString { get; set; }
        [Column("request_payload")]
        public string RequestPayload { get; set; }
        [Column("response_payload")]
        public string ResponsePayload { get; set; }
        [Column("status_code")]
        public int StatusCode { get; set; }
        [Column("ip_address")]
        public string IpAddress { get; set; }
        [Column("user_agent")]
        public string UserAgent { get; set; }
        [Column("execution_time_ms")]
        public int ExecutionTimeMs { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual LaqApiUsers? User { get; set; }
    }
}