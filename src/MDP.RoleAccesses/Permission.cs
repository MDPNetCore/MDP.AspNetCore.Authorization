using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    public partial class Permission : IValidatableObject
    {
        // Constants
        private static readonly string _doubleAsteriskString = Guid.NewGuid().ToString();


        // Properties
        public string PermissionId { get; set; }

        public string Role { get; set; }

        public string ResourceProvider { get; set; }

        public string ResourceType { get; set; }

        public string ResourcePath { get; set; }


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

            // PermissionPattern
            var permissionPattern = this.ConvertToPermissionPattern(this.ResourceProvider, this.ResourceType, this.ResourcePath);
            if (permissionPattern == null) throw new InvalidOperationException($"{nameof(permissionPattern)}=null");

            // PermissionMatch
            if (permissionPattern.IsMatch(resourceUri) == true) return true;

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

    public partial class Permission : IValidatableObject
    {
        // Methods
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region Contracts

            if (validationContext == null) throw new ArgumentException($"{nameof(validationContext)}=null");

            #endregion

            // PermissionId
            if (string.IsNullOrEmpty(this.PermissionId) == true) yield return new ValidationResult($"{nameof(this.PermissionId)}=null", new[] { nameof(this.PermissionId) });

            // Role
            if (string.IsNullOrEmpty(this.Role) == true) yield return new ValidationResult($"{nameof(this.Role)}=null", new[] { nameof(this.Role) });

            // ResourceProvider
            if (string.IsNullOrEmpty(this.ResourceProvider) == true) yield return new ValidationResult($"{nameof(this.ResourceProvider)}=null", new[] { nameof(this.ResourceProvider) });

            // ResourceType
            if (string.IsNullOrEmpty(this.ResourceType) == true) yield return new ValidationResult($"{nameof(this.ResourceType)}=null", new[] { nameof(this.ResourceType) });

            // ResourcePath
            if (string.IsNullOrEmpty(this.ResourcePath) == true) yield return new ValidationResult($"{nameof(this.ResourcePath)}=null", new[] { nameof(this.ResourcePath) });
        }
    }
}
