using System.Threading.Tasks;
using CommunicationLibrary.Models.Responses;

namespace CommunicationLibrary.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetApiKeyJwt(string apiKey);
    }
}