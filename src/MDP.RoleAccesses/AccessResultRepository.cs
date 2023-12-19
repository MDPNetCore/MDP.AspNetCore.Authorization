using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    public interface AccessResultRepository
    {
        // Methods
        public void SetValue(string role, string resourceProvider, string resourceType, string resourcePath, bool accessResult);

        public bool TryGetValue(string role, string resourceProvider, string resourceType, string resourcePath, out bool accessResult);
    }
}
