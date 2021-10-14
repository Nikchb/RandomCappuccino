using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace RandomCappuccino.Server.Mapper
{
    public static partial class ServiceProviderExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
