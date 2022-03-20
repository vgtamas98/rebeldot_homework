using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebAPI.Interfaces;

namespace WebAPI.API.Middleware.Auth
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PrivilegeRequirement>
    {
        private readonly IUserService _userService;

        public PermissionAuthorizationHandler(IUserService userService)
        {
            _userService = userService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PrivilegeRequirement requirement)
        {
            var claimsPrincipal = context.User;
            
            if (!AreClaimsValid(claimsPrincipal))
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            //returns claims as an array, the first element ( [0] ) is the "sub", which contains the user's email
            var email = claimsPrincipal.Claims.ToList()[0].Value;
            var user = _userService.GetByEmail(email);

            var userClaimsModel = ExtractUserClaims(claimsPrincipal);

        
            if (context.Resource is HttpContext httpContext)
            {

                httpContext.Items.Add("userSession", user);
                Console.WriteLine(httpContext.Items["userSession"]);
            }
            if (userClaimsModel.Claim_Roles != null)
            {
                ValidateUserPrivileges(context, requirement, userClaimsModel.Claim_Roles);
            } else
            {
                context.Fail();
            }
            await Task.CompletedTask;
        }

        private void ValidateUserPrivileges(AuthorizationHandlerContext context, PrivilegeRequirement requirement, IEnumerable<string> userClaimRoles)
        {
            if (requirement.Role == Policies.All)
                if (userClaimRoles.Contains(Policies.User) || userClaimRoles.Contains(Policies.Admin))
                {
                    context.Succeed(requirement);
                    return;
                }

            if (userClaimRoles.Contains(requirement.Role))
                context.Succeed(requirement);
            else
                context.Fail();
        }

        private bool AreClaimsValid(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal != null &&
                   claimsPrincipal.Identity.IsAuthenticated &&
                   claimsPrincipal.HasClaim(c => c.Type.Equals(AuthorizationConstants.ClaimRole)) &&
                   claimsPrincipal.HasClaim(c => c.Type.Equals(AuthorizationConstants.ClaimSubject));
        }

        private UserClaimModel ExtractUserClaims(ClaimsPrincipal claimsPrincipal)
        {
            var claimRoleValues = claimsPrincipal
                .FindAll(c => c.Type.Equals(AuthorizationConstants.ClaimRole))
                .Select(c => c.Value);
            Guid.TryParse(claimsPrincipal
                .FindFirst(c => c.Type.Equals(AuthorizationConstants.ClaimSubject)).Value, out Guid claimSubjectValue);

            return new UserClaimModel
            {
                Claim_UserId = claimSubjectValue,
                Claim_Roles = claimRoleValues
            };
        }
    }
}
