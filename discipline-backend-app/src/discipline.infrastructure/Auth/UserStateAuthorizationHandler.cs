using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.infrastructure.Auth;

internal sealed class UserStateAuthorizationHandler(
    IServiceProvider serviceProvider) : AuthorizationHandler<UserStateRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserStateRequirement requirement)
    {
        if (!(Ulid.TryParse(context?.User?.Identity?.Name, out var userId)))
        {
            context.Succeed(requirement);
            return;
        }

        using var scope = serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IReadUserRepository>();
        var user = await userRepository.GetByIdAsync(new UserId(userId));
        if (!user.IsUserActive())
        {
            context.Fail();
            return;
        }
        context.Succeed(requirement);
    }
}