using MDP.Application;
using MDP.Registration;
using MDP.RoleAccesses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authorization.RoleAccesses
{
    public class MemoryPermissionRepositoryFactory : Factory<WebApplicationBuilder, MemoryPermissionRepositoryFactory.Setting>
    {
        // Constructors
        public MemoryPermissionRepositoryFactory() : base("Authorization", "RoleAccesses") { }


        // Methods
        public override void ConfigureService(WebApplicationBuilder applicationBuilder, Setting setting)
        {
            #region Contracts

            if (applicationBuilder == null) throw new ArgumentException($"{nameof(applicationBuilder)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Require
            if (setting.Permissions == null) return;
            if (setting.Permissions.Count == 0) return;

            // ApplicationInfo
            var applicationInfo = applicationBuilder.Services.BuildServiceProvider()?.GetRequiredService<ApplicationInfo>();
            if (applicationInfo == null) throw new InvalidOperationException($"{nameof(applicationInfo)}=null");
            if(string.IsNullOrEmpty(applicationInfo.Name)==true) throw new InvalidOperationException($"{nameof(applicationInfo.Name)}=null");

            // PermissionList
            var permissionList = new List<MDP.RoleAccesses.Permission>();
            foreach (var permissionSetting in setting.Permissions)
            {
                // Require
                if (string.IsNullOrEmpty(permissionSetting.Role) == true) throw new InvalidOperationException($"{nameof(permissionSetting.Role)}=null");
                if (string.IsNullOrEmpty(permissionSetting.ResourceType) == true) throw new InvalidOperationException($"{nameof(permissionSetting.ResourceType)}=null");
                if (string.IsNullOrEmpty(permissionSetting.ResourcePath) == true) throw new InvalidOperationException($"{nameof(permissionSetting.ResourcePath)}=null");

                // Add
                permissionList.Add(new MDP.RoleAccesses.Permission
                (
                    permissionId: Guid.NewGuid().ToString(),
                    role: permissionSetting.Role,
                    resourceProvider: applicationInfo.Name,
                    resourceType: permissionSetting.ResourceType,
                    resourcePath: permissionSetting.ResourcePath
                ));
            }

            // MemoryPermissionRepository
            applicationBuilder.Services.AddTransient<PermissionRepository>((serviceProvider) => 
            {
                // PermissionRepository
                PermissionRepository permissionRepository = null;
                permissionRepository = new MemoryPermissionRepository(permissionList);
                permissionRepository = new CachePermissionRepository(permissionRepository);

                // Return
                return permissionRepository;
            });
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
