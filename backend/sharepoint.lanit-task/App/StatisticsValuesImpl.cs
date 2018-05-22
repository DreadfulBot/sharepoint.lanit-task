using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace sharepoint.lanit_task.App
{
    class StatisticsValuesImpl : IStatisticsValues
    {
        private ListWorker _listWorker;

        public StatisticsValuesImpl()
        {
            _listWorker = new ListWorker();
        }

        public string getDate()
        {
            return DateTime.UtcNow.ToString("s") + "Z";
        }

        public string getUser(SPWeb web)
        {
            if (web.CurrentUser == null) throw new Exception("Undefined user");
            return "-1;#" + web.CurrentUser.LoginName;
        }

        public string getValue(SPWeb web, SPItemEventProperties props)
        {
            string valueInternalName = _listWorker.GetFieldInternalName(web, props.ListId, Settings.ValueFieldName);

            int oldValue = int.Parse(props.AfterProperties[valueInternalName].ToString());
            return (oldValue + 1).ToString();
        }
    }
}
