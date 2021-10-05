using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RandomCappuccino.Server.Services
{
    public class ServiceBase
    {
        protected ServiceResponse ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            if(Validator.TryValidateObject(model, context, validationResults, true))
            {
                return Accept();
            }
            return Decline(validationResults.Select(v => v.ErrorMessage).ToArray());

        }

        public ServiceResponse Accept()
        {
            return new ServiceResponse(true);
        }

        public ServiceResponse Decline()
        {
            return new ServiceResponse(false);
        }

        public ServiceResponse Decline(params string[] messages)
        {
            return new ServiceResponse(messages);
        }

        public ServiceContentResponse<T> Accept<T>(T response)
        {
            return new ServiceContentResponse<T>(response);
        }

        public ServiceContentResponse<T> Decline<T>()
        {
            return new ServiceContentResponse<T>(false);
        }

        public ServiceContentResponse<T> Decline<T>(params string[] messages)
        {
            return new ServiceContentResponse<T>(messages);
        }
    }
}
