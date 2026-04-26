namespace Ong.Domain.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
