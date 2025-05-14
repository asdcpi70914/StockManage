using SRC.Backend.Models.Brain;

namespace SRC.Backend.Models.System
{
    public static class SystemServiceCollection
    {
        public static IServiceCollection AddSystemService(this IServiceCollection services)
        {
            return
                services
                .AddSingleton<ISystemHelper, SystemHelper>()
                ;
            ;
        }
    }
}
