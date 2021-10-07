using Grpc.Core;
using Microsoft.AspNetCore.Components;
using RandomCappuccino.Shared;
using System.Threading.Tasks;
using static RandomCappuccino.Shared.UserService;

namespace RandomCappuccino.Client.Pages
{
    public partial class Account
    {
        [Inject]
        public UserServiceClient UserService { get; set; }

        private UserInfo User { get; set; }

        private string Email { get; set; } = "";

        private string CurrentPassword { get; set; } = "";

        private string NewPassword { get; set; } = "";


        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            try
            {
                var response = await UserService.GetUserInfoAsync(new UserInfoRequest());
                if (response.Succeed)
                {
                    User = response.UserInfo;
                    ResetChanges();
                }
                UpdateErrorMessages(response.Messages);
            }
            catch (RpcException ex) 
            {
                HandleRPCExection(ex);
            }
            
        }

        private void ResetChanges()
        {
            Email = User.Email;
            CurrentPassword = "";
            NewPassword = "";
        }

        private async Task UpdateUserInfo()
        {
            var response = await UserService.UpdateUserInfoAsync(new UpdateUserInfoRequest { Email = Email });           

            if (response.Succeed)
            {
                User = response.UserInfo;
                ResetChanges();
            }
            UpdateErrorMessages(response.Messages);
        }

        private async Task UpdateUserPassword()
        {
            var response = await UserService.UpdateUserPasswordAsync(new UpdateUserPasswordRequest { CurrentPassword = CurrentPassword, NewPassword = NewPassword });

            if (response.Succeed)
            {                
                ResetChanges();
            }
            UpdateErrorMessages(response.Messages);
        }
    }
}
