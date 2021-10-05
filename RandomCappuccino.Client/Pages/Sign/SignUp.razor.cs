using Microsoft.AspNetCore.Components;
using RandomCappuccino.Client.Services;
using RandomCappuccino.Shared;
using System.Threading.Tasks;
using static RandomCappuccino.Shared.SignService;

namespace RandomCappuccino.Client.Pages.Sign
{
    public partial class SignUp
    {
        [Inject]
        public AuthenticationService authenticationService { get; set; }

        [Inject]
        public SignServiceClient SignService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private string Email { get; set; } = "";
        private string Password { get; set; } = "";

        private async Task SignUpAsync()
        {
            var responce = await SignService.SignUpAsync(new SignRequest { Email = Email, Password = Password });
            if (responce.Succeed)
            {
                authenticationService.SetToken(responce.Token);
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
