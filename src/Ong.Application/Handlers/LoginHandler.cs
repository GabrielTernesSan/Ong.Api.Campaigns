using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain.Repositories;
using Ong.Domain.Services;

namespace Ong.Application.Handlers
{
    public class LoginHandler : IRequestHandler<LoginRequest, Response>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginHandler(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<Response> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                response.AddError("Email ou senha inválidos.");
                return response;
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                response.AddError("Email ou senha inválidos.");
                return response;
            }

            var token = _tokenService.GenerateToken(user);

            response.SetResult(new { Token = token });

            return response;
        }
    }
}
