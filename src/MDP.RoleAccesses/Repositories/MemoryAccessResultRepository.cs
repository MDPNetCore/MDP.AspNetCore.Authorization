using CLK.Caching.Memory;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    [MDP.Registration.Service<AccessResultRepository>()]
    public class MemoryAccessResultRepository : AccessResultRepository
    {
        // Fields
        private readonly ExpirationMemoryCache _accessResultCache = new ExpirationMemoryCache();


        // Methods
        public void SetValue(string role, string resourceProvider, string resourceType, string resourcePath, bool accessResult)
        {
            #region Contracts

            if (string.IsNullOrEmpty(role) == true) throw new ArgumentException($"{nameof(role)}=null");
            if (string.IsNullOrEmpty(resourceProvider) == true) throw new ArgumentException($"{nameof(resourceProvider)}=null");
            if (string.IsNullOrEmpty(resourceType) == true) throw new ArgumentException($"{nameof(resourceType)}=null");
            if (string.IsNullOrEmpty(resourcePath) == true) throw new ArgumentException($"{nameof(resourcePath)}=null");

            #endregion

            // CacheKey
            var cacheKey = $"{role}/{resourceProvider}/{resourceType}/{resourcePath}";
            if (string.IsNullOrEmpty(cacheKey) == true) throw new InvalidOperationException($"{nameof(cacheKey)}=null");

            // SetValue
            _accessResultCache.SetValue(cacheKey, accessResult);
        }

        public bool TryGetValue(string role, string resourceProvider, string resourceType, string resourcePath, out bool accessResult)
        {
            #region Contracts

            if (string.IsNullOrEmpty(role) == true) throw new ArgumentException($"{nameof(role)}=null");
            if (string.IsNullOrEmpty(resourceProvider) == true) throw new ArgumentException($"{nameof(resourceProvider)}=null");
            if (string.IsNullOrEmpty(resourceType) == true) throw new ArgumentException($"{nameof(resourceType)}=null");
            if (string.IsNullOrEmpty(resourcePath) == true) throw new ArgumentException($"{nameof(resourcePath)}=null");

            #endregion

            // CacheKey
            var cacheKey = $"{role}/{resourceProvider}/{resourceType}/{resourcePath}";
            if (string.IsNullOrEmpty(cacheKey) == true) throw new InvalidOperationException($"{nameof(cacheKey)}=null");

            // TryGetValue
            return _accessResultCache.TryGetValue(cacheKey, out accessResult);
        }
    }
}
