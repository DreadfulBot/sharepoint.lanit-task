using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using sharepoint.lanit_task.App;
using System.Web;

namespace sharepoint.lanit_task.Webparts.StatisticsDetails
{
    public partial class StatisticsDetailsUserControl : UserControl
    {
        private ListWorker _listWorker;
        private IStatisticsValues _statisticValues;
        protected void Page_Load(object sender, EventArgs e)
        {
            _listWorker = new ListWorker();
            _statisticValues = new StatisticsValuesImpl();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!numberRequireValidator.IsValid && !dateCompareValidator.IsValid)
                {
                    throw new Exception("Input data validation failed");
                }

                SPWeb oldContext = SPContext.Current.Web;

                SPUtility.ValidateFormDigest();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using(SPSite site = new SPSite(Settings.SiteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.GetList("/Lists/" + Settings.ListName);
                            SPListItem listItem = list.AddItem();

                            string valueInternalName =
                                _listWorker.GetFieldInternalName(web, list.ID, Settings.ValueFieldName);

                            string dateInternalName =
                                _listWorker.GetFieldInternalName(web, list.ID, Settings.DateFieldName);

                            string userInternalName =
                                _listWorker.GetFieldInternalName(web, list.ID, Settings.UserFieldName);

                            listItem[valueInternalName] = txtNumber.Text;
                            listItem[dateInternalName] = dtcDate.SelectedDate;
                            listItem[userInternalName] = _statisticValues.getUser(oldContext);
                            listItem.Update();

                            UriBuilder uriBuilder = new UriBuilder(Page.Request.Url.OriginalString);
                            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                            query["success"] = "1";
                            query["message"] = "Элемент списка успешно создан";
                            uriBuilder.Query = query.ToString();

                            Response.Redirect(uriBuilder.ToString());
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
