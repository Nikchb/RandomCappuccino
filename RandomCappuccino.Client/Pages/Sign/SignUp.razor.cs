using Microsoft.AspNetCore.Components;
using RandomCappuccino.Shared;
using System.Threading.Tasks;
using static RandomCappuccino.Shared.SignService;

namespace RandomCappuccino.Client.Pages.Sign
{
    public partial class SignUp
    {
        [Inject]
        public SignServiceClient SignService { get; set; }

        private string Email { get; set; } = "";
        private string Password { get; set; } = "";

        private async Task SignUpAsync()
        {                     
            var responce = await SignService.SignUpAsync(new SignRequest { Email = Email, Password = Password });
            if (responce.Succeed)
            {
                AuthenticationService.SetToken(responce.Token);
                NavigationManager.NavigateTo("/");
            }
            MessageManager.UpdateErrorMessages(responce.Messages);
        }
    }
}
