using MDP.Application;
using MDP.RoleAccesses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authorization.RoleAccesses
{
    public class RoleAccessRequirementHandler : AuthorizationHandler<RoleAccessRequirement>
    {
        // Fields
        private readonly RoleAccessContext _accessContext = null;

        private readonly ApplicationInfo _applicationInfo = null;


        // Constructors
        public RoleAccessRequirementHandler(RoleAccessContext accessContext, ApplicationInfo applicationInfo)
        {
            #region Contracts

            if (accessContext == null) throw new ArgumentException($"{nameof(accessContext)}=null");
            if (applicationInfo == null) throw new ArgumentException($"{nameof(applicationInfo)}=null");

            #endregion

            // Default
            _accessContext = accessContext;
            _applicationInfo = applicationInfo;
        }


        // Methods
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleAccessRequirement requirement)
        {
            #region Contracts

            if (context == null) throw new ArgumentException($"{nameof(context)}=null");
            if (requirement == null) throw new ArgumentException($"{nameof(requirement)}=null");

            #endregion

            // HttpContext
            var httpContext = context.Resource as HttpContext;
            if (httpContext == null) return Task.CompletedTask;

            // Identity
            var identity = httpContext.User?.Identity as ClaimsIdentity;
            if (identity == null) return Task.CompletedTask;
            if (identity.IsAuthenticated == false) return Task.CompletedTask;

            // RoleList
            var roleList = identity.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).ToList();
            if (roleList == null) return Task.CompletedTask;
            if (roleList.Count == 0) return Task.CompletedTask;            

            // HasAccess
            if (_accessContext.HasAccess(roleList, _applicationInfo.Name, "Path", httpContext.Request.Path) == true)
            {
                context.Succeed(requirement);
            }

            // Return
            return Task.CompletedTask;
        }
    }
}
