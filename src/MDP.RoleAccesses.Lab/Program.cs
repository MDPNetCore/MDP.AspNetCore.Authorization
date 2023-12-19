using System;

namespace MDP.RoleAccesses.Lab
{
    public class Program
    {
        // Methods
        public static void Run(RoleAccessContext accessContext)
        {
            #region Contracts

            if (accessContext == null) throw new ArgumentException($"{nameof(accessContext)}=null");

            #endregion

            // True
            Console.WriteLine("HasAccess=True");
            HasAccess(accessContext, "Admin", "MDP.RBAC.Service/Menu/Users/Add");
            HasAccess(accessContext, "Admin", "MDP.RBAC.Service/Menu/Roles/Add");
            HasAccess(accessContext, "User", "MDP.RBAC.Service/Menu/Users/List");
            HasAccess(accessContext, "User", "MDP.RBAC.Service/Menu/Users/Password/Reset");
            Console.WriteLine();

            // False
            Console.WriteLine("HasAccess=False");
            HasAccess(accessContext, "Admin", "MDP.RBAC.Service/Menu/Users/List");
            HasAccess(accessContext, "Admin", "MDP.RBAC.Service/Menu/Users/Password");
            HasAccess(accessContext, "User", "MDP.RBAC.Service/Menu/Users/Add");
            HasAccess(accessContext, "User", "MDP.RBAC.Service/Menu/Roles/Add");
            Console.WriteLine();
        }

        private static void HasAccess(RoleAccessContext accessContext, string role, string resourceUri)
        {
            #region Contracts

            if (accessContext == null) throw new ArgumentException($"{nameof(accessContext)}=null");
            if (string.IsNullOrEmpty(role) == true) throw new ArgumentException($"{nameof(role)}=null");
            if (string.IsNullOrEmpty(resourceUri) == true) throw new ArgumentException($"{nameof(resourceUri)}=null");

            #endregion

            // Display
            Console.WriteLine($"HasAccess={accessContext.HasAccess(role, resourceUri)}, Role={role}, Resource={resourceUri}");
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
