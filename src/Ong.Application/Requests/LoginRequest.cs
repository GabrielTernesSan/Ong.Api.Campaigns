using MediatR;
using Ong.Commom;

namespace Ong.Application.Requests
{
    public class LoginRequest : IRequest<Response>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
