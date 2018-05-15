using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace sharepoint.lanit_task.App
{
    class RoleAssignmentWorker
    {
        protected SPRoleAssignment AssignGroup(SPWeb web, SPGroup group)
        {
            SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Contributor);
            SPRoleAssignment roleAssignment = new SPRoleAssignment(group);
            roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
            web.RoleAssignments.Add(roleAssignment);
            web.Update();
            return roleAssignment;
        }

        public SPRoleAssignment FindRoleAssignment(SPWeb web, string name)
        {
            SPRoleAssignment roleAssignment = null;
            try
            {
                SPGroup group = web.SiteGroups[name];
                roleAssignment = AssignGroup(web, group);
            } catch {

            }
            return roleAssignment;
        }

        public SPRoleAssignment CreateRoleAssignment(SPWeb web, string name)
        {
            web.SiteGroups.Add(
                name, 
                web.Author, 
                web.Author, 
                string.Format("User group for {0} object", name));

            Logger.LogMessage(string.Format("Group added {0}", name));

            SPGroup group = web.SiteGroups[name];
            SPRoleAssignment roleAssignment = AssignGroup(web, group);

            Logger.LogMessage(string.Format("Permissions for group {0} added succesfully", name));

            return roleAssignment;
        }
    }
}
