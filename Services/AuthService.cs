using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PsicoAgenda.Services
{
    public class AuthService
    {
        private const string Tokenkey = "jwt_token";

        public async Task SaveTokenAsync(string token)
        {
            await Microsoft.Maui.Storage.SecureStorage.SetAsync(Tokenkey, token);            
        }

        public async Task<string> GetTokenAsync()
        {
            return await Microsoft.Maui.Storage.SecureStorage.GetAsync(Tokenkey);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        public Task LogoutAsync()
        {
            Microsoft.Maui.Storage.SecureStorage.Remove(Tokenkey);
            return Task.CompletedTask;
        }
    }
}
