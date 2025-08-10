using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Implementations;
using UserManagement.Services.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
        => services.AddScoped<IUserService, UserService>();

    public static IServiceCollection AddDomainAuditServices(this IServiceCollection services)
    => services.AddScoped<IUserAuditService, UserAuditService>();
}
