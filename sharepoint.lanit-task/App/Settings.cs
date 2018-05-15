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

        public static string[] _ListField = { "Значение", "Дата", "Пользователь" };

        public static string ValueFieldName { get { return _ListField[0]; } set { } }
        public static string DateFieldName { get { return _ListField[1]; } set { } }
        public static string UserFieldName { get { return _ListField[2]; } set { } }
    }
}
