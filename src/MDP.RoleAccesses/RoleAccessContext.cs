using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    [MDP.Registration.Service<RoleAccessContext>(singleton: true)]
    public class RoleAccessContext
    {
        // Fields
        private readonly PermissionRepository _permissionRepository = null;

        private readonly AccessResultRepository _accessResultRepository = null;


        // Constructors
        public RoleAccessContext(PermissionRepository permissionRepository, AccessResultRepository accessResultRepository)
        {
            #region Contracts

            if (permissionRepository == null) throw new ArgumentException($"{nameof(permissionRepository)}=null");

            #endregion

            // Default
            _permissionRepository = permissionRepository;
            _accessResultRepository = accessResultRepository ?? new CacheAccessResultRepository();
        }


        // Methods
        public bool HasAccess(string role, string resourceUri)
        {
            #region Contracts

            if (string.IsNullOrEmpty(role) == true) throw new ArgumentException($"{nameof(role)}=null");
            if (string.IsNullOrEmpty(resourceUri) == true) throw new ArgumentException($"{nameof(resourceUri)}=null");

            #endregion

            // ResourceSectionList
            var resourceSectionList = resourceUri.Split("/");
            if (resourceSectionList.Length < 3) throw new InvalidOperationException($"{nameof(resourceSectionList.Length)}<3");

            // ResourceProvider
            var resourceProvider = resourceSectionList[0];
            if (string.IsNullOrEmpty(resourceProvider) == true) throw new InvalidOperationException($"{nameof(resourceProvider)}=null");

            // ResourceType
            var resourceType = resourceSectionList[1];
            if (string.IsNullOrEmpty(resourceType) == true) throw new InvalidOperationException($"{nameof(resourceType)}=null");

            // ResourcePath
            var resourcePath = String.Join("/", resourceSectionList.Skip(2));
            if (string.IsNullOrEmpty(resourcePath) == true) throw new InvalidOperationException($"{nameof(resourcePath)}=null");

            // Return
            return this.HasAccess(role, resourceProvider, resourceType, resourcePath);
        }

        public bool HasAccess(string role, string resourceProvider, string resourceType, string resourcePath)
        {
            #region Contracts

            if (string.IsNullOrEmpty(role) == true) throw new ArgumentException($"{nameof(role)}=null");
            if (string.IsNullOrEmpty(resourceProvider) == true) throw new ArgumentException($"{nameof(resourceProvider)}=null");
            if (string.IsNullOrEmpty(resourceType) == true) throw new ArgumentException($"{nameof(resourceType)}=null");
            if (string.IsNullOrEmpty(resourcePath) == true) throw new ArgumentException($"{nameof(resourcePath)}=null");

            #endregion

            // Variables
            var accessResult = false;

            // AccessResultRepository
            if (_accessResultRepository.TryGetValue(role, resourceProvider, resourceType, resourcePath, out accessResult) == true)
            {
                return accessResult;
            }
            accessResult = false;

            // PermissionList
            var permissionList = _permissionRepository.FindAllByRole(role, resourceProvider, resourceType);
            if (permissionList == null) throw new InvalidOperationException($"{nameof(permissionList)}=null");

            // HasAccess
            foreach (var permission in permissionList)
            {
                if (permission.HasAccess(role, resourceProvider, resourceType, resourcePath) == true)
                {
                    accessResult = true;
                    break;
                }
            }

            // AccessResultRepository
            _accessResultRepository.SetValue(role, resourceProvider, resourceType, resourcePath, accessResult);

            // Return
            return accessResult;
        }


        public bool HasAccess(List<string> roleList, string resourceUri)
        {
            #region Contracts

            if (roleList == null) throw new ArgumentException($"{nameof(roleList)}=null");
            if (string.IsNullOrEmpty(resourceUri) == true) throw new ArgumentException($"{nameof(resourceUri)}=null");

            #endregion

            // HasAccess
            foreach (var role in roleList)
            {
                if (this.HasAccess(role, resourceUri) == true)
                {
                    return true;
                }
            }

            // Return
            return false;
        }

        public bool HasAccess(List<string> roleList, string resourceProvider, string resourceType, string resourcePath)
        {
            #region Contracts

            if (roleList == null) throw new ArgumentException($"{nameof(roleList)}=null");
            if (string.IsNullOrEmpty(resourceProvider) == true) throw new ArgumentException($"{nameof(resourceProvider)}=null");
            if (string.IsNullOrEmpty(resourceType) == true) throw new ArgumentException($"{nameof(resourceType)}=null");
            if (string.IsNullOrEmpty(resourcePath) == true) throw new ArgumentException($"{nameof(resourcePath)}=null");

            #endregion

            // HasAccess
            foreach (var role in roleList)
            {
                if (this.HasAccess(role, resourceProvider, resourceType, resourcePath) == true)
                {
                    return true;
                }
            }

            // Return
            return false;
        }
    }
}
