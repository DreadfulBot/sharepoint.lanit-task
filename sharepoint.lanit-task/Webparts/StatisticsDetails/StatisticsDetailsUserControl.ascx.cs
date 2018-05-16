using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using sharepoint.lanit_task.App;
using System.Web;

namespace sharepoint.lanit_task.Webparts.StatisticsDetails
{
    public partial class StatisticsDetailsUserControl : UserControl
    {
        private ListWorker _listWorker;
        protected void Page_Load(object sender, EventArgs e)
        {
            _listWorker = new ListWorker();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    if(!numberRequireValidator.IsValid && !dateCompareValidator.IsValid)
                    {
                        throw new Exception("Input data validation failed");
                    }

                    SPWeb web = SPContext.Current.Web;

                    SPListItem listItem = web.Lists[Settings.ListName].Items.Add();

                    string valueInternalName = _listWorker.GetFieldInternalName(web, web.Lists[Settings.ListName].ID, Settings.ValueFieldName);
                    string dateInternalName = _listWorker.GetFieldInternalName(web, web.Lists[Settings.ListName].ID, Settings.DateFieldName);

                    listItem[valueInternalName] = txtNumber.Text;
                    listItem[dateInternalName] = dtcDate.SelectedDate;
                    listItem.Update();

                    //Page.ClientScript.RegisterStartupScript(
                    //    this.GetType(),
                    //    "StatisticDetails",
                    //    string.Format("showReponse({0}, '{1}');", "true", "Новый элемент списка успешно создан"),
                    //    true);

                    UriBuilder uriBuilder = new UriBuilder(Page.Request.Url.OriginalString);
                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    query["success"] = "1";
                    query["message"] = "Элемент списка успешно создан";
                    uriBuilder.Query = query.ToString();

                    Response.Redirect(uriBuilder.ToString());
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                //Page.ClientScript.RegisterStartupScript(
                //    this.GetType(), 
                //    "StatisticDetails", 
                //    string.Format("showReponse({0}, {1});", "false", "Не удалось создать элемент. Подробности см. в системном журнале"), 
                //    true);
            }
        }
    }
}
