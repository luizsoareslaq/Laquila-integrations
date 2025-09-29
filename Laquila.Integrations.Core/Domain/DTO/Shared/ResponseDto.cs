using System.Text.Json.Serialization;

namespace Laquila.Integrations.Core.Domain.DTO.Shared
{
    public class ResponseDto
    {
        [JsonPropertyName("data")]
        public ResponseDataDto? Data { get; set; }

        [JsonPropertyName("errors")]
        public List<ResponseErrorsDto>? Errors { get; set; }
    }

    public class ResponseDataDto
    {
        [JsonPropertyName("status_code")]
        public required string StatusCode { get; set; }

        [JsonPropertyName("message")]
        public required string Message { get; set; }
    }

    public class ResponseErrorsDto
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("entity")]
        public required string Entity { get; set; }

        [JsonPropertyName("key")]
        public required string Key { get; set; }

        [JsonPropertyName("value")]
        public required string Value { get; set; }

        [JsonPropertyName("message")]
        public required string Message { get; set; }
    }
}