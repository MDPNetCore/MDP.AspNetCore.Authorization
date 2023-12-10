using System;

namespace MDP.RoleAccesses.Lab
{
    public class Program
    {
        // Methods
        public static void Run(RoleAccessContext roleAccessContext)
        {
            #region Contracts

            if (roleAccessContext == null) throw new ArgumentException($"{nameof(roleAccessContext)}=null");

            #endregion

            // Display
            Console.WriteLine("12345");
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
