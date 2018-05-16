using Microsoft.SharePoint.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using sharepoint.lanit_task.App.Models;
using sharepoint.lanit_task.App;
using Microsoft.SharePoint;

namespace sharepoint.lanit_task.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class StatisticsServices : IStatisticsServices
    {
        private IStatisticsValues _statisticsValues;
        private ListWorker _listWorker;
        public StatisticsServices()
        {
            _statisticsValues = new StatisticsValuesImpl();
            _listWorker = new ListWorker();
        }


        public List<StatisticsItem> GetCurrentUserStatistics()
        {
            if(SPContext.Current.Web.CurrentUser == null)
            {
                throw new Exception("User must be logged in");
            }

            List<StatisticsItem> result = new List<StatisticsItem>();
            try
            {
                using (SPWeb web = SPContext.Current.Web)
                {
                    string user = _statisticsValues.getUser(web, null);
                    SPList list = web.Lists.TryGetList(Settings.ListName);

                    string internalUserField = _listWorker.GetFieldInternalName(web, list.ID, Settings.UserFieldName);
                    string internalDateField = _listWorker.GetFieldInternalName(web, list.ID, Settings.DateFieldName);
                    string internalValueField = _listWorker.GetFieldInternalName(web, list.ID, Settings.ValueFieldName);


                    SPQuery query = new SPQuery();
                    query.Query = string.Concat(
                        "<Where><Eq>",
                            "<FieldRef Name='" + internalUserField + "' LookupId='True'/>",
                            "<Value Type='User'>" + web.CurrentUser.ID + "</Value>",
                        "</Eq></Where>"
                        );

                    SPListItemCollection items = list.GetItems(query);
                    
                    foreach(SPListItem e in items)
                    {
                        result.Add(new StatisticsItem
                        {
                            Date = e[internalDateField].ToString(),
                            User = e[internalUserField].ToString(),
                            Value = e[internalValueField].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return result;
        }

        public List<StatisticsItem> GetUserStatistics(string userLogin)
        {
            throw new NotImplementedException();
        }

        public string HelloWorld()
        {
            return "Hello, world!";
        }
    }
}