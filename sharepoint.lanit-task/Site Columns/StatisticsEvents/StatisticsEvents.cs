using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using sharepoint.lanit_task.App;

namespace sharepoint.lanit_task.StatisticsEvents
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class StatisticsEvents : SPItemEventReceiver
    {
        private ListWorker _listWorker;
        private IStatisticsValues _statisticValues;
        public StatisticsEvents()
        {
            _listWorker = new ListWorker();
            _statisticValues = new StatisticsValuesImpl();
        }

        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPWeb web = properties.OpenWeb())
                    {
                        string valueInternalName = _listWorker.GetInternalFieldName(web, properties.ListId, Settings.ValueFieldName);
                        string dateInternalName = _listWorker.GetInternalFieldName(web, properties.ListId, Settings.DateFieldName);
                        string userInternalName = _listWorker.GetInternalFieldName(web, properties.ListId, Settings.UserFieldName);

                        properties.AfterProperties[valueInternalName] = _statisticValues.getValue(web, properties);
                        properties.AfterProperties[dateInternalName] = _statisticValues.getDate(web, properties);
                        properties.AfterProperties[userInternalName] = _statisticValues.getUser(web, properties);

                        base.ItemAdding(properties);
                    }
                });
            } catch (Exception e)
            {
                Logger.LogException(e);
            }
        }


    }
}