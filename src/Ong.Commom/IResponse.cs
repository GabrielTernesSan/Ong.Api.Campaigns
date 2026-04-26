namespace Ong.Commom
{
    public interface IResponse
    {
        List<string> Erros { get; }
        bool HasErrors { get; }
    }
}
