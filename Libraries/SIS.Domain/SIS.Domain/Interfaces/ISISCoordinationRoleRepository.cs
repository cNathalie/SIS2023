using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Domain.Interfaces
{
    public interface ISISCoordinationRoleRepository
    {
        public Dictionary<string, CoordinationRole> CoordinationRoles { get; }

        public Dictionary<string, CoordinationRole> RefreshCoordinationRoles();
        public bool Exists(CoordinationRole coordinationRole);
        public int Insert(CoordinationRole coordinationRole);
        public void Update(CoordinationRole coordinationRoleToUpdate, CoordinationRole newCoordintationRole);
        public void Delete(CoordinationRole coordinationRole);
    }
}
