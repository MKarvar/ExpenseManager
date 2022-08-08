using Common.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebUtilities.PagingResult
{
    public static class UriServiceConfigureExtension
    {
        public static IServiceCollection AddUriService(this IServiceCollection services)
        {

            Assert.NotNull(services, nameof(services));
            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
            return services;
        }
    }
}
