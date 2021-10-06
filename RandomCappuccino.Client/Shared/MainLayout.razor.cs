using Microsoft.AspNetCore.Components;
using RandomCappuccino.Client.Services;
using System.Threading.Tasks;

namespace RandomCappuccino.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public AuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private void SignOut()
        {
            AuthenticationService.RemoveToken();
            NavigationManager.NavigateTo("/sign-in");
        }
    }
}
