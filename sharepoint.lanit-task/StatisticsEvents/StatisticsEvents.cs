using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace sharepoint.lanit_task.StatisticsEvents
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class StatisticsEvents : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {

            base.ItemAdding(properties);
        }


    }
}