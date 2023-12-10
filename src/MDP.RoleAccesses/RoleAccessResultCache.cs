using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    public interface RoleAccessResultCache
    {
        // Methods
        public void SetAccessResult(string role, string resourceProvider, string resourceType, string resourcePath, bool accessResult);

        public bool TryGetAccessResult(string role, string resourceProvider, string resourceType, string resourcePath, out bool accessResult);
    }
}
