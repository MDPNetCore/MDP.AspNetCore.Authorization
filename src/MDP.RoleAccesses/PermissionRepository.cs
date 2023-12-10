using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.RoleAccesses
{
    public interface PermissionRepository
    {
        // Methods
        List<Permission> FindAllByRole(string role, string resourceProvider, string resourceType);
    }
}
