﻿using MDP.Registration;
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
    public class RoleAccessContextFactory : Factory<WebApplicationBuilder, RoleAccessContextFactory.Setting>
    {
        // Constructors
        public RoleAccessContextFactory() : base("Authorization", "RoleAccesses") { }


        // Methods
        public override void ConfigureService(WebApplicationBuilder applicationBuilder, Setting setting)
        {
            #region Contracts

            if (applicationBuilder == null) throw new ArgumentException($"{nameof(applicationBuilder)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // RoleAccessContext
            applicationBuilder.Services.TryAddSingleton<RoleAccessContext>();
        }


        // Class
        public class Setting
        {
            
        }
    }
}
