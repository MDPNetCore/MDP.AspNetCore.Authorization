using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    [MDP.Registration.Factory<IServiceCollection, MemoryPermissionRepositorySetting>("MDP.RoleAccesses", "MemoryPermissionRepository")]
    public class MemoryPermissionRepositoryFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, MemoryPermissionRepositorySetting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // PermissionSettingList
            foreach (var permissionSetting in setting.Permissions)
            {
                // Properties
                if (string.IsNullOrEmpty(permissionSetting.PermissionId) == true) permissionSetting.PermissionId = Guid.NewGuid().ToString();
                if (string.IsNullOrEmpty(permissionSetting.Role) == true) throw new InvalidOperationException($"{nameof(permissionSetting.Role)}=null");
                if (string.IsNullOrEmpty(permissionSetting.ResourceProvider) == true) throw new InvalidOperationException($"{nameof(permissionSetting.ResourceProvider)}=null");
                if (string.IsNullOrEmpty(permissionSetting.ResourceType) == true) throw new InvalidOperationException($"{nameof(permissionSetting.ResourceType)}=null");
                if (string.IsNullOrEmpty(permissionSetting.ResourcePath) == true) throw new InvalidOperationException($"{nameof(permissionSetting.ResourcePath)}=null");
            }

            // PermissionList
            var permissionList = new List<MDP.RoleAccesses.Permission>();
            foreach (var permissionSetting in setting.Permissions)
            {
                permissionList.Add(new MDP.RoleAccesses.Permission(
                    permissionId: permissionSetting.PermissionId,
                    role: permissionSetting.Role,
                    resourceProvider: permissionSetting.ResourceProvider,
                    resourceType: permissionSetting.ResourceType,
                    resourcePath: permissionSetting.ResourcePath
                ));
            }

            // MemoryPermissionRepository
            serviceCollection.AddTransient<PermissionRepository>((serviceProvider) => 
            {                
                return new MemoryPermissionRepository(permissionList); 
            });
        }


        // Class
        public class MemoryPermissionRepositorySetting
        {
            // Properties
            public List<Permission> Permissions { get; set; } = null;
        }

        public class Permission
        {
            // Properties
            public string PermissionId { get; set; }

            public string Role { get; set; }

            public string ResourceProvider { get; set; }

            public string ResourceType { get; set; }

            public string ResourcePath { get; set; }
        }
    }
}
