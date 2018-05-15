using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace sharepoint.lanit_task.App
{
    interface IStatisticsValues
    {
        string getValue(SPWeb web, SPItemEventProperties props);
        string getUser(SPWeb web, SPItemEventProperties props);
        string getDate(SPWeb web, SPItemEventProperties props);

    }
}
