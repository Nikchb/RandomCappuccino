using Blazored.LocalStorage;
using System.Net.Http;
using System.Threading.Tasks;

namespace RandomCappuccino.Client.Services
{
    public class AuthenticationService
    {
        public delegate void AuthenticationStateHandler();

        public event AuthenticationStateHandler AuthenticationStateChanged;

        private readonly ISyncLocalStorageService localStorage;

        private readonly HttpClient httpClient;

        public AuthenticationService(ISyncLocalStorageService localStorage, HttpClient httpClient)
        {
            this.localStorage = localStorage;
            this.httpClient = httpClient;
            InitializeAuthenticationState();
        }

        private void InitializeAuthenticationState()
        {
            IsAuthenticated = false;
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            
            var token = localStorage.GetItemAsString("token");
            if (token != null)
            {
                IsAuthenticated = true;
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
            if (AuthenticationStateChanged != null)
            {
                AuthenticationStateChanged.Invoke();
            }
        }

        public bool IsAuthenticated { get; private set; } = false;        

        public void SetToken(string token)
        {                      
            localStorage.SetItemAsString("token", token);
            InitializeAuthenticationState();
        }

        public void RemoveToken()
        {            
            localStorage.RemoveItem("token");
            InitializeAuthenticationState();
        }
    }
}
