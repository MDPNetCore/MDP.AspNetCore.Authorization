using CLK.Caching.Memory;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    [MDP.Registration.Service<PermissionRepository>()]
    public class CachePermissionRepository : PermissionRepository
    {
        // Fields
        private readonly ExpirationMemoryCache _permissionListCache = new ExpirationMemoryCache();

        private readonly PermissionRepository _permissionRepository = null;


        // Constructors
        public CachePermissionRepository(PermissionRepository permissionRepository)
        {
            #region Contracts

            if (permissionRepository == null) throw new ArgumentException($"{nameof(permissionRepository)}=null");

            #endregion

            // Default
            _permissionRepository = permissionRepository;
        }


        // Methods
        public List<Permission> FindAllByRole(string role, string resourceProvider, string resourceType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(role) == true) throw new ArgumentException($"{nameof(role)}=null");
            if (string.IsNullOrEmpty(resourceProvider) == true) throw new ArgumentException($"{nameof(resourceProvider)}=null");
            if (string.IsNullOrEmpty(resourceType) == true) throw new ArgumentException($"{nameof(resourceType)}=null");

            #endregion

            // CacheKey
            var cacheKey = $"{role}/{resourceProvider}/{resourceType}";
            if (string.IsNullOrEmpty(cacheKey) == true) throw new InvalidOperationException($"{nameof(cacheKey)}=null");

            // PermissionList
            List<Permission> permissionList = _permissionListCache.GetValue(cacheKey, () => 
            {
                // FindAll
                permissionList = _permissionRepository.FindAllByRole(role, resourceProvider, resourceType);
                if (permissionList == null) throw new InvalidOperationException($"{nameof(permissionList)}=null");

                // Return
                return permissionList; 
            });
            if (permissionList == null) throw new InvalidOperationException($"{nameof(permissionList)}=null");

            // FindAll
            return permissionList.FindAll(o => o.Role == role && o.ResourceProvider == resourceProvider && o.ResourceType == resourceType).ToList();
        }
    }
}
