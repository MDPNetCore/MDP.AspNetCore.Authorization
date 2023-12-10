using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    public class MemoryPermissionRepository : PermissionRepository
    {
        // Fields
        private List<Permission> _permissionList = null;


        // Constructors
        public MemoryPermissionRepository(List<Permission> permissionList)
        {
            #region Contracts

            if (permissionList == null) throw new ArgumentException($"{nameof(permissionList)}=null");

            #endregion

            // Default
            _permissionList = permissionList;
        }


        // Methods
        public List<Permission> FindAllByRole(string role, string resourceProvider, string resourceType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(role) == true) throw new ArgumentException($"{nameof(role)}=null");
            if (string.IsNullOrEmpty(resourceProvider) == true) throw new ArgumentException($"{nameof(resourceProvider)}=null");
            if (string.IsNullOrEmpty(resourceType) == true) throw new ArgumentException($"{nameof(resourceType)}=null");

            #endregion

            // FindAll
            return _permissionList.FindAll(o=>o.Role==role && o.ResourceProvider==resourceProvider && o.ResourceType==resourceType).ToList();
        }
    }
}
