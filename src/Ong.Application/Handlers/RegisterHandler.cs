using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain;
using Ong.Domain.Enums;
using Ong.Domain.Repositories;

namespace Ong.Application.Handlers
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, Response>
    {
        private readonly IUserRepository _userRepository;

        public RegisterHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                response.AddError("Já existe um usuário com este email.");
                return response;
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User(Guid.NewGuid(), request.Name, request.Email, passwordHash, ERole.Doador.ToString());

            await _userRepository.CreateAsync(user);

            response.SetResult(new { user.Id, user.Name, user.Email, user.Role });

            return response;
        }
    }
}
