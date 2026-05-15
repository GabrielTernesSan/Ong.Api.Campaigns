using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain;
using Ong.Domain.Repositories;

namespace Ong.Application.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, Response>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user != null)
                user.UpdateEmail(request.Email);
            else
                user = new User(request.Id, request.Name, request.Email);

            await _userRepository.CreateAsync(user);

            return new Response();
        }
    }
}
