using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    public class Permission
    {
        // Constants
        private static readonly string _doubleAsteriskString = Guid.NewGuid().ToString();


        // Fields
        private readonly Regex _permissionPattern = null;


        // Constructors
        public Permission(string permissionId, string role, string resourceProvider, string resourceType, string resourcePath)
        {
            #region Contracts

            if (string.IsNullOrEmpty(role) == true) throw new ArgumentException($"{nameof(role)}=null");
            if (string.IsNullOrEmpty(resourceProvider) == true) throw new ArgumentException($"{nameof(resourceProvider)}=null");
            if (string.IsNullOrEmpty(resourceType) == true) throw new ArgumentException($"{nameof(resourceType)}=null");
            if (string.IsNullOrEmpty(resourcePath) == true) throw new ArgumentException($"{nameof(resourcePath)}=null");

            #endregion

            // Default
            this.PermissionId = permissionId;
            this.Role = role;
            this.ResourceProvider = resourceProvider;
            this.ResourceType = resourceType;
            this.ResourcePath = resourcePath;

            // PermissionPattern
            _permissionPattern = this.ConvertToPermissionPattern(resourceProvider, resourceType, resourcePath);
            if (_permissionPattern == null) throw new InvalidOperationException($"{nameof(_permissionPattern)}=null");
        }


        // Properties
        public string PermissionId { get; }

        public string Role { get; }

        public string ResourceProvider { get; }

        public string ResourceType { get; }

        public string ResourcePath { get; }


        // Methods
        public bool HasAccess(string role, string resourceProvider, string resourceType, string resourcePath)
        {
            #region Contracts

            if (string.IsNullOrEmpty(role) == true) throw new ArgumentException($"{nameof(role)}=null");
            if (string.IsNullOrEmpty(resourceProvider) == true) throw new ArgumentException($"{nameof(resourceProvider)}=null");
            if (string.IsNullOrEmpty(resourceType) == true) throw new ArgumentException($"{nameof(resourceType)}=null");
            if (string.IsNullOrEmpty(resourcePath) == true) throw new ArgumentException($"{nameof(resourcePath)}=null");

            #endregion

            // Role
            if (role.Equals(this.Role, StringComparison.OrdinalIgnoreCase) == false) return false;

            // ResourcePath
            if (resourcePath.StartsWith("/") == true) resourcePath = resourcePath.Substring(1);

            // ResourceUri
            var resourceUri = string.Empty;
            resourceUri = $"{resourceProvider}/{resourceType}/{resourcePath}";
            resourceUri = resourceUri.ToLower();

            // IsMatch
            if (_permissionPattern.IsMatch(resourceUri) == true) return true;

            // Return
            return false;
        }

        private Regex ConvertToPermissionPattern(string resourceProvider, string resourceType, string resourcePath)
        {
            #region Contracts

            if (string.IsNullOrEmpty(resourceProvider) == true) throw new ArgumentException($"{nameof(resourceProvider)}=null");
            if (string.IsNullOrEmpty(resourceType) == true) throw new ArgumentException($"{nameof(resourceType)}=null");
            if (string.IsNullOrEmpty(resourcePath) == true) throw new ArgumentException($"{nameof(resourcePath)}=null");

            #endregion

            // ResourcePath
            if (resourcePath.StartsWith("/") == true) resourcePath = resourcePath.Substring(1);

            // PermissionPattern
            var permissionPattern = string.Empty;
            permissionPattern = $"{resourceProvider}/{resourceType}/{resourcePath}";
            permissionPattern = permissionPattern.ToLower();

            // Convert
            permissionPattern = permissionPattern.Replace("**", _doubleAsteriskString);
            permissionPattern = permissionPattern.Replace("*", "[^/]*");
            permissionPattern = permissionPattern.Replace(_doubleAsteriskString, ".*");
            permissionPattern = "^" + permissionPattern + "$";

            // Return
            return new Regex(permissionPattern);
        }
    }
}
