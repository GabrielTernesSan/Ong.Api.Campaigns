using MediatR;
using Ong.Commom;
using System.Text.Json.Serialization;

namespace Ong.Application.Requests
{
    public class UpdateOutboxRequest : IRequest<Response>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string? Error { get; set; }
    }
}
