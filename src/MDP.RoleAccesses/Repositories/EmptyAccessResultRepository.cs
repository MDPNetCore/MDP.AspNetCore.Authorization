using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    [MDP.Registration.Service<AccessResultRepository>()]
    public class EmptyAccessResultRepository : AccessResultRepository
    {
        // Methods
        public void SetValue(string role, string resourceProvider, string resourceType, string resourcePath, bool accessResult)
        {
            // Nothing

        }

        public bool TryGetValue(string role, string resourceProvider, string resourceType, string resourcePath, out bool accessResult)
        {
            // Nothing
            accessResult = false;

            // Return
            return false;
        }
    }
}
