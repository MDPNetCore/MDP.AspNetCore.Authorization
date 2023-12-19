using MDP.Registration;
using MDP.RoleAccesses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.AspNetCore.Authorization.RoleAccesses
{
    public class MemoryAccessResultRepositoryFactory : Factory<WebApplicationBuilder, MemoryAccessResultRepositoryFactory.Setting>
    {
        // Constructors
        public MemoryAccessResultRepositoryFactory() : base("Authorization", "RoleAccesses") { }


        // Methods
        public override void ConfigureService(WebApplicationBuilder applicationBuilder, Setting setting)
        {
            #region Contracts

            if (applicationBuilder == null) throw new ArgumentException($"{nameof(applicationBuilder)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // MemoryAccessResultRepository
            applicationBuilder.Services.AddTransient<AccessResultRepository>((serviceProvider) =>
            {
                // AccessResultRepository
                AccessResultRepository accessResultRepository = null;
                accessResultRepository = new MemoryAccessResultRepository();

                // Return
                return accessResultRepository;
            });
        }


        // Class
        public class Setting
        {
            
        }
    }
}
