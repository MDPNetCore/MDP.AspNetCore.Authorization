﻿using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    public class MemoryPermissionRepositoryFactory : Factory<IServiceCollection, MemoryPermissionRepositoryFactory.Setting>
    {
        // Constructors
        public MemoryPermissionRepositoryFactory() : base("MDP.RoleAccesses", "MemoryPermissionRepository") { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, Setting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // PermissionList
            var permissionList = new List<MDP.RoleAccesses.Permission>();
            foreach (var permissionSetting in setting.Permissions)
            {
                // Require
                if (string.IsNullOrEmpty(permissionSetting.Role) == true) throw new InvalidOperationException($"{nameof(permissionSetting.Role)}=null");
                if (string.IsNullOrEmpty(permissionSetting.ResourceProvider) == true) throw new InvalidOperationException($"{nameof(permissionSetting.ResourceProvider)}=null");
                if (string.IsNullOrEmpty(permissionSetting.ResourceType) == true) throw new InvalidOperationException($"{nameof(permissionSetting.ResourceType)}=null");
                if (string.IsNullOrEmpty(permissionSetting.ResourcePath) == true) throw new InvalidOperationException($"{nameof(permissionSetting.ResourcePath)}=null");

                // Add
                permissionList.Add(new MDP.RoleAccesses.Permission
                (
                    permissionId: Guid.NewGuid().ToString(),
                    role: permissionSetting.Role,
                    resourceProvider: permissionSetting.ResourceProvider,
                    resourceType: permissionSetting.ResourceType,
                    resourcePath: permissionSetting.ResourcePath
                ));
            }

            // MemoryPermissionRepository
            serviceCollection.AddSingleton(new ServiceRegistration()
            {
                BuilderType = typeof(IServiceCollection),
                ServiceType = typeof(PermissionRepository),
                InstanceType = typeof(MemoryPermissionRepository),
                InstanceName = nameof(MemoryPermissionRepository),
                Parameters = new Dictionary<string, object>
                {
                    { "PermissionList" , permissionList}
                },
                Singleton = false,
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

            public string ResourceProvider { get; set; }

            public string ResourceType { get; set; }

            public string ResourcePath { get; set; }
        }
    }
}
