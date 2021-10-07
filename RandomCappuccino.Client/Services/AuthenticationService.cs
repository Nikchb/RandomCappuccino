using Blazored.LocalStorage;
using System.Net.Http;
using System.Threading.Tasks;

namespace RandomCappuccino.Client.Services
{
    public class AuthenticationService
    {
        private readonly ISyncLocalStorageService localStorage;

        private readonly HttpClient httpClient;

        public AuthenticationService(ISyncLocalStorageService localStorage, HttpClient httpClient)
        {
            this.localStorage = localStorage;
            this.httpClient = httpClient;
            InitializeAuthenticationStatus();
        }

        private void InitializeAuthenticationStatus()
        {
            IsAuthenticated = false;
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            
            var token = localStorage.GetItemAsString("token");
            if (token != null)
            {
                IsAuthenticated = true;
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }

        public bool IsAuthenticated { get; private set; } = false;        

        public void SetToken(string token)
        {                      
            localStorage.SetItemAsString("token", token);
            InitializeAuthenticationStatus();
        }

        public void RemoveToken()
        {            
            localStorage.RemoveItem("token");
            InitializeAuthenticationStatus();
        }
    }
}
