using Microsoft.AspNetCore.Components;
using RandomCappuccino.Client.Services;
using RandomCappuccino.Shared;
using System.Threading.Tasks;
using static RandomCappuccino.Shared.SignService;

namespace RandomCappuccino.Client.Pages.Sign
{
    public partial class SignIn
    {
        [Inject]
        public SignServiceClient SignService { get; set; }

        private string Email { get; set; } = "";
        private string Password { get; set; } = "";

        protected override void OnParametersSet()
        {
            if (AuthenticationService.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/account");
            }
        }

        private async Task SignInAsync()
        {          
            var responce = await SignService.SignInAsync(new SignRequest { Email = Email, Password = AppFunctions.HashPassword(Password) });
            if (responce.Succeed)
            {
                AuthenticationService.SetToken(responce.Token);
                NavigationManager.NavigateTo("/account");
            }
            MessageManager.UpdateErrorMessages(responce.Messages);
        }
    }
}
