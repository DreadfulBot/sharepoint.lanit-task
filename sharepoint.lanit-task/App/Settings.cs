using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharepoint.lanit_task.App
{
    class Settings
    {
        private static string _ListName = "Статистика";
        public static string ListName { get { return _ListName; } set { } }

        private static string _SiteURL = "http://devsharepoint/";
        public static string SiteURL { get { return _SiteURL; } set { } }

        private static string _ListEditorsGroupName = "Редакторы статистики";
        public static string ListEditorsGroupName { get { return _ListEditorsGroupName; } set { } }
    }
}
