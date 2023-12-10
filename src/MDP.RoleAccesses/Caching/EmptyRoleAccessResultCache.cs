using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    [MDP.Registration.Service<RoleAccessResultCache>()]
    public class EmptyRoleAccessResultCache : RoleAccessResultCache
    {
        // Methods
        public void SetAccessResult(string role, string resourceProvider, string resourceType, string resourcePath, bool accessResult)
        {
            // Nothing

        }

        public bool TryGetAccessResult(string role, string resourceProvider, string resourceType, string resourcePath, out bool accessResult)
        {
            // Nothing
            accessResult = false;

            // Return
            return false;
        }
    }
}
