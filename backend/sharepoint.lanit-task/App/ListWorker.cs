using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace sharepoint.lanit_task.App
{
    class ListWorker
    {
        public void ClearListGroups(SPWeb web, SPList list)
        {
            list.BreakRoleInheritance(true);

            // remove all existing roles
            int listCounter = list.RoleAssignments.Count;
            for(int i = 0; i < listCounter; i++)
            {
                SPRoleAssignment roleAssignment = list.RoleAssignments[0];
                list.RoleAssignments.RemoveById(roleAssignment.Member.ID);
            }
        }

        public string GetFieldInternalName(SPWeb web, Guid listId, string field)
        {
            return web.Lists[listId].Fields[field].InternalName;
        }
    }
}
