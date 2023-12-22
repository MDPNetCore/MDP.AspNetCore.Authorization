using MDP.Application;
using MDP.Registration;
using MDP.RoleAccesses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authorization.RoleAccesses
{
    public class PermissionRepositoryFactory : Factory<WebApplicationBuilder, PermissionRepositoryFactory.Setting>
    {
        // Constructors
        public PermissionRepositoryFactory() : base("Authorization", "RoleAccesses") { }


        // Methods
        public override void ConfigureService(WebApplicationBuilder applicationBuilder, Setting setting)
        {
            #region Contracts

            if (applicationBuilder == null) throw new ArgumentException($"{nameof(applicationBuilder)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // ApplicationInfo
            var applicationInfo = applicationBuilder.Services.BuildServiceProvider()?.GetRequiredService<ApplicationInfo>();
            if (applicationInfo == null) throw new InvalidOperationException($"{nameof(applicationInfo)}=null");
            if (string.IsNullOrEmpty(applicationInfo.Name) == true) throw new InvalidOperationException($"{nameof(applicationInfo.Name)}=null");

            // MemoryMenuRepository
            if (setting.Permissions != null && setting.Permissions.Count > 0)
            {
                // PermissionList
                var permissionList = new List<MDP.RoleAccesses.Permission>();
                foreach (var permissionSetting in setting.Permissions)
                {
                    permissionList.Add(new MDP.RoleAccesses.Permission()
                    {
                        PermissionId = Guid.NewGuid().ToString(),
                        Role = permissionSetting.Role,
                        ResourceProvider = applicationInfo.Name,
                        ResourceType = permissionSetting.ResourceType,
                        ResourcePath = permissionSetting.ResourcePath
                    });
                }
                permissionList.ForEach(o => Validator.ValidateObject(o, new ValidationContext(o)));

                // Register
                applicationBuilder.Services.AddTransient<PermissionRepository>((serviceProvider) =>
                {
                    // Create
                    PermissionRepository permissionRepository = null;
                    permissionRepository = new MemoryPermissionRepository(permissionList);
                    permissionRepository = new CachePermissionRepository(permissionRepository);

                    // Return
                    return permissionRepository;
                });
            }
        }


        // Class
        public class Setting
        {
            // Properties
            public List<Permission> Permissions { get; set; } = null;
        }

        public class Permission
        {
            // Properties
            public string Role { get; set; }

            public string ResourceType { get; set; }

            public string ResourcePath { get; set; }
        }
    }
}
