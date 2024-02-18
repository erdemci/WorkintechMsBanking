
namespace MsBanking.Common.Helper
{
    public interface IHttpClientHandler
    {
        Task<string> GetStringAsync(string url);
    }
}