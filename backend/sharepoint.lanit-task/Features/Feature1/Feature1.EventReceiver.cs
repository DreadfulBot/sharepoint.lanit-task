using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using sharepoint.lanit_task.App;

namespace sharepoint.lanit_task.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("015576ea-5116-46a6-aae8-ff01a2656eae")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        private RoleWorker _roleWorker;
        private ListWorker _listWorker;
        public Feature1EventReceiver()
        {
            _roleWorker = new RoleWorker();
            _listWorker = new ListWorker();
        }

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSite site = properties.Feature.Parent as SPSite;

                Logger.LogMessage("site ok");

                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists.TryGetList(Settings.ListName);

                    if (list == null)
                    {
                        throw new Exception(string.Format("Unable to find list {0}", Settings.ListName));
                    }

                    // some security settings
                    list.RestrictUserUpdates = true;

                    // remove all roles from list
                    _listWorker.ClearListGroups(web, list);

                    // creating group, containig administators
                    // for new list
                    SPRoleAssignment roleAssignment = _roleWorker.FindRoleAssignment(web, Settings.ListEditorsGroupName);
                    if(roleAssignment == null)
                    {
                        roleAssignment = _roleWorker.CreateRoleAssignment(web, Settings.ListEditorsGroupName);
                    }

                    list.RoleAssignments.Add(roleAssignment);

                    Logger.LogMessage(
                        string.Format("New role {0} were added and assigned to list {1}", 
                        Settings.ListEditorsGroupName,
                        Settings.ListName
                        ));
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
