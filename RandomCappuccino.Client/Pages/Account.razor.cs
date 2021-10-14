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

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    User = response.UserInfo;
                    ResetChanges();                    
                }                
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
            try
            {
                var response = await UserService.UpdateUserInfoAsync(new UpdateUserInfoRequest { Email = Email });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    User = response.UserInfo;
                    ResetChanges();
                    MessageManager.UpdateSuccessMessages("User information is successfully updated");
                }
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
            }
        }

        private async Task UpdateUserPassword()
        {
            try
            {

                var response = await UserService.UpdateUserPasswordAsync(new UpdateUserPasswordRequest { CurrentPassword = CurrentPassword, NewPassword = NewPassword });

                MessageManager.UpdateErrorMessages(response.Messages);
                if (response.Succeed)
                {
                    ResetChanges();
                    MessageManager.UpdateSuccessMessages("User password is successfully changed");
                }
                
            }
            catch (RpcException ex)
            {
                HandleRPCExection(ex);
           }
        }
    }
}
